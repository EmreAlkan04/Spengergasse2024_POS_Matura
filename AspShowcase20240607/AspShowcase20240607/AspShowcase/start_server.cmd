cd src\AspShowcase.Client
cmd /C npm install
cmd /C npm run build
cd ..\AspShowcase.Webapi
dotnet restore --no-cache
:start
dotnet watch run
goto start
