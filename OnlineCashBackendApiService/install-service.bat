@echo off
setlocal

:: Имя службы (должно быть уникальным)
set SERVICE_NAME=OnlineCashV3

:: Отображаемое имя службы
set DISPLAY_NAME=Service for online cash

:: Имя исполняемого файла (укажите имя вашего .exe)
set EXE_NAME=OnlineCashBackendApiService.exe

:: Полный путь к EXE относительно этого BAT-файла
set EXE_PATH=%~dp0%EXE_NAME%

:: Проверяем, существует ли EXE
if not exist "%EXE_PATH%" (
    echo Error: file not found %EXE_NAME%
    echo Cope to app folder.
    pause
    exit /b 1
)

:: Останавливаем и удаляем службу, если она уже существует
sc query %SERVICE_NAME% >nul 2>&1
if %errorlevel%==0 (
    echo Stoping exists service...
    net stop %SERVICE_NAME% >nul 2>&1
    echo Delete exists service...
    sc delete %SERVICE_NAME% >nul
    timeout /t 3 >nul
)

:: Устанавливаем новую службу
echo Устанавливаю службу "%DISPLAY_NAME%"...
sc create %SERVICE_NAME% binPath= "\"%EXE_PATH%\"" start= auto DisplayName= "%DISPLAY_NAME%"

if %errorlevel% neq 0 (
    echo Error steup service!
    pause
    exit /b 1
)

echo Служба успешно установлена.
echo Запускаю службу...
net start %SERVICE_NAME%

echo Complite.
pause