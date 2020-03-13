FROM mcr.microsoft.com/dotnet/core/sdk:3.1
ARG BUILD_CONFIGURATION=Debug
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

WORKDIR /src
ENV NUGET_PACKAGES=/src/packages
COPY ["WebApi/WebApi.csproj", "WebApi/"]
COPY ["Persistence/Persistence.csproj", "Persistence/"]

RUN dotnet restore "WebApi/WebApi.csproj"
COPY . .
WORKDIR "/src/WebApi"
RUN dotnet build --no-restore "WebApi.csproj" -c $BUILD_CONFIGURATION

RUN echo "exec dotnet run --no-build /seed --no-launch-profile -c $BUILD_CONFIGURATION -- & exec dotnet run --no-build --no-launch-profile -c $BUILD_CONFIGURATION --" > /entrypoint.sh

ENTRYPOINT ["/bin/bash", "/entrypoint.sh"]