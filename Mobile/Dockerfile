FROM mcr.microsoft.com/dotnet/sdk:6.0
ARG BUILD_CONFIGURATION=Debug
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

WORKDIR /src
ENV NUGET_PACKAGES=/src/packages
COPY ["Mobile/Mobile.csproj", "Mobile/"]

RUN dotnet restore "Mobile/Mobile.csproj"
COPY Mobile/. Mobile/
WORKDIR "/src/Mobile"
RUN dotnet build --no-restore "Mobile.csproj" -c $BUILD_CONFIGURATION

RUN echo "dotnet run --no-build --no-launch-profile -c $BUILD_CONFIGURATION" > /entrypoint.sh

ENTRYPOINT ["/bin/bash", "/entrypoint.sh"]