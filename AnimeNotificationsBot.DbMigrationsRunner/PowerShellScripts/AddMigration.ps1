﻿param([string]$MigrationName)
dotnet ef migrations add $MigrationName --project ..\..\AnimeNotificationsBot.DAL\AnimeNotificationsBot.DAL.csproj --startup-project ..\AnimeNotificationsBot.DbMigrationsRunner.csproj