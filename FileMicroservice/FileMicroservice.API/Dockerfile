#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
ARG ENVIRONMENT
WORKDIR /src
COPY ["./FileMicroservice.API/Configurations/Docker/NuGet.Config", "./"]
COPY ["./FileMicroservice.API/FileMicroservice.API.csproj", "./"]
COPY ["./FileMicroservice.BLL/FileMicroservice.BLL.csproj", "../FileMicroservice.BLL/"]
COPY ["./FileMicroservice.DAL/FileMicroservice.DAL.csproj", "../FileMicroservice.DAL/"]
RUN dotnet restore "FileMicroservice.API.csproj" --configfile ./NuGet.Config
COPY . .
WORKDIR "/src/"
RUN dotnet build "./FileMicroservice.API/FileMicroservice.API.csproj" -c $ENVIRONMENT -o /app/build

FROM build AS publish
ARG ENVIRONMENT
RUN dotnet publish "./FileMicroservice.API/FileMicroservice.API.csproj" -c $ENVIRONMENT -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileMicroservice.API.dll"]
