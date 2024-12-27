FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files first
COPY ./src/event-sourcing.Api/event-sourcing.Api.csproj .

# Restore dependencies
RUN dotnet restore "event-sourcing.Api.csproj"

# Copy everything else and build
COPY ./src/event-sourcing.Api/ .
RUN dotnet publish "event-sourcing.Api.csproj" -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "event-sourcing.Api.dll"]
