#! /bin/env python3
# encoding: utf-8

# exam - Exam baker


from __future__ import print_function

import sys
import os
import json
import zipfile
import unicodedata
import subprocess


AppInfo = {
    "name": "exam",
    "version": "v0.3 20171105"
}

ExcludedFilesByExtension = [
    ".ini", ".db",                                          # Windows
    ".DS_Store",                                            # Mac
    ".bak", ".old",                                         # Backup
    ".dex", ".apk", ".ap_", ".kt~", ".kts~", "ktm~",        # Android
    ".com", ".so", ".lib", ".tmp", ".obj", ".out",
    ".o", ".a", ".lai", ".la", ".dylib", ".dll", ".asm~",   # Assembler
    ".exe", ".pdb", ".userprefs",
    ".user", ".suo", ".nupkg", ".pidb", ".cs~",             # C#
    ".pch", ".slo", ".lo", ".c~", ".cpp~",                  # C/C++
    ".mod", ".smod",                                        # Fortran
    ".jar", ".class", ".war", ".ear", ".ctxt", ".java~",    # Java
    "ts~", ".js~", ".html~", ".css~",                       # JavaScript
    ".bpi", ".bpl", ".map", ".dres", ".ocx", "pas~",        # Pascal (Delphi)
    ".pyc", "pyw", ".cache", ".env", ".py~"                 # Python
]


DataFile = "usr_exam_data.json"
MarksFile = "notas.txt"
InfoFile = "usr_exam_data.txt"
ZipFileNamePrefix = "zip_exam-"


def set_input_function():
    global input

    if sys.version_info[0] < 3:
        input = raw_input


def remove_accents(input_str):
    nkfd_form = unicodedata.normalize('NFKD',
                                      input_str.encode("utf-8", "ignore")
                                      .decode("ascii", "ignore"))
    return u"".join([c for c in nkfd_form if not unicodedata.combining(c)])


class UserData:
    def __init__(self, surname, name, dni_prefix, dni, dni_letter, email):
        name = name.strip().lower()
        surname = surname.strip().lower()
        dni = str(dni).strip()
        dni_letter = dni_letter.strip().upper()
        email = email.strip().lower()

        if len(name) < 2:
            raise Exception("nombre demasiado corto")

        if len(surname) < 2:
            raise Exception("apellido demasiado corto")

        if len(email) < 3:
            raise Exception("e.mail demasiado corto")

        if email.count('@') != 1:
            raise Exception("e.mail solo tiene que tener una '@'")

        if len(dni) < 8:
            raise Exception("DNI demasiado corto")

        if len(dni_letter) != 1:
            raise Exception("la letra del DNI debe ser de longitud exacta 1")

        self._name = name[0].upper() + name[1:]
        parts = surname.split()
        self._surname = " ".join([w[0].upper() + w[1:] for w in parts])
        self._surname = self._surname.strip()
        self._dni = dni_prefix + str(int(dni)) + dni_letter
        self._email = email

    @property
    def name(self):
        return self._name

    @property
    def surname(self):
        return self._surname

    @property
    def email(self):
        return self._email

    @property
    def dni_prefix(self):
        return UserData.decompose_dni(self._dni)[0]

    @property
    def dni(self):
        return UserData.decompose_dni(self._dni)[1]

    @property
    def dni_letter(self):
        return UserData.decompose_dni(self._dni)[2]

    @property
    def folder_name(self):
        return remove_accents(self.surname.lower().replace(' ', '_')
                + "_" + self.name.lower().replace(' ','_')
                + "-"+ self.full_dni())

    def full_dni(self):
        return self._dni

    def full_name(self):
        return self.surname + ", " + self.name

    def to_dict(self):
        toret = {}

        toret["dni"] = self.full_dni()
        toret["name"] = self.name
        toret["surname"] = self.surname
        toret["email"] = self.email

        return toret

    def to_json(self):
        return json.dumps(self.to_dict())

    @staticmethod
    def decompose_dni(dni):
        """Return DNI decomposed in parts
            :return: A tuple with three elements: prefix, dni, letter
        """
        prefix = ""
        dni_start = 0

        for i, x in enumerate(dni):
            if x.isalpha():
                prefix += x
            else:
                dni_start = i
                break

        return tuple([dni[:dni_start], dni[dni_start:-1], dni[-1]])

    @staticmethod
    def from_dict(data):
        dec_dni = UserData.decompose_dni(data["dni"])
        toret = UserData(data["surname"], data["name"],
                         dec_dni[0], dec_dni[1], dec_dni[2],
                         data["email"])
        return toret

    @staticmethod
    def from_json(str_data):
        return UserData.from_dict(json.loads(str_data))

    @staticmethod
    def default():
        return UserData( "apellidos", "nombre", "", "12345678", "D", "e@mail" )

    def __str__(self):
        return self.full_name() + "(" + self.full_dni()+ "): " + self.email


def ask_for(msg, default, only_digits=False, max_length=128, min_length=1):
    toret = ""
    possibilities = "]"

    if min_length <= 0:
        possibilities = "/-]"

    correct = False
    while not correct:
        toret = input(msg + "[" + str(default) + possibilities + ": ").strip()

        if not toret:
            toret = default

        if (min_length < 0
        and toret == "-"):
            toret = ""

        correct = (len(toret) >= min_length and len(toret) <= max_length)

        if (correct
        and only_digits):
            correct = toret.isdigit()

    return toret


def ask_user_data(user_data):
    correct = False

    while not correct:
        print()

        dni_prefix = ask_for("Dame la letra prefijo de tu DNI: ",
                      user_data.dni_prefix,
                      min_length=-1,
                      max_length=1).upper()
        dni = ask_for("Dame tu DNI (solo digitos): ",
                      str(user_data.dni),
                      only_digits=True,
                      max_length=10)
        dni_letter = ask_for("Dame la letra de tu DNI: ",
                             user_data.dni_letter,
                             only_digits=False,
                             max_length=1).upper()
        name = ask_for("Dame tu nombre propio: ", user_data.name)
        surname = ask_for("Dame tus apellidos: ", user_data.surname)
        email = ask_for("Dame tu e.mail: ", user_data.email)

        try:
            user_data = UserData(surname, name,
                                 dni_prefix, dni, dni_letter, email)
            print(user_data)

            str_correct = ask_for("Es correcto (s/n): ",
                                  "S",
                                  only_digits=False,
                                  max_length=1).upper()
            correct = (str_correct[0] == 'S')
        except Exception as e:
            print("Error recogiendo datos del usuario: " + e.args[0])
            correct = False

    print()
    return user_data


def build_status(status, msg):
    toret = "["

    if status:
        toret += "OK] ... "
    else:
        toret += "ERROR] ... "

    return toret + msg

def create_marks_file(user_data, file_name):
    with open(file_name, "wt") as f:
        f.write("\n===\n")
        f.write(str.format("DNI: {0}\n", user_data.full_dni()))
        f.write(str.format("Nombre: {0}, {1}\n",
                              user_data.surname, user_data.name))
        f.write(str.format("e.Mail: {0}", user_data.email))
        f.write("\n===\n\n")
        for i in range(1, 7):
            f.write(str.format("Ejercicio {0:2d}: \n", i))
        f.write("\n\nNota: \n")

def create_info_file(user_data, file_name):
    with open(file_name, "wt") as f:
        f.write(user_data.full_dni() + "\n")
        f.write(user_data.name + "\n")
        f.write(user_data.surname + "\n")
        f.write(user_data.email + "\n")

def create_data_file(user_data, file_name):
    with open(file_name, "wt") as f:
        f.write(user_data.to_json())
        f.write("\n")


def retrieve_data_file(file_name):
    with open(file_name, "rU") as f:
        return UserData.from_json(str.join("\n", f.readlines()))


def pak_zip(user_data):
    fileName = ZipFileNamePrefix + user_data.full_dni() + ".zip";
    path = user_data.folder_name

    print("Empaquetando examen en archivo zip: " + fileName + "...\n")

    with zipfile.ZipFile(fileName, 'w') as zip_handle:
        path = path.rstrip("\\/")
        for root, dirs, files in os.walk(path):
            for file in files:
                ext = os.path.splitext(file)[ 1 ]
                if ext not in ExcludedFilesByExtension:
                    filePath = os.path.join(root, file)
                    zip_handle.write(filePath, filePath)


def chk_info(user_data):
    """Checks the user info to be correct."""
    print("Info...\n\n")
    print(str(user_data) + "\n\n")

    folder_name = user_data.folder_name
    print(build_status(os.path.isdir(folder_name),
                       "Localizando el directorio del examen"))

    print(build_status(os.path.isfile(DataFile),
                       "Localizando info del usuario"))

    print(build_status(os.path.isfile(os.path.join(folder_name, InfoFile)),
                       "Localizando el archivo de datos del usuario"))

    print(build_status(os.path.isfile(os.path.join(folder_name, MarksFile)),
                       "Localizando el archivo de notas"))

def reset(user_data):
    """Restes to defaults."""
    old_folder_name = user_data.folder_name
    user_data = UserData.default()

    print("Ini...\n")

    # Locate exam folder
    exam_folder_located = os.path.isdir(old_folder_name)
    print(build_status(exam_folder_located,
                       "Localizando el directorio del examen"))

    # Rename folder, create data and info files
    if exam_folder_located:
        # Rename folder
        folder_name = user_data.folder_name
        if old_folder_name != folder_name:
            renamed = True

            try:
                os.rename(old_folder_name, folder_name)
            except OSError as e:
                renamed = False

            print(build_status(renamed,
                               "Directorio renombrado:\n    "
                               + old_folder_name + "\n  ->" + folder_name))

        create_data_file(user_data, DataFile)
        print(build_status(True, "Datos de usuario guardados"))

        create_info_file(user_data, os.path.join(folder_name, InfoFile))
        print(build_status(True, "Creado archivo info"))

        create_marks_file(user_data, os.path.join(folder_name, MarksFile))
        print(build_status(True, "Creado archivo de notas"))

    return


def prep(user_data):
    """Prepares the files for the exam."""
    print("Prep...\n")

    # Locate exam folder
    old_folder_name = user_data.folder_name
    exam_folder_located = os.path.isdir(old_folder_name)
    print(build_status(exam_folder_located,
                       "Localizando el directorio del examen"))

    # Rename folder, create data and info files
    if exam_folder_located:
        # Collect user data
        user_data = ask_user_data(user_data)

        # Rename folder
        folder_name = user_data.folder_name
        if old_folder_name != folder_name:
            renamed = True

            try:
                os.rename(old_folder_name, folder_name)
            except OSError as e:
                renamed = False

            print(build_status(renamed,
                               "Directorio renombrado:\n    "
                               + old_folder_name + "\n  ->" + folder_name))

        create_data_file(user_data, DataFile)
        print(build_status(True, "Datos de usuario guardados"))

        create_info_file(user_data, os.path.join(folder_name, InfoFile))
        print(build_status(True, "Creado archivo info"))

        create_marks_file(user_data, os.path.join(folder_name, MarksFile))
        print(build_status(True, "Creado archivo de notas"))

    return


def create_or_load_data():
    toret = UserData.default()

    if os.path.isfile(DataFile):
        toret = retrieve_data_file(DataFile)

    return toret


def build_header():
    return AppInfo["name"] + " " + AppInfo["version"] + "\n"


def build_help():
    return  "\nHelp:\nexam [cmd]\n\nCommands:\n\
             help: Muestra esta ayuda y termina.\n\
             ini : Recomienza con datos por defecto.\n\
             info: Comprueba la info existente.\n\
             prep: Prepara el examen sobre la info del usuario.\n\
             zip : Empaqueta el examen en un solo archivo zip.\n"


def parse_options(args, user_data):
    toret = lambda: print(build_help())

    if len(args) > 1:
        if args[1] == "prep":
            toret = lambda: prep(user_data)
        elif args[1] == "ini":
            toret = lambda: reset(user_data)
        elif args[1] == "info":
            toret = lambda: chk_info(user_data)
        elif args[1] == "zip":
            toret = lambda: pak_zip(user_data)

    return toret


if __name__ == "__main__":
    set_input_function()
    user_data = create_or_load_data()
    print(build_header())

    if sys.version_info[0] >= 3:
        cmd = parse_options(sys.argv, user_data)
        cmd()
    else:
        print("\nCRITICAL: Needs Python 3.\n")
