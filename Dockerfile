#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 9150

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Src/Galvanize.Gateway/Galvanize.Gateway.csproj", "Src/Galvanize.Gateway/"]
RUN dotnet restore "Src/Galvanize.Gateway/Galvanize.Gateway.csproj"
COPY . .
WORKDIR "/src/Src/Galvanize.Gateway"
RUN dotnet build "Galvanize.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Galvanize.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Galvanize.Gateway.dll"]