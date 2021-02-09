FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 19350
ENV ASPNETCORE_URLS=http://*:19350

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["api/src/TicTacToe.Api/TicTacToe.Api.csproj", "api/src/TicTacToe.Api/"]
RUN dotnet restore "api/src/TicTacToe.Api/TicTacToe.Api.csproj"
COPY . .
WORKDIR "/src/api/src/TicTacToe.Api"
RUN dotnet build "TicTacToe.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TicTacToe.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TicTacToe.Api.dll"]
