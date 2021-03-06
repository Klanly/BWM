@echo off
pushd %~dp0\..\..

rem 客户端bytes文件
del /Q Assets\Resources\Table\*.bytes
copy /Y doc\table\*.pbt Assets\Resources\Table
rename Assets\Resources\Table\*.pbt *.bytes
call:clearMeta Assets\Resources\Table

rem 客户端C#文件
del /Q Assets\Scripts\Table\*.cs
copy /Y doc\table\*.cs Assets\Scripts\Table
call:clearMeta Assets\Scripts\Table

rem 服务器json文件
del /Q Common\data\table\*.json
copy /Y doc\table\*.json Common\data\table

rem 服务器go文件
rename Common\table\Table.go Table.BAK
del /Q Common\table\Table*.go
rename Common\table\*.BAK *.go
copy /Y doc\table\*.go Common\table

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

