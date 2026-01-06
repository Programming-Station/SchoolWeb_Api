# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["School_API/School_API.csproj", "School_API/"]
COPY ["School.Models/School.Models.csproj", "School.Models/"]
COPY ["School.Services/School.Services.csproj", "School.Services/"]
COPY ["School.Infrastructure/School.Infrastructure.csproj", "School.Infrastructure/"]
COPY ["School.Domain/School.Domain.csproj", "School.Domain/"]
COPY ["School.Utilities/School.Utilities.csproj", "School.Utilities/"]
COPY ["School_DTOs/School_DTOs.csproj", "School_DTOs/"]
RUN dotnet restore "./School_API/School_API.csproj"
COPY . .
WORKDIR "/src/School_API"
RUN dotnet build "./School_API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Setting time zone to Asia/Kolkata
# RUN rm -rf /etc/localtime
# RUN ln -s /usr/share/zoneinfo/Asia/Kolkata /etc/localtime


# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./School_API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
# Copy the published output from the previous stage
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "School_API.dll"]