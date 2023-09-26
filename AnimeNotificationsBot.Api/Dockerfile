#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AnimeNotificationsBot.Api/AnimeNotificationsBot.Api.csproj", "AnimeNotificationsBot.Api/"]
COPY ["AnimeNotificationsBot.BLL/AnimeNotificationsBot.BLL.csproj", "AnimeNotificationsBot.BLL/"]
COPY ["AnimeNotificationsBot.Common/AnimeNotificationsBot.Common.csproj", "AnimeNotificationsBot.Common/"]
COPY ["AnimeNotificationsBot.DAL/AnimeNotificationsBot.DAL.csproj", "AnimeNotificationsBot.DAL/"]
RUN dotnet restore "AnimeNotificationsBot.Api/AnimeNotificationsBot.Api.csproj"
COPY . .
WORKDIR "/src/AnimeNotificationsBot.Api"
RUN dotnet build "AnimeNotificationsBot.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AnimeNotificationsBot.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AnimeNotificationsBot.Api.dll"]