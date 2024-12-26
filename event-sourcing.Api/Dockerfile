FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files first
COPY ./event-sourcing.Api/event-sourcing.csproj .

# Restore dependencies
RUN dotnet restore "event-sourcing.csproj"

# Copy everything else and build
COPY ./event-sourcing.Api/ .
RUN dotnet publish "event-sourcing.csproj" -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "event-sourcing.dll"]
