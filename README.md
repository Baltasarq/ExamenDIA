# Examen DIA
Plantillas para examenes de DIA

Clona este repositorio y úsalo como plantilla para contestar a las preguntas del examen.

## Preparación

### Preparación automática (recomendada)
Ejecuta la herramienta *exam.py* con el parámetro *prep*, y contesta a las preguntas. A continuación, ya puedes abrir un editor de textos, MonoDevelop, o Xamarin/Visual Studio.

    $ python3 exam.py prep

### Ayudas en la Preparación automática
En caso de que surja cualquier problema, será posible obtener la información del usuario tal cual está guardada:

    $ python3 exam.py info

Si es necesario, es posible recomenzar con la información por defecto:

    $ python3 examp.py ini

### Preparación manual
Recuerda, en primer lugar, modificar el nombre del directorio, siguiendo el formato siguiente (datos en minúsculas y usando solo caracteres básicos y dígitos):

    <apellido1>_<apellido2>_<nombre>-<dni>

Por ejemplo:

    palotes_gomez_perico-11222333R

Además, debes escribir tus datos personales en el archivo *usr_exam_data.txt*, con el formato:
<pre>
11222333R
Perico Gómez
Palotes
perico@palotes.com
</pre>

La plantilla tiene formato de proyecto adecuado para Xamarin Studio/Visual Studio/MonoDevelop.

## Entrega

### Entrega automatizada (recomendada)
Ejecuta la herramienta *exam.py* con el parámetro *zip*, de forma que se generará un archivo zip ya preparado para la entrega.

    $ python3 exam.py zip

### Entrega manual
En primer lugar, recuerda renombrar el directorio con los ejercicios del examen (**no** el directorio padre, **ExamenDIA**) como se muestra más arriba. Por ejemplo:

    palotes_gomez_perico-11222333R

Empaqueta el directorio donde se encuentran las preguntas del examen en un archivo zip, y entrégalo. Asegúrate de incorporar todos los archivos.

Si quieres hacerlo desde línea de comando (¡TAB autocompleta los nombres de archivos!):

    $ zip palotes_gomez_perico-11222333R.zip palotes_gomez_perico-11222333R/*
