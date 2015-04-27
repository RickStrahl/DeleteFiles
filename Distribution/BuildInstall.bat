REM cd ..\..\..\Distribution
cd
copy ..\DeleteFiles\bin\Release\DeleteFiles.exe DeleteFiles.exe
call 7z a -tzip "DeleteFiles.zip" "DeleteFiles.exe"