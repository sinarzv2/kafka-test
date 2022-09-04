@echo off
set version=%1
echo Build docker image webapplication2 version: %version%

echo cleanup...
dotnet clean -c Debug
dotnet clean -c Release

echo delete old publish folders
for /d /r . %%d in (Publish) do @if exist "%%d" rd /s/q "%%d"

echo create publish...
dotnet publish -c Release --self-contained false -o ./Publish

echo remove previous docker images...
docker image rm webapplication2:%version% -f

echo create docker image...
docker image build -t webapplication2:%version% .

echo Download docker image webapplication2version: %version%
docker save webapplication2 -o ./webapplication2-%version%.tar

echo Successfully done.
pause