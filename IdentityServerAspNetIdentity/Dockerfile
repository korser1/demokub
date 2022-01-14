FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine
ARG BUILD_CONFIGURATION=Debug
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

WORKDIR /src
ENV NUGET_PACKAGES=/src/packages
COPY ["IdentityServerAspNetIdentity/IdentityServerAspNetIdentity.csproj", "IdentityServerAspNetIdentity/"]

RUN dotnet restore "IdentityServerAspNetIdentity/IdentityServerAspNetIdentity.csproj"
COPY IdentityServerAspNetIdentity/. IdentityServerAspNetIdentity/
WORKDIR "/src/IdentityServerAspNetIdentity"
RUN dotnet build --no-restore "IdentityServerAspNetIdentity.csproj" -c $BUILD_CONFIGURATION

RUN echo "dotnet run --no-build /seed --no-launch-profile -c $BUILD_CONFIGURATION && dotnet run --no-build --no-launch-profile -c $BUILD_CONFIGURATION" > /entrypoint.sh

ENTRYPOINT ["/bin/sh", "/entrypoint.sh"]