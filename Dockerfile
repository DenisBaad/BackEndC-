From mcr.microsoft.com/dotnet/sdk:8.0 As build-env
WORKDIR /app

COPY Src/ .

WORKDIR Backend/Aquiles.API

RUN dotnet restore
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "Aquiles.API.dll"]