@echo off
set wait=1
echo Updating...
echo wscript.sleep %wait%000 > wait.vbs
wscript.exe wait.vbs
del wait.vbs
copy Update.exe Marwari.exe
start Marwari.exe
exit

