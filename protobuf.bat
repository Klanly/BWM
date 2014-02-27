@echo off
pushd "%~dp0"

set protogen="3Party\protobuf-net r668\ProtoGen\protogen.exe"
set dest=Assets\Scripts\Common

del /Q %dest%\*.cs
for /f "tokens=* delims=" %%i in ('dir /b /s Common\*.proto') do (
	echo %%i
	%protogen% -i:"%%i" -o:"%%i.cs" -q
	move /Y "%%i.cs" %dest%
)

call:clearMeta %dest%

pause
GOTO:EOF

rem ===================================================================
rem 递归删除给定目录中所有孤立的*.meta文件
rem 参数：路径
:clearMeta
echo hello %~1
for /f "tokens=* delims=" %%i in ('dir /b /s %~1\*.meta') do (
	if not exist %%~dpni del /Q %%i
)
GOTO:EOF