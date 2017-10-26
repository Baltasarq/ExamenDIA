# Examen DIA
Plantillas para examenes de DIA

Clona este repositorio y úsalo como plantilla para contestar a las preguntas del examen.

## Configuración automática (recomendada)
Ejecuta la herramienta *exam.py* con el parámetro *prep*, y contesta a las preguntas. A continuación, ya puedes abrir  MonoDevelop, o Xamarin/Visual Studio.

    $ python3 exam.py prep

## Condiguración manual
Recuerda, en primer lugar, modificar el nombre del directorio, siguiendo el formato dado, por los datos en minúsculas y usando solo caracteres básicos y dígitos. Por ejemplo:

    palotes_gomez_perico-11222333R
    
Además, debes escribir tus datos personales en *Codigo/Info.cs*.

La plantilla tiene formato de proyecto para Xamarin Studio/Visual Studio/MonoDevelop.

## Construcción automatizada
En el caso de que estén instaladas las herramientas de desarrollo, es posible lanzar la construcción con la herramienta *exam.py* con el parámetro *build*.

    $ python3 exam.py build
    
## Ejecución automatizada
En el caso de que esté instalado **mono**, o en Windows, que se puede ejecutar directamente el ensamblado generado, es posible lanzar la ejecución con la herramienta *exam.py* con el parámetro *run*.

    $ python3 exam.py run

## Entrega automatizada (recomendada)
Ejecuta la herramienta *exam.py* con el parámetro *zip*, de forma que se generará un archivo zip ya preparado para la entrega.

    $ python3 exam.py zip

## Entrega manual
Empaqueta el directorio donde se encuentran las preguntas del examen en un archivo zip, y entrégalo. Asegúrate de incorporar todos los archivos.
