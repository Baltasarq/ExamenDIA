#! /bin/env python
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
    "version": "v0.2 20171022"
}

ExcludedFilesByExtension = [ ".exe", ".dll", ".obj", ".pdb", ".userprefs" ]


ExamFolder = "apellidos_nombre-dni"
Executable = "Codigo/bin/Debug/Codigo.exe"
DataFile = "exam_data.json"
ZipFileNamePrefix = "zipped_exam_"
SolutionFileName = "ExamenDIA.sln"
InfoClassFolder = os.path.join(ExamFolder, "Codigo")
InfoClassFile = os.path.join(InfoClassFolder, "Info.cs")


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
    def __init__(self, surname, name, dni, dni_letter, email):
        name = name.strip().lower()
        surname = surname.strip().lower()
        dni = str(dni).strip()
        dni_letter = dni_letter.strip().upper()
        email = email.strip().lower()

        if len(name) < 2:
            raise Exception("too short name")

        if len(surname) < 2:
            raise Exception("too short surname")

        if len(email) < 3:
            raise Exception("too short email")

        if email.count('@') != 1:
            raise Exception("email should have just one '@'")

        if len(dni_letter) != 1:
            raise Exception("dni letter should have exactly length 1")

        self._name = name[0].upper() + name[1:]
        parts = surname.split()
        self._surname = " ".join([w[0].upper() + w[1:] for w in parts])
        self._surname = self._surname.strip()
        self._dni = str(int(dni)) + dni_letter
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
    def dni(self):
        return int(self._dni[:-1])

    @property
    def dni_letter(self):
        return self._dni[-1]

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

    def to_json(self, file_name):
        with open(file_name, "wt") as f:
            json.dump(self.to_dict(), f)

        return

    @staticmethod
    def from_dict(data):
        toret = UserData(   data["surname"], data["name"],
                         data["dni"][:-1], data["dni"][-1], data["email"])
        return toret

    @staticmethod
    def from_json(file_name):
        with open(file_name, "rU") as f:
            data = json.load(f)
        return UserData.from_dict(data)

    @staticmethod
    def default():
        return UserData( "Palotes", "Perico", "1", "D", "perico@palotes.com" )

    def __str__(self):
        return self.full_name() + "(" + self.full_dni()+ "): " + self.email


def ask_for(msg, default_value, only_digits=False, max_length=128):
    toret = ""
    correct = False

    while not correct:
        toret = input(msg + "[" + str(default_value) + "]: ").strip()

        if not toret:
            toret = default_value

        correct = True if len(toret) > 0 else False
        correct = True if toret.isdigit() else correct
        correct = False if len(toret) > max_length else correct

    return toret


def ask_user_data(user_data):
    correct = False

    while not correct:
        print()

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
        email = ask_for("Dame tu email: ", user_data.email)

        try:
            user_data = UserData(surname, name, dni, dni_letter, email)
            print(user_data)

            str_correct = ask_for("Es correcto (s/n): ",
                                  "S",
                                  only_digits=False,
                                  max_length=1).upper()
            correct = (str_correct[0] == 'S')
        except Exception as e:
            print("Error collecting user data: " + e.args[0])
            correct = False

    return user_data


def build_status(status, msg):
    toret = "["

    if status:
        toret += "OKAY] ... "
    else:
        toret += "FAIL] ... "

    return toret + msg


def locate_builder():
    """Determines the builder, msbuild or xbuild.
        :return: The name of builder executable, as a string, or None.
    """
    toret = "xbuild"
    retcode = subprocess.call(
                    toret + " /h",
                    stdout=subprocess.DEVNULL,
                    stderr=subprocess.DEVNULL,
                    shell=True)

    print(build_status(retcode == 0, "Trying to locate: " + toret))

    if retcode != 0:
        toret = "msbuild"
        retcode = subprocess.call(
                    toret + " /h",
                    stdout=subprocess.DEVNULL,
                    stderr=subprocess.DEVNULL,
                    shell=True)
        print(build_status(retcode == 0, "Trying to locate: " + toret))

    if retcode != 0:
        print(build_status(False, "No builder found."))
        toret = None

    return toret

def is_mono_present():
    """Determines whether the mono vm is available.
        :return: The name of builder executable, as a string, or an empty string.
    """
    retcode = subprocess.call(
                    "mono --help",
                    stdout=subprocess.DEVNULL,
                    stderr=subprocess.DEVNULL,
                    shell=True)

    toret = (retcode == 0)
    print(build_status(toret, "Trying to locate: mono"))
    return toret

def run(user_data):
    cmd = os.path.join(user_data.folder_name, Executable)

    print("Executing: " + cmd)

    # If mono is not present, will try to execute the program directly
    if is_mono_present():
        cmd = "mono " + cmd

    print()
    retcode = subprocess.call(cmd, shell=True)

    print(build_status(retcode == 0, "Executing: " + cmd))


def build(user_data):
    builder = locate_builder()
    solution_file_path = os.path.join(user_data.folder_name, SolutionFileName)

    if builder:
        print("Building: " + solution_file_path + "...")
        retcode = subprocess.call(builder + " " + solution_file_path, shell=True)
        print(build_status(retcode == 0, "Building."))


def pak_zip(user_data):
    fileName = ZipFileNamePrefix + user_data.full_dni() + ".zip";
    path = user_data.folder_name

    print("Zipping exam to file: " + fileName + "...\n")

    with zipfile.ZipFile(fileName, 'w') as zip_handle:
        path = path.rstrip("\\/")
        for root, dirs, files in os.walk(path):
            for file in files:
                ext = os.path.splitext(file)[ 1 ]
                if ext not in ExcludedFilesByExtension:
                    filePath = os.path.join(root, file)
                    zip_handle.write(filePath, filePath)


def prep(user_data):
    """Prepares the files for the exam."""
    print("Prepping...\n")

    # Locate important files
    exam_folder_located = os.path.isdir(ExamFolder)
    infoclass_folder_located = os.path.isdir(InfoClassFolder)

    # Report the user
    print(build_status(exam_folder_located, "Locating the exam directory"))
    print(build_status(infoclass_folder_located, "Locating folder for Info.cs"))

    if infoclass_folder_located:
        user_data = ask_user_data(user_data)
        with open(InfoClassFile, "wt") as f:
            f.write("namespace Codigo {\n    public static class Info {\n    "
                + "    public const string Nombre = \""
                + user_data.full_name() + "\";\n        "
                + "public const string Email = \""
                + user_data.email + "\";\n        "
                + "public const int Dni = "
                + str(user_data.dni) + ";\n        "
                + "public const char LetraDni = '"
                + user_data.dni_letter + "';\n    }\n}\n")
        print(build_status(True, "Info class file created"))

    if exam_folder_located:
        renamed = True
        try:
            os.rename(ExamFolder, user_data.folder_name)
        except OSError as e:
            renamed = False

        print(build_status(renamed, "Directory renamed to: "
                                    + user_data.folder_name))

    if infoclass_folder_located:
        user_data.to_json(DataFile)
        print(build_status(True, "User data stored"))

    return


def load_data():
    toret = UserData.default()

    if os.path.isfile(DataFile):
        toret = UserData.from_json(DataFile)

    return toret


def build_header():
    return AppInfo["name"] + " " + AppInfo["version"] + "\n"


def build_help():
    return  "\nHelp:\nexam [cmd]\n\nCommands:\n\
             help: Shows this help and exits.\n\
             prep: Prepares the files for the exam.\n\
             build: Builds the whole solution.\n\
             run: Runs the whole solution.\n\
             zip : Zips the exam to a single file for sending it.\n"


def parse_options(args, user_data):
    toret = lambda: print(build_help())

    if len(args) > 1:
        if args[1] == "prep":
            toret = lambda: prep(user_data)
        elif args[1] == "zip":
            toret = lambda: pak_zip(user_data)
        elif args[1] == "build":
            toret = lambda: build(user_data)
        elif args[1] == "run":
            toret = lambda: run(user_data)

    return toret


if __name__ == "__main__":
    set_input_function()
    user_data = load_data()
    print(build_header())

    if sys.version_info[0] >= 3:
        cmd = parse_options(sys.argv, user_data)
        cmd()
    else:
        print("\nCRITICAL: Needs Python 3.\n")
