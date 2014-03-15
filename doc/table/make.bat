@echo off
pushd "%~dp0"
set tool=..\..\..\..\Tools\release

call clear.bat
%tool%\TableBuild .

popd
pause