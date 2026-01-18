# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and project files, restore dependencies
COPY MyFactory.sln .
COPY src/ ./src/
RUN dotnet restore "src/MyFactory.WebApi/MyFactory.WebApi.csproj"

# Copy the rest of the source and publish
WORKDIR /app/src/MyFactory.WebApi
RUN dotnet publish "MyFactory.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MyFactory.WebApi.dll"]

