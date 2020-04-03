#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:2.1-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["TestAutomationFramework/TestAutomationFramework.csproj", "TestAutomationFramework/"]
RUN dotnet restore "TestAutomationFramework/TestAutomationFramework.csproj"
COPY . .
WORKDIR "/src/TestAutomationFramework"
RUN dotnet build "TestAutomationFramework.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestAutomationFramework.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestAutomationFramework.dll"]
