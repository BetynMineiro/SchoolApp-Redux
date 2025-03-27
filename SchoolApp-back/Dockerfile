FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

COPY *.sln .
COPY SchoolApp.Api/SchoolApp.Api.csproj ./SchoolApp.Api/
COPY SchoolApp.Application/SchoolApp.Application.csproj ./SchoolApp.Application/
COPY SchoolApp.CrossCutting/SchoolApp.CrossCutting.csproj ./SchoolApp.CrossCutting/SchoolApp/
COPY SchoolApp.Domain/SchoolApp.Domain.csproj ./SchoolApp.Domain/
COPY SchoolApp.Auth0.Adapter/SchoolApp.Auth0.Adapter.csproj ./SchoolApp.Auth0.Adapter/
COPY SchoolApp.MongoDb.Adapter/SchoolApp.MongoDb.Adapter.csproj ./SchoolApp.MongoDb.Adapter/

RUN dotnet restore SchoolApp.Api/SchoolApp.Api.csproj

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App

COPY --from=build-env /App/out .

ENTRYPOINT ["dotnet", "SchoolApp.Api.dll"]
