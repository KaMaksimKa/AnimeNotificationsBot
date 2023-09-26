#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5050
EXPOSE 5051

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AnimeNotificationsBot.Quartz/AnimeNotificationsBot.Quartz.csproj", "AnimeNotificationsBot.Quartz/"]
COPY ["AnimeNotificationsBot.BLL/AnimeNotificationsBot.BLL.csproj", "AnimeNotificationsBot.BLL/"]
COPY ["AnimeNotificationsBot.Common/AnimeNotificationsBot.Common.csproj", "AnimeNotificationsBot.Common/"]
COPY ["AnimeNotificationsBot.DAL/AnimeNotificationsBot.DAL.csproj", "AnimeNotificationsBot.DAL/"]
RUN dotnet restore "AnimeNotificationsBot.Quartz/AnimeNotificationsBot.Quartz.csproj"
COPY . .
WORKDIR "/src/AnimeNotificationsBot.Quartz"
RUN dotnet build "AnimeNotificationsBot.Quartz.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AnimeNotificationsBot.Quartz.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AnimeNotificationsBot.Quartz.dll"]