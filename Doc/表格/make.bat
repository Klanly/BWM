@echo off
rem ȫ�����ֱ��˫��
rem ѡ���ļ�������������Ӧ��xml��������ļ�
rem ������ mail.ymz@163.com 2012��5��4��16:44:05

pushd %~dp0
rem ɾ�����ļ�
if exist client del /Q client\*
if exist server del /Q server\*

rem ���
..\Tool\PackTool\TableBuild.exe %*

popd
pause
