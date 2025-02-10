# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EventApp.WebAPI/EventApp.WebAPI.csproj", "EventApp.WebAPI/"]
COPY ["EventApp.Application/EventApp.Application.csproj", "EventApp.Application/"]
COPY ["EventApp.Domain/EventApp.Domain.csproj", "EventApp.Domain/"]
COPY ["EventApp.Persistence/EventApp.Persistence.csproj", "EventApp.Persistence/"]
RUN dotnet restore "./EventApp.WebAPI/EventApp.WebAPI.csproj"
COPY . .
WORKDIR "/src/EventApp.WebAPI"
RUN dotnet build "./EventApp.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EventApp.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventApp.WebAPI.dll"]