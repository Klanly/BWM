@echo off
pushd "%~dp0"
del /Q *.meta;*.pbt;*.proto;*.cs;*.json;*.go
popd