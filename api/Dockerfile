# --- build stage ---
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY NumberToWordsApi.csproj ./
RUN dotnet restore NumberToWordsApi.csproj

COPY . .
RUN dotnet publish NumberToWordsApi.csproj -c Release -o /app/publish

# --- runtime stage ---
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

# Render assigns the port to listen on via $PORT at runtime, so this has to be
# resolved by the shell at container start, not baked in at build time.
CMD ASPNETCORE_URLS=http://+:$PORT dotnet NumberToWordsApi.dll
