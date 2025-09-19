FROM mcr.microsoft.com/dotnet/sdk:9.0 AS dev

WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./

CMD ["dotnet", "run", "--urls", "http://0.0.0.0:5000"]