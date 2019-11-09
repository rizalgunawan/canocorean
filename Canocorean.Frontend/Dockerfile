FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Canocorean.Frontend/Canocorean.Frontend.csproj", "Canocorean.Frontend/"]
COPY ["Canocorean.Infrastructure/Canocorean.Infrastructure.csproj", "Canocorean.Infrastructure/"]
RUN dotnet restore "Canocorean.Frontend/Canocorean.Frontend.csproj"
COPY . .
RUN apt-get update -yq && apt-get upgrade -yq && apt-get install -yq curl git nano
RUN curl -sL https://deb.nodesource.com/setup_12.x | bash - && apt-get install -yq nodejs build-essential
RUN npm install -g npm
RUN npm install -g @angular/cli@8.3.18
WORKDIR "/src/Canocorean.Frontend"
RUN dotnet build "Canocorean.Frontend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Canocorean.Frontend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Canocorean.Frontend.dll"]
