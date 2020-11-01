#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
ARG ENVIRONMENT
WORKDIR /src
COPY ["./TempateMicroservice.API/Configurations/Docker/NuGet.Config", "./"]
COPY ["./TempateMicroservice.API/TempateMicroservice.API.csproj", "./"]
COPY ["./TempateMicroservice.BLL/TempateMicroservice.BLL.csproj", "../TempateMicroservice.BLL/"]
COPY ["./TempateMicroservice.DAL/TempateMicroservice.DAL.csproj", "../TempateMicroservice.DAL/"]
RUN dotnet restore "ProductMicroservice.API.csproj" --configfile ./NuGet.Config
COPY . .
WORKDIR "/src/"
RUN dotnet build "./TempateMicroservice.API/TempateMicroservice.API.csproj" -c $ENVIRONMENT -o /app/build

FROM build AS publish
ARG ENVIRONMENT
RUN dotnet publish "./TempateMicroservice.API/TempateMicroservice.API.csproj" -c $ENVIRONMENT -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TempateMicroservice.API.dll"]
