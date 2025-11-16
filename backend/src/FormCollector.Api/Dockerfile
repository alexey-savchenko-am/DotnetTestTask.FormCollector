# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src

COPY Directory.Build.props ./ 
COPY Directory.Packages.props ./

COPY src/FormCollector.Api/FormCollector.Api.csproj ./FormCollector.Api/
COPY src/FormCollector.Application/FormCollector.Application.csproj ./FormCollector.Application/
COPY src/FormCollector.Domain/FormCollector.Domain.csproj ./FormCollector.Domain/
COPY src/SharedKernel.Domain/SharedKernel.Domain.csproj ./SharedKernel.Domain/
COPY src/FormCollector.Infrastructure/FormCollector.Infrastructure.csproj ./FormCollector.Infrastructure/

# restore
WORKDIR /src/FormCollector.Api
RUN dotnet restore "./FormCollector.Api.csproj"

COPY src/FormCollector.Api ./FormCollector.Api
COPY src/FormCollector.Application ./FormCollector.Application
COPY src/FormCollector.Domain ./FormCollector.Domain
COPY src/SharedKernel.Domain ./SharedKernel.Domain
COPY src/FormCollector.Infrastructure ./FormCollector.Infrastructure

# build
RUN dotnet build "./FormCollector.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build /p:UseAppHost=false

# publish
FROM build AS publish
RUN dotnet publish "./FormCollector.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish/ .
COPY ./src/FormCollector.Api/appsettings*.json ./
ENTRYPOINT ["dotnet", "FormCollector.Api.dll"]
