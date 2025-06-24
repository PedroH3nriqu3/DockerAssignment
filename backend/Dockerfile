# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore

RUN dotnet publish -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
ENV ASPNETCORE_HTTP_PORTS=80
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "AuthenticationServer.dll"]