# ---- Build frontend ----
FROM node:24 AS frontend-build

WORKDIR /app/clientapp
COPY clientapp/package.json clientapp/package-lock.json ./
RUN npm ci
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
RUN dotnet publish -c Release -o out --no-restore

# Copy frontend build to backend
COPY --from=frontend-build /app/clientapp/dist/ /app/out/wwwroot/

# ---- Run application ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app
COPY --from=backend-build /app/out ./

# Run as the non-root user provided by the .NET image
USER $APP_UID

# Add a build argument for the port
ARG PORT=5057

# Set the HTTP port ASP.NET Core listens on
ENV ASPNETCORE_HTTP_PORTS=${PORT}

# Expose the port
EXPOSE ${PORT}

# Start the application
ENTRYPOINT ["dotnet", "weather.dll"]
