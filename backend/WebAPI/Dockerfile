FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# USER app
WORKDIR /app

# Ensure the DataProtection-Keys directory exists
RUN mkdir -p /app/DataProtection-Keys && chmod -R 777 /app/DataProtection-Keys

# Expose HTTP and HTTPS ports
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WebAPI/WebAPI.csproj", "WebAPI/"]
COPY ["BLL/BLL.csproj", "BLL/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["Models/Models.csproj", "Models/"]
COPY ["Utility/Utility.csproj", "Utility/"]
COPY ["DAL/DAL.csproj", "DAL/"]
RUN dotnet restore "./WebAPI/WebAPI.csproj"
COPY . .
WORKDIR "/src/WebAPI"
RUN dotnet build "./WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set a non-root user after setting permissions
RUN adduser --disabled-password --gecos '' myuser \
    && chown -R myuser:myuser /app

USER myuser

ENTRYPOINT ["dotnet", "WebAPI.dll"]