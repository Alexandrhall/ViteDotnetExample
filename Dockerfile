# ---- Build frontend ----
FROM node:24 AS frontend-build

WORKDIR /app/clientapp
COPY clientapp/package.json clientapp/package-lock.json ./
RUN npm install
COPY clientapp ./
RUN npm run build

# ---- Build backend ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS backend-build

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
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app
COPY --from=backend-build /app/out ./

# Add a build argument for the port
ARG PORT=5057

# Set environment variable for ASP.NET Core URLs
ENV ASPNETCORE_URLS=http://+:${PORT}

# Expose the port
EXPOSE ${PORT}

# Start the application
ENTRYPOINT ["dotnet", "weather.dll"]
