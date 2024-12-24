FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY ["src/datntdev.MyCodebase.Web.Host/datntdev.MyCodebase.Web.Host.csproj", "src/datntdev.MyCodebase.Web.Host/"]
COPY ["src/datntdev.MyCodebase.Web.Core/datntdev.MyCodebase.Web.Core.csproj", "src/datntdev.MyCodebase.Web.Core/"]
COPY ["src/datntdev.MyCodebase.Application/datntdev.MyCodebase.Application.csproj", "src/datntdev.MyCodebase.Application/"]
COPY ["src/datntdev.MyCodebase.Core/datntdev.MyCodebase.Core.csproj", "src/datntdev.MyCodebase.Core/"]
COPY ["src/datntdev.MyCodebase.EntityFrameworkCore/datntdev.MyCodebase.EntityFrameworkCore.csproj", "src/datntdev.MyCodebase.EntityFrameworkCore/"]
WORKDIR "/src/src/datntdev.MyCodebase.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/datntdev.MyCodebase.Web.Host", "src/datntdev.MyCodebase.Web.Host"]
COPY ["src/datntdev.MyCodebase.Web.Core", "src/datntdev.MyCodebase.Web.Core"]
COPY ["src/datntdev.MyCodebase.Application", "src/datntdev.MyCodebase.Application"]
COPY ["src/datntdev.MyCodebase.Core", "src/datntdev.MyCodebase.Core"]
COPY ["src/datntdev.MyCodebase.EntityFrameworkCore", "src/datntdev.MyCodebase.EntityFrameworkCore"]
WORKDIR "/src/src/datntdev.MyCodebase.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "datntdev.MyCodebase.Web.Host.dll"]
