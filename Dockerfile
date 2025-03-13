# ---- Bygg frontend ----
    FROM node:22 AS frontend-build

    WORKDIR /app/clientapp
    COPY clientapp/package.json clientapp/package-lock.json ./
    RUN npm install
    COPY clientapp ./
    RUN npm run build
    
    # ---- Bygg backend ----
    FROM mcr.microsoft.com/dotnet/sdk:9.0 AS backend-build
    
    WORKDIR /app
    
    # Kopiera .NET-projektfiler och återställ dependencies
    COPY weather.csproj ./
    RUN dotnet restore
    
    ENV ASPNETCORE_URLS=http://localhost:5057

    # Kopiera hela backend-projektet
    COPY . ./
    RUN dotnet build -c Release --no-restore
    RUN dotnet publish -c Release -o out
    
    # Kopiera frontend-build till backend
    COPY --from=frontend-build /app/clientapp/dist/ /app/out/wwwroot/
    
    # ---- Kör-applikation ----
    FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
    
    WORKDIR /app
    COPY --from=backend-build /app/out ./
    
    # Exponera port 5057
    EXPOSE 5057
    
    # Starta applikationen
    ENTRYPOINT ["dotnet", "weather.dll"]
    