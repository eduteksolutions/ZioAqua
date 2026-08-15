FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["zioAqua.csproj", "./"]
RUN dotnet restore "./zioAqua.csproj"

COPY . .
RUN dotnet publish "zioAqua.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "zioAqua.dll"]