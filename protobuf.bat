@echo off
set protogen=%~dp0\3Party\protobuf-net r668\ProtoGen\protogen.exe
set dest=%~dp0\Assets\Scripts\Common

pushd "%~dp0\Common"
echo ===================================================================
del /Q *.cs
call:buildDir "%~dp0\Common"
echo ===================================================================
del /Q "%dest%\*.cs"
xcopy *.cs "%dest%" /S /Y /F
del /Q *.cs
echo ===================================================================
call:clearMeta "%dest%"
popd

pause
GOTO:EOF

rem ===================================================================
rem 对指定目录的proto文件进行C#代码生成
rem 参数: 路径
:buildDir
echo build: %~1
for /f "tokens=* delims=" %%i in ('dir /b "%~1\*.proto"') do (
	echo %%i
	"%protogen%" -i:"%%i" -o:"%%i.cs" -q
)

rem ===================================================================
rem 递归删除给定目录中所有孤立的*.meta文件
rem 参数: 路径
:clearMeta
echo clear meta: %~1
for /f "tokens=* delims=" %%i in ('dir /b /s "%~1\*.meta"') do (
	if not exist "%%~dpni" del /Q "%%i"
)
GOTO:EOF