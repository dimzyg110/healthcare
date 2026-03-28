FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY HealthcareSystem.csproj .
RUN dotnet restore HealthcareSystem.csproj

# Copy everything else and build
COPY . .
RUN dotnet publish HealthcareSystem.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published output
COPY --from=build /app/publish .

# Copy the SQLite database
COPY healthcare.db .

# Set environment variables
ARG PORT=8080
ENV PORT=${PORT}
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose port
EXPOSE 8080

# Use shell form so $PORT is expanded at runtime
CMD dotnet HealthcareSystem.dll --urls "http://+:${PORT}"
