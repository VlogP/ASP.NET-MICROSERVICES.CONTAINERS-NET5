#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
ARG ENVIRONMENT
WORKDIR /src
COPY ["./AuthMicroservice.API/Configurations/Docker/NuGet.Config", "./"]
COPY ["./AuthMicroservice.API/AuthMicroservice.API.csproj", "./"]
COPY ["./AuthMicroservice.BLL/AuthMicroservice.BLL.csproj", "../AuthMicroservice.BLL/"]
COPY ["./AuthMicroservice.DAL/AuthMicroservice.DAL.csproj", "../AuthMicroservice.DAL/"]
RUN dotnet restore "AuthMicroservice.API.csproj" --configfile ./NuGet.Config
COPY . .
WORKDIR "/src/"
RUN dotnet build "./AuthMicroservice.API/AuthMicroservice.API.csproj" -c $ENVIRONMENT -o /app/build

FROM build AS publish
ARG ENVIRONMENT
RUN dotnet publish "./AuthMicroservice.API/AuthMicroservice.API.csproj" -c $ENVIRONMENT -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ADD ./AuthMicroservice.API/Certificates/authcredential.pfx /app/Certificates/authcredential.pfx
ADD ./AuthMicroservice.API/Certificates/authmicroservice.crt /usr/local/share/ca-certificates/authmicroservice.crt
RUN update-ca-certificates

ENTRYPOINT ["dotnet", "AuthMicroservice.API.dll"]