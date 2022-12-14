# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /source
COPY . .
RUN dotnet restore ./AuthenticationService --disable-parallel
RUN dotnet publish ./AuthenticationService -c release -o /app --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal
WORKDIR /app
Copy --from=build /app ./

EXPOSE 5001-5002

ENTRYPOINT ["dotnet", "AuthenticationService.Api.dll"]