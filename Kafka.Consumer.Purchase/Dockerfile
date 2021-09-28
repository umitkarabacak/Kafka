#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Kafka.Consumer.Purchase/Kafka.Consumer.Purchase.csproj", "Kafka.Consumer.Purchase/"]
RUN dotnet restore "Kafka.Consumer.Purchase/Kafka.Consumer.Purchase.csproj"
COPY . .
WORKDIR "/src/Kafka.Consumer.Purchase"
RUN dotnet build "Kafka.Consumer.Purchase.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kafka.Consumer.Purchase.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kafka.Consumer.Purchase.dll"]