#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["APIGateway.csproj", "./"]
RUN dotnet restore "APIGateway.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "APIGateway.csproj" -c Development -o /app/build

FROM build AS publish
RUN dotnet publish "APIGateway.csproj" -c Development -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY Certificates/aspnetapp.crt /usr/local/share/ca-certificates/aspnetapp.crt
COPY Certificates/aspnetapp.key /usr/local/share/ca-certificates/aspnetapp.key
RUN update-ca-certificates

ENTRYPOINT ["dotnet", "APIGateway.dll"]
