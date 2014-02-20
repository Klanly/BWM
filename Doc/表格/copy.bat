@echo off
set root=..\..\..
copy /Y client\*.tbl2 %root%\Client2.5D\runclient\data\tables1
copy /Y client\*.tbl2 %root%\Client2.5D\runclient\data\tables2
copy /Y client\*.tbl.h %root%\Client2.5D\BWTable
copy /Y client\*.tbl.cpp %root%\Client2.5D\BWTable

copy /Y server\*.tbl %root%\Duckweed\map
copy /Y server\*.h %root%\Duckweed\table
pause
