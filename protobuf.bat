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
rem ��ָ��Ŀ¼��proto�ļ�����C#��������
rem ����1: proto·��
rem ����2: cs�ļ�Ŀ��·��
:buildDir
echo build: %~1 -> %~2
for /f "tokens=* delims=" %%i in ('dir /b "%~1\*.proto"') do (
	echo %~1\%%i
	"%protogen%" -i:"%~1\%%i" -o:"%~1\%%i.cs" -q
)
xcopy "%~1\*.cs" "%~2" /Y /F

rem ===================================================================
rem �ݹ�ɾ������Ŀ¼�����й�����*.meta�ļ�
rem ����: ·��
:clearMeta
echo clear meta: %~1
for /f "tokens=* delims=" %%i in ('dir /b /s "%~1\*.meta"') do (
	if not exist "%%~dpni" (
		echo "%~1\%%i"
		del /Q "%~1\%%i"
	)
)
GOTO:EOF