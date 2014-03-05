@echo off
pushd "%~dp0"
set tool=..\..\..\..\Tools\release

%tool%\TableBuild .

popd
pause