#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["YoloGroup.Developer.API/YoloGroup.Developer.API.csproj", "YoloGroup.Developer.API/"]
RUN dotnet restore "YoloGroup.Developer.API/YoloGroup.Developer.API.csproj"
COPY . .
WORKDIR "/src/YoloGroup.Developer.API"
RUN dotnet build "YoloGroup.Developer.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "YoloGroup.Developer.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /src
COPY /YoloGroup.Developer.API/Files/ /app/Files/
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YoloGroup.Developer.API.dll"]