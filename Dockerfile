# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy everything and restore
COPY . ./
RUN dotnet restore "RandomnessnessAPI.csproj"

# Build and publish
RUN dotnet publish "RandomnessnessAPI.csproj" -c Release -o out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

# Railway uses the PORT env var
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "RandomnessnessAPI.dll"]
