#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
ARG ENVIRONMENT
WORKDIR /src
COPY ["./AuthMicroservice/AuthMicroservice.csproj", "./"]
COPY ["./AuthBuisnessLayer/AuthBuisnessLayer.csproj", "../AuthBuisnessLayer/"]
COPY ["./AuthDataLayer/AuthDataLayer.csproj", "../AuthDataLayer/"]
RUN dotnet restore "AuthMicroservice.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "./AuthMicroservice/AuthMicroservice.csproj" -c $ENVIRONMENT -o /app/build

FROM build AS publish
ARG ENVIRONMENT
RUN dotnet publish "./AuthMicroservice/AuthMicroservice.csproj" -c $ENVIRONMENT -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthMicroservice.dll"]