# Examen DIA

Esta plantilla permite, teniendo instalado **Visual Studio Code** y **Docker**, tener en unos minutos todo listo para programar en C#, sin tener que instalar nada manualmente, ni tener permisos de administrador.

## Instalación

0. Abre Docker Desktop, y minimízalo. Abre Visual Studio Code, e instala la extensión "Dev Containers" (Microsoft), si no está instalada.

1. En la parte de control de versiones, a la izquierda, pulsa en Clonar repositorio. Introduce `http://github.code/baltasarq/ExamenDIA`.

2. Tras clonar el repositorio, el editor preguntará si debe abrirlo. Pulsamos en `Abrir` u `Open`.

3. Tras abrirlo, el editor preguntará si debe reabrirlo en el contenedor. Seleccionar `reabrir en contenedor`, o `reopen in container`.

4. El editor indica que está instalando componentes. Permite ver el estado de la instalación con `Show log`. Es posible que, una vez que en el log aparezca `container started`, el editor se pare, sin hacer nada. Púlsese `Ctrl+Shit+P`, y selecciónese `reload window` o `recargar ventana`. El editor recargará, y la instalación continuará.

5. Una vez termine, el proyecto estará cargado. En el terminal, se puede comprobar que todo funciona con `dotnet --version`. También, es posible pulsar con el botón derecho en el proyecto `CódigoExamen`, y construirlo. Además, pulsando F5 se iniciará la depuración, solo es necesario seleccionar `C#`, y después, `CodigoExmaen`.

## Plantilla para examenes de DIA

Clona este repositorio y úsalo como plantilla para contestar a las preguntas del examen.

Funciona directamente en Visual Studio Code. ¡Recuerda compilar y ejecutar el proyecto al menos la última vez!

Al comenzar cada ejecución, se crea un nuevo archivo zip *apellidos_nombre-dni.zip*, que contiene ya todo el código.

**NOTA**: este zip no estará actualizado bajo las siguientes circunstancias (¡no hay milagros!):

1. En caso de haber errores de compilación en el código, pues solo se genera cuando se ejecuta el proyecto.
2. En caso de que la ejecución no llegue a iniciarse (falle justo al comenzar).

## Entrega

Para obtener el archivo _zip_ desde el contenedor, se puede utilizar **Docker Desktop**, y acceder a _Files_ en el menú del contenedor. Pulsando el botón derecho sobre el Zip, se puede descargar con `Save`.

En otro caso, se puede obtener el nombre del contenedor que se está utilizando con `ps`, y utilizar `cp` para copiar el archivo. Este comando no soporta _wildcards_.

    $ docker ps
    Imagen    Tamaño    Id     Nombre
    Ubuntu    4Gb       A0...  silversurfer

El nombre _silversurfer_ es inventado para este ejemplo, por eso es necesario comprobar cuál es el nombre concreto del contenedor. Más abajo, _path/to/dir_, puede ser, por ejemplo, en Linux: `~/Desktop`, mientras en Windows podría ser (cambia _user_ por tu nombre de usuario): `c:\users\usr\Escritorio`.

    $ docker silversurfer exec ls /home/vscode
    lpez_sper-12345678D.zip

    $ docker cp silversurfer:/home/vscode/lpez_sper-12345678D.zip path/to/dir
    Ok
