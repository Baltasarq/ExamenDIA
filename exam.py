#! /bin/env python
# encoding: utf-8

# exam - Exam baker

from __future__ import print_function

import sys
import os
import unicodedata


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
    
    def __str__(self):
        return self.full_name() + "(" + self.full_dni()+ "): " + self.email


ExamFolder = "apellidos_nombre-dni"
InfoClassFile = os.path.join(ExamFolder, os.path.join("Codigo", "Info.cs"))


def ask_user_data():
    toret = None
    correct = False
    
    while not correct:
        print()
        dni = input("Dame tu DNI (solo digitos): ").strip()
        dni_letter = input("Dame la letra de tu DNI: ").strip().upper()
        name = input("Dame tu nombre propio: ").strip()
        surname = input("Dame tus apellidos: ").strip()
        email = input("Dame tu email: ").strip()
        
        try:
            toret = UserData(surname, name, dni, dni_letter, email)
            print(toret)
        
            str_correct = input("Es correcto (S/n): ").strip()
            correct = (len(str_correct) == 0
                        or str_correct[0].upper() == 'S')
        except Exception as e:
            print("Error collecting user data: " + e.args[0])
            correct = False
        
    return toret


def build_status(status, msg):
    toret = "["
    
    if status:
        toret += "OKAY] ... "
    else:
        toret += "FAIL] ... "
        
    return toret + msg


def prep():
    """Prepares the files for the exam."""
    print("\nPrepping...\n")
    
    # Locate important files
    exam_folder_located = os.path.isdir(ExamFolder)
    infoclass_file_located = os.path.isfile(InfoClassFile)
    
    # Report the user
    print(build_status(exam_folder_located, "Locating the exam directory"))
    print(build_status(infoclass_file_located, "Locating Info.cs"))
    
    if infoclass_file_located:
        user_data = ask_user_data()
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
    
    return


AppInfo = {
    "name": "exam",
    "version": "v0.1 20171022"
}


def build_header():
    return AppInfo["name"] + " " + AppInfo["version"]


def build_help():
    return  "\nHelp:\nexam [cmd]\n\nCommands:\n\
             help: Shows this help and exits.\n\
             prep: Prepares the files for the exam.\n"


def parse_options(args):
    toret = lambda: print(build_help())

    if len(args) > 1:
        if args[1] == "prep":
            toret = lambda: prep()

    return toret


if __name__ == "__main__":
    set_input_function()
    print(build_header())
    
    if sys.version_info[0] >= 3:
        print(build_header())
        cmd = parse_options(sys.argv)
        cmd()
    else:
        print("\nCRITICAL: Needs Python 3.\n")
