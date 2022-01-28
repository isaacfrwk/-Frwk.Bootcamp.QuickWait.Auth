#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/1 - Application/FrwkQuickWaitAuth/FrwkQuickWaitAuth.csproj", "src/1 - Application/FrwkQuickWaitAuth/"]
COPY ["src/4 - Infrastructure/FrwkQuickWait.Data/FrwkQuickWait.Data.csproj", "src/4 - Infrastructure/FrwkQuickWait.Data/"]
COPY ["src/3 - Service/FrwkQuickWait.Service/FrwkQuickWait.Service.csproj", "src/3 - Service/FrwkQuickWait.Service/"]
COPY ["src/2 - Domain/FrwkQuickWait.Domain/FrwkQuickWait.Domain.csproj", "src/2 - Domain/FrwkQuickWait.Domain/"]
RUN dotnet restore "src/1 - Application/FrwkQuickWaitAuth/FrwkQuickWaitAuth.csproj"
COPY . .
WORKDIR "/src/src/1 - Application/FrwkQuickWaitAuth"
RUN dotnet build "FrwkQuickWaitAuth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FrwkQuickWaitAuth.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FrwkQuickWaitAuth.dll"]