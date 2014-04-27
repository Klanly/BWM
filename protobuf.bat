@echo off
set protogen=%~dp0\3Party\protobuf-net r668\ProtoGen\protogen.exe
set dest=%~dp0\Assets\Scripts\Common

pushd "%~dp0\Common"
echo ===================================================================
del /Q /S *.cs
del /Q /S "%dest%\*.cs"
call:buildDir "%~dp0\Common" "%dest%"
del /Q /S *.cs
echo ===================================================================
call:clearMeta "%dest%"
popd

"%protogen%" -i:"Assets\Scripts\Config\UserData.proto" -o:"Assets\Scripts\Config\UserData.proto.cs" -p:observable -q

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
GOTO:EOF

rem ===================================================================
rem 递归删除给定目录中所有孤立的*.meta文件
rem 参数: 路径
:clearMeta
echo clear meta: %~1
for /f "tokens=* delims=" %%i in ('dir /b /s "%~1\*.meta"') do (
	if not exist "%%~dpni" (
		echo %%i
		del /Q "%%i"
	)
)
GOTO:EOF