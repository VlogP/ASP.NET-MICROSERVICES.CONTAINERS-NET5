#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
ARG ENVIRONMENT
WORKDIR /src
COPY ["./ReportMicroservice.API/Configurations/Docker/NuGet.Config", "./"]
COPY ["./ReportMicroservice.API/ReportMicroservice.API.csproj", "./"]
COPY ["./ReportMicroservice.BLL/ReportMicroservice.BLL.csproj", "../ReportMicroservice.BLL/"]
COPY ["./ReportMicroservice.DAL/ReportMicroservice.DAL.csproj", "../ReportMicroservice.DAL/"]
RUN dotnet restore "./ReportMicroservice.API.csproj" --configfile ./NuGet.Config
COPY . .
WORKDIR "/src/"
RUN dotnet build "./ReportMicroservice.API/ReportMicroservice.API.csproj" -c $ENVIRONMENT -o /app/build

FROM build AS publish
ARG ENVIRONMENT
RUN dotnet publish "./ReportMicroservice.API/ReportMicroservice.API.csproj" -c $ENVIRONMENT -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReportMicroservice.API.dll"]