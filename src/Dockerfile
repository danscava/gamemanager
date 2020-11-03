FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["GameManager.Api/GameManager.Api.csproj", "GameManager.Api/"]
COPY ["GameManager.Data/GameManager.Data.csproj", "GameManager.Data/"]
COPY ["GameManager.Services/GameManager.Services.csproj", "GameManager.Services/"]

RUN dotnet restore "GameManager.Api/GameManager.Api.csproj"
COPY . .
WORKDIR "/src/GameManager.Api"
RUN dotnet build "GameManager.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameManager.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameManager.Api.dll"]