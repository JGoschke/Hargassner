#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Hargassner2/Hargassner2.csproj", "Hargassner2/"]
RUN dotnet restore "Hargassner2/Hargassner2.csproj"
COPY . .
WORKDIR "/src/Hargassner2"
RUN dotnet build "Hargassner2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hargassner2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
VOLUME /app/log
ENTRYPOINT ["dotnet", "Hargassner2.dll"]