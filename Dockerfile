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
EXPOSE 80
HEALTHCHECK CMD curl --fail http://localhost:80/health || exit
ENTRYPOINT ["dotnet", "HueApiProxy.dll"]