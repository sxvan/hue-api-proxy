FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /app
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
WORKDIR /app

ENV TZ=Europe/Zurich
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

RUN apk --no-cache add curl

RUN addgroup --gid 1000 -S app && adduser --uid 1000 -S app -G app
RUN chown -R app:app /app
USER app

COPY --from=build /app/out .
ENV DOTNET_EnableDiagnostics=0
ENV ASPNETCORE_HTTP_PORTS=8080
ENV ASPNETCORE_URLS=http://*:8080
EXPOSE 8080
HEALTHCHECK --interval=5m --timeout=3s CMD curl --fail http://localhost:8080/health || exit
ENTRYPOINT ["dotnet", "HueApiProxy.dll"]
