REM @echo off
REM Löscht alle temporären Visual Studio Dateien
rd /S /Q ".vs"
  
FOR /D /R %%d IN (*) DO (
  rd /S /Q "%%d\obj"
)


