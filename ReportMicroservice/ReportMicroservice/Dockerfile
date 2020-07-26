#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["./ReportMicroservice/ReportMicroservice.csproj", ""]
COPY ["./ReportBuisnessLayer/ReportBuisnessLayer.csproj", "../ReportBuisnessLayer/"]
COPY ["./ReportDataLayer/ReportDataLayer.csproj", "../ReportDataLayer/"]
RUN dotnet restore "./ReportMicroservice.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "./ReportMicroservice/ReportMicroservice.csproj" -c Development -o /app/build

FROM build AS publish
RUN dotnet publish "./ReportMicroservice/ReportMicroservice.csproj" -c Development -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReportMicroservice.dll"]