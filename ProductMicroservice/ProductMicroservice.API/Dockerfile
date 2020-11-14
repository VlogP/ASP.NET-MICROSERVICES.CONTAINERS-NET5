#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG ENVIRONMENT
WORKDIR /src
COPY ["./ProductMicroservice.API/Configurations/Docker/NuGet.Config", "./"]
COPY ["./ProductMicroservice.API/ProductMicroservice.API.csproj", "./"]
COPY ["./ProductMicroservice.BLL/ProductMicroservice.BLL.csproj", "../ProductMicroservice.BLL/"]
COPY ["./ProductMicroservice.DAL/ProductMicroservice.DAL.csproj", "../ProductMicroservice.DAL/"]
RUN dotnet restore "ProductMicroservice.API.csproj" --configfile ./NuGet.Config
COPY . .
WORKDIR "/src/"
RUN dotnet build "./ProductMicroservice.API/ProductMicroservice.API.csproj" -c $ENVIRONMENT -o /app/build

FROM build AS publish
ARG ENVIRONMENT
RUN dotnet publish "./ProductMicroservice.API/ProductMicroservice.API.csproj" -c $ENVIRONMENT -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductMicroservice.API.dll"]
