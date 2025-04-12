FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["sap-cap-mcp.csproj", "./"]
RUN dotnet restore "sap-cap-mcp.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "sap-cap-mcp.csproj" -c Release -o /app/build --self-contained true /p:PublishSingleFile=true
RUN rm /app/build/appsettings.Development.json
RUN rm /app/build/appsettings.json
COPY ./appsettings.Docker.json /app/build/appsettings.json
RUN rm /app/build/appsettings.Docker.json

# Expose ports - both the HTTP and HTTPS ports from your configuration
EXPOSE 21210

WORKDIR /app/build
RUN rm -rf /src
RUN chmod +x ./sap-cap-mcp

ENTRYPOINT [ "./sap-cap-mcp" ]