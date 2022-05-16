#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["uMini.Web/uMini.Web.csproj", "uMini.Web/"]
COPY ["uMini.Infrastructure/uMini.Infrastructure.csproj", "uMini.Infrastructure/"]
RUN dotnet restore "uMini.Web/uMini.Web.csproj"
COPY . .
WORKDIR "/src/uMini.Web"
RUN dotnet build "uMini.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "uMini.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "uMini.Web.dll"]