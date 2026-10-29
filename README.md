# Examen DIA

Esta plantilla permite, teniendo instalado **Visual Studio Code** y **Docker**,
tener en unos minutos todo listo para programar en C#, sin tener que
instalar nada manualmente, ni tener permisos de administrador.

## Instalación

1. Abre Visual Studio Code, e instala la extensión "Dev Containers" (Microsoft),
si no está instalada.
En la parte de control de versiones, a la izquierda,
pulsa en `Clonar repositorio` (_Clone repository_).
Introduce `http://github.code/baltasarq/ExamenDIA`.

2. Tras clonar el repositorio, el editor preguntará si debe abrirlo.
Pulsamos en `Abrir` (_Open_).

3. Tras abrirlo, el editor preguntará si debe reabrirlo en el contenedor.
Seleccionar `reabrir en contenedor`, (_reopen in container_).

4. El editor indica que está instalando componentes.
Permite ver el estado de la instalación con `Show log`.
Es posible que, una vez que en el log aparezca `container arrancado`
(_container started_), el editor se pare, sin hacer nada.
Púlsese `Ctrl+Shit+P`, y selecciónese `Desarrollador: recargar ventana`
(_Developer: reload window_). El editor recargará,
y la instalación continuará.

5. Una vez termine, el proyecto estará cargado. En el terminal,
se puede comprobar que todo funciona con `dotnet --version`.
También, es posible pulsar con el botón derecho en el proyecto `CodigoExamen`,
y construirlo.
Además, pulsando F5 se iniciará la depuración, solo es necesario
seleccionar `C#`, y después, `CodigoExamen`.

## Plantilla para examenes de DIA

Debes escribir tu solución al examen en `Solucion/Ppal.cs`.
Dentro del método `Prueba()`, elimina el código existente y
comienza con el tuyo.

El proyecto pregunta la primera vez los datos personales,
como DNI, nombre, apellidos, etc.
Así, cada vez que se ejecute el código se creará un archivo Zip
con los archivos `*.cs` en la carpeta del usuario, que en el contenedor
es `/home/vscode`.
¡Recuerda compilar y ejecutar el proyecto al menos la última vez!

Así, si el alumno ha introducido sus datos como nombre: `Súper`,
apellidos: `López`, DNI: `12345678D`, se creará el archivo Zip
`sper_lpez-12345678D` en `/home/vscode`.

**NOTA**: este zip no estará actualizado bajo las siguientes circunstancias
(¡no hay milagros!):

1. En caso de haber errores de compilación en el código, pues solo se genera
al comienzo de la ejecución del proyecto.
2. En caso de que la ejecución no llegue a iniciarse (falle justo al comenzar).

## Entrega

1. En VS Code, seleccionar `abrir directorio` (_File_ >> _Open folder_).
Introduce `/home/vscode`. Se puede volver en cualquier momento
al proyecto del examen mediante `abrir reciente` (_File_ >> _Open recent..._).
Una vez abierto `/home/vscode`, también se puede volver a este directorio
de la misma forma, con `abrir reciente`.

2. A la izquierda, en el explorador de la carpeta, aparecerá el archivo Zip,
por ejemplo, `sper_lpez-12345678D`.
Pulsa el botón derecho sobre él, y selecciona `Descarga` (_Download_).
