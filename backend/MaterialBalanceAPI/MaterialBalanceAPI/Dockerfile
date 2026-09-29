FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["MaterialBalanceAPI.csproj", "./"]
RUN dotnet restore "MaterialBalanceAPI.csproj"
COPY . .
RUN dotnet publish "MaterialBalanceAPI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=80
EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MaterialBalanceAPI.dll"]