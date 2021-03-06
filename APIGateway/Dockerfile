#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
ARG ENVIRONMENT
WORKDIR /src
COPY ["Configurations/Docker/NuGet.Config", "./"]
COPY ["APIGateway.csproj", "./"]
RUN dotnet restore "APIGateway.csproj" --configfile ./NuGet.Config
COPY . .
WORKDIR "/src/"
RUN dotnet build "APIGateway.csproj" -c $ENVIRONMENT -o /app/build

FROM build AS publish
ARG ENVIRONMENT
RUN dotnet publish "APIGateway.csproj" -c $ENVIRONMENT -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ADD Certificates/aspnetapp.crt /usr/local/share/ca-certificates/aspnetapp.crt
RUN update-ca-certificates

ENTRYPOINT ["dotnet", "APIGateway.dll"]
