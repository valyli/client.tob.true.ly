set app_path=%~dp0
cd %~dp0
title %app_path%injector.py

rem conda activate
python injector.py %1 %2
pause