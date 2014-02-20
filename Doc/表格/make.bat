@echo off
rem 全部打表：直接双击
rem 选中文件打表：鼠标拖入相应的xml表格配置文件
rem 杨明哲 mail.ymz@163.com 2012年5月4日16:44:05

pushd %~dp0
rem 删除旧文件
if exist client del /Q client\*
if exist server del /Q server\*

rem 打表
..\Tool\PackTool\TableBuild.exe %*

popd
pause
