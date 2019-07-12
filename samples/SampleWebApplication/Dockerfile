FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src
COPY ["samples/SampleWebApplication/SampleWebApplication.csproj", "samples/SampleWebApplication/"]
COPY ["src/Microsoft.Extensions.Metrics/Microsoft.Extensions.Metrics.csproj", "src/Microsoft.Extensions.Metrics/"]
RUN dotnet restore "samples/SampleWebApplication/SampleWebApplication.csproj"
COPY . .
WORKDIR "/src/samples/SampleWebApplication"
RUN dotnet build "SampleWebApplication.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "SampleWebApplication.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SampleWebApplication.dll"]