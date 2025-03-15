# ---- Build frontend ----
FROM node:22 AS frontend-build

WORKDIR /app/clientapp
COPY clientapp/package.json clientapp/package-lock.json ./
RUN npm install
COPY clientapp ./
RUN npm run build

# ---- Build backend ----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS backend-build

WORKDIR /app

# Copy .NET project files and restore dependencies
COPY weather.csproj ./
RUN dotnet restore

# Copy the entire backend project
COPY . ./
RUN dotnet build -c Release --no-restore
RUN dotnet publish -c Release -o out

# Copy frontend build to backend
COPY --from=frontend-build /app/clientapp/dist/ /app/out/wwwroot/

# ---- Run application ----
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app
COPY --from=backend-build /app/out ./

# Expose port 5057
EXPOSE 5057

# Start the application
ENTRYPOINT ["dotnet", "weather.dll"]
