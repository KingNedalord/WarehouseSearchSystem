# === ЭТАП 1: Сборка и восстановление зависимостей ===
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

# Защита от сбоев сети: увеличиваем время ожидания до 5 минут (300 секунд)
ENV NUGET_HTTP_REQUEST_TIMEOUT=300
ENV DOTNET_NUGET_SIGN_VALIDATION=false

# 1. Копируем файл решения (.sln) в корень контейнера
COPY WareHouseSearchSystem.sln .

# 2. Копируем файлы описания проектов (.csproj) строго по своим папкам.
# Это нужно, чтобы Docker кэшировал NuGet-пакеты и не перекачивал их при изменении кода.
COPY Controller/*.csproj ./Controller/
COPY DAL/*.csproj ./DAL/
COPY Main/*.csproj ./Main/
COPY Models/*.csproj ./Models/
COPY Presentation/*.csproj ./Presentation/
COPY Services/*.csproj ./Services/
COPY Tests/*.csproj ./Tests/

# 3. Восстанавливаем зависимости с расширенным таймаутом
RUN dotnet restore

# 4. Только теперь копируем весь остальной исходный код (C# файлы)
COPY . .


# === ЭТАП 2: Автоматический запуск тестов ===
# Docker заходит в папку с тестами и запускает их. Если тесты упадут, сборка здесь остановится.
FROM build AS testrunner
WORKDIR /source/Tests
RUN dotnet test --no-restore


# === ЭТАП 3: Публикация приложения ===
# Компилируем проект Main в оптимизированную релизную версию без лишнего мусора
FROM testrunner AS publish
WORKDIR /source/Main
RUN dotnet publish -c Release -o /app --no-restore


# === ЭТАП 4: Финальный минимальный образ для запуска ===
# Берем легкий рантайм вместо тяжелого SDK, чтобы весить меньше
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS final
WORKDIR /app

# Забираем готовое скомпилированное приложение из этапа publish
COPY --from=publish /app .

# Команда, которая выполнится при старте контейнера
ENTRYPOINT ["dotnet", "Main.dll"]