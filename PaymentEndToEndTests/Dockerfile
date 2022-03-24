FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PaymentEndToEndTests/PaymentEndToEndTests.csproj", "PaymentEndToEndTests/"]
RUN dotnet restore "PaymentEndToEndTests/PaymentEndToEndTests.csproj"
COPY . .
WORKDIR "/src/PaymentEndToEndTests"
RUN dotnet build "PaymentEndToEndTests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PaymentEndToEndTests.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaymentEndToEndTests.dll"]
