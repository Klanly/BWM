@echo off
set protogen=%~dp0\3Party\protobuf-net r668\ProtoGen\protogen.exe
set dest=%~dp0\Assets\Scripts\Common

pushd "%~dp0\Common"
echo ===================================================================
del /Q *.cs
del /Q "%dest%\*.cs"
call:buildDir "%~dp0\Common" "%dest%"
call:buildDir "%~dp0\Common\Config" "%dest%\Editor"
del /Q *.cs
echo ===================================================================
call:clearMeta "%dest%"
popd

pause
GOTO:EOF

rem ===================================================================
rem 对指定目录的proto文件进行C#代码生成
rem 参数1: proto路径
rem 参数2: cs文件目标路径
:buildDir
echo build: %~1 -> %~2
for /f "tokens=* delims=" %%i in ('dir /b "%~1\*.proto"') do (
	echo %~1\%%i
	"%protogen%" -i:"%~1\%%i" -o:"%~1\%%i.cs" -q
)
xcopy "%~1\*.cs" "%~2" /Y /F

rem ===================================================================
rem 递归删除给定目录中所有孤立的*.meta文件
rem 参数: 路径
:clearMeta
echo clear meta: %~1
for /f "tokens=* delims=" %%i in ('dir /b /s "%~1\*.meta"') do (
	if not exist "%%~dpni" (
		echo "%~1\%%i"
		del /Q "%~1\%%i"
	)
)
GOTO:EOF