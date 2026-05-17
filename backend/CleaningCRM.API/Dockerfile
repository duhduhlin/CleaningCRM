FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CleaningCRM.API/CleaningCRM.API.csproj", "CleaningCRM.API/"]
RUN dotnet restore "CleaningCRM.API/CleaningCRM.API.csproj"
COPY . .
WORKDIR "/src/CleaningCRM.API"
RUN dotnet build "CleaningCRM.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CleaningCRM.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 5000
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleaningCRM.API.dll"]
