#!/bin/bash
cd src/AspShowcase.Client
npm install
npm run build
cd ../AspShowcase.Webapi
dotnet restore --no-cache
dotnet watch run

