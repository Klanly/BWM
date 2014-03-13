@echo off
pushd %~dp0
set dest=..\..

rem 客户端bytes文件
del /Q %dest%\Assets\Resources\Table\*.bytes
copy /Y *.pbt %dest%\Assets\Resources\Table
rename %dest%\Assets\Resources\Table\*.pbt *.bytes
call:clearMeta %dest%\Assets\Resources\Table

rem 客户端C#文件
del /Q %dest%\Assets\Scripts\Table\*.cs
copy /Y *.cs %dest%\Assets\Scripts\Table
del /Q %dest%\Assets\Scripts\Table\*.proto
copy /Y *.proto %dest%\Assets\Scripts\Table
call:clearMeta %dest%\Assets\Scripts\Table

rem 服务器json文件
del /Q %dest%\Common\data\table\*.json
copy /Y *.json %dest%\Common\data\table

popd
pause
GOTO:EOF

rem ===================================================================
rem 递归删除给定目录中所有孤立的*.meta文件
rem 参数：路径
:clearMeta
for /f "tokens=* delims=" %%i in ('dir /b /s %~1\*.meta') do (
	if not exist %%~dpni del /Q %%i
)
GOTO:EOF

