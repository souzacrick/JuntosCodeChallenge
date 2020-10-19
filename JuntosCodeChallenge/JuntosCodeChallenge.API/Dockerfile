#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["JuntosCodeChallenge.API/JuntosCodeChallenge.API.csproj", "JuntosCodeChallenge.API/"]
COPY ["JuntosCodeChallenge.Infrastructure/JuntosCodeChallenge.Infrastructure.csproj", "JuntosCodeChallenge.Infrastructure/"]
COPY ["JuntosCodeChallenge.Domain/JuntosCodeChallenge.Domain.csproj", "JuntosCodeChallenge.Domain/"]
RUN dotnet restore "JuntosCodeChallenge.API/JuntosCodeChallenge.API.csproj"
COPY . .
WORKDIR "/src/JuntosCodeChallenge.API"
RUN dotnet build "JuntosCodeChallenge.API.csproj" -c Release -o /app/build

WORKDIR /src
COPY ["JuntosCodeChallenge.Test/JuntosCodeChallenge.Test.csproj", "JuntosCodeChallenge.Test/"]
RUN dotnet restore "JuntosCodeChallenge.Test/JuntosCodeChallenge.Test.csproj"
COPY . .
WORKDIR "/src/JuntosCodeChallenge.Test"
RUN dotnet build "JuntosCodeChallenge.Test.csproj" -c Release -o /app/build
RUN dotnet test "JuntosCodeChallenge.Test.csproj" --logger "trx;LogFileName=tests.trx" 
RUN ls

WORKDIR "/src/JuntosCodeChallenge.API"
FROM build AS publish
RUN dotnet publish "JuntosCodeChallenge.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JuntosCodeChallenge.API.dll"]
