FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PaymentBrowserClient/PaymentWebClient.csproj", "PaymentBrowserClient/"]
COPY ["PaymentApplication/PaymentApplication.csproj", "PaymentApplication/"]
COPY ["PaymentCore/PaymentCore.csproj", "PaymentCore/"]
RUN dotnet restore "PaymentBrowserClient/PaymentWebClient.csproj"
COPY . .
WORKDIR "/src/PaymentBrowserClient"
RUN dotnet build "PaymentWebClient.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "PaymentWebClient.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /publish/wwwroot /usr/local/webapp/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY "PaymentBrowserClient/nginx.conf" /etc/nginx/nginx.conf
