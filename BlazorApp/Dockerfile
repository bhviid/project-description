FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /BlazorApp/
COPY /BlazorApp/ .
RUN dotnet restore 
RUN dotnet build BlazorApp.csproj -c Release -o /app/Build

FROM build as publish
RUN dotnet publish BlazorApp.csproj -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY BlazorApp/nginx.conf /etc/nginx/nginx.conf