#!/bin/bash
echo "Building..."
dotnet build ./src/Agora.API/Agora.API.csproj "--property:useTestcontainer=true"
if [ $? -ne 0 ]; then
    echo "Build failed."
    exit $?
fi
echo "Build completed successfully."
echo "Running..."
dotnet run --project ./src/Agora.API/Agora.API.csproj --no-build
