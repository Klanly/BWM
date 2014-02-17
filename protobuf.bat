@echo off
pushd "%~dp0"

set protogen="3Party\protobuf-net r668\ProtoGen\protogen.exe"
for /f "tokens=* delims=" %%i in ('dir /b /s Common\*.proto') do (
	echo %%i
	%protogen% -i:"%%i" -o:"%%i.cs" -q
	move /Y "%%i.cs" Assets\Scripts\Common\
)

pause