FROM mcr.microsoft.com/dotnet/sdk:8.0 AS publish

COPY ./src src
COPY ./GitVersion.yml GitVersion.yml

RUN dotnet build "src/demo/Saritasa.NetForge.Demo/Saritasa.NetForge.Demo.csproj" -c Release -o /src/demo/Saritasa.NetForge.Demo/bin/Debug/net8.0

FROM alpine:3.18 AS runtime
RUN addgroup -g 1000 nginx &&\
    adduser -u 1000 -G nginx -s /sbin/nologon -D nginx &&\
    apk --update upgrade &&\
    apk add --no-cache nginx curl &&\
    curl -LSs -o /usr/bin/rattus \
    https://github.com/Saritasa/rattus/releases/download/0.2/rattus-linux-amd64 &&\
    chmod +x /usr/bin/rattus

COPY --from=publish /app/wwwroot /app
COPY --from=publish /app/wwwroot/nginx /etc/nginx/

RUN touch /run/nginx.pid &&\
    chown -R nginx:nginx /run/nginx.pid /app /etc/nginx &&\
    chmod 775 /run/nginx.pid /app

WORKDIR /app

EXPOSE 8080
EXPOSE 443

USER nginx

ENTRYPOINT ["nginx"]
CMD ["-g", "daemon off;"]
