@echo off
pushd %~dp0\..\..

rem �ͻ���bytes�ļ�
del /Q Assets\Resources\Table\*.bytes
copy /Y doc\table\*.pbt Assets\Resources\Table
rename Assets\Resources\Table\*.pbt *.bytes
call:clearMeta Assets\Resources\Table

rem �ͻ���C#�ļ�
del /Q Assets\Scripts\Table\*.cs
copy /Y doc\table\*.cs Assets\Scripts\Table
call:clearMeta Assets\Scripts\Table

rem ������json�ļ�
del /Q Common\data\table\*.json
copy /Y doc\table\*.json Common\data\table

popd
pause
GOTO:EOF

rem ===================================================================
rem �ݹ�ɾ������Ŀ¼�����й�����*.meta�ļ�
rem ������·��
:clearMeta
for /f "tokens=* delims=" %%i in ('dir /b /s %~1\*.meta') do (
	if not exist %%~dpni del /Q %%i
)
GOTO:EOF

