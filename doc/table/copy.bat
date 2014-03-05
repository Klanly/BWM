@echo off
pushd %~dp0
set dest=..\..

del /Q %dest%\Assets\Resources\Table\*.bytes
copy /Y *.pbt %dest%\Assets\Resources\Table
rename %dest%\Assets\Resources\Table\*.pbt *.bytes
call:clearMeta %dest%\Assets\Resources\Table

del /Q %dest%\Assets\Scripts\Table\*.cs
copy /Y *.cs %dest%\Assets\Scripts\Table
del /Q %dest%\Assets\Scripts\Table\*.proto
copy /Y *.proto %dest%\Assets\Scripts\Table
call:clearMeta %dest%\Assets\Scripts\Table

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

