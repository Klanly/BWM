@echo off
pushd %~dp0
set dest=..\..

rem �ͻ���bytes�ļ�
del /Q %dest%\Assets\Resources\Table\*.bytes
copy /Y *.pbt %dest%\Assets\Resources\Table
rename %dest%\Assets\Resources\Table\*.pbt *.bytes
call:clearMeta %dest%\Assets\Resources\Table

rem �ͻ���C#�ļ�
del /Q %dest%\Assets\Scripts\Table\*.cs
copy /Y *.cs %dest%\Assets\Scripts\Table
del /Q %dest%\Assets\Scripts\Table\*.proto
copy /Y *.proto %dest%\Assets\Scripts\Table
call:clearMeta %dest%\Assets\Scripts\Table

rem ������json�ļ�
del /Q %dest%\Common\data\table\*.json
copy /Y *.json %dest%\Common\data\table

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

