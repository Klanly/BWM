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
rem ��ָ��Ŀ¼��proto�ļ�����C#��������
rem ����: ·��
:buildDir
echo build: %~1
for /f "tokens=* delims=" %%i in ('dir /b "%~1\*.proto"') do (
	echo %%i
	"%protogen%" -i:"%%i" -o:"%%i.cs" -q
)

rem ===================================================================
rem �ݹ�ɾ������Ŀ¼�����й�����*.meta�ļ�
rem ����: ·��
:clearMeta
echo clear meta: %~1
for /f "tokens=* delims=" %%i in ('dir /b /s "%~1\*.meta"') do (
	if not exist "%%~dpni" del /Q "%%i"
)
GOTO:EOF