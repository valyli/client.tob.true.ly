set app_path=%~dp0
cd %~dp0
title %app_path%injector.py

rem conda activate
python injector.py %~dp0Cap1 %1
pause