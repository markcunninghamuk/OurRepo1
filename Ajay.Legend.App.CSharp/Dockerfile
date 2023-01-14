#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Ajay.Legend.App.CSharp/Ajay.Legend.App.CSharp.csproj", "Ajay.Legend.App.CSharp/"]

# Adding missing certificate to allow access to githubusercontent.com below.
# We don't need to do this for build servers but when building locally the certificates required for
# checking the Certificate Revocation List may be blocked and the curl would fail.
COPY "cisco_umbrella_root_ca.crt" "/usr/local/share/ca-certificates/cisco_umbrella_root_ca.crt"
RUN chmod 644 /usr/local/share/ca-certificates/cisco_umbrella_root_ca.crt
RUN update-ca-certificates

COPY nuget.config .

# set up NuGet credential provider
RUN curl -L "https://raw.githubusercontent.com/Microsoft/artifacts-credprovider/master/helpers/installcredprovider.sh" | sh
ARG INTERNAL_FEED_ACCESSTOKEN
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS "{\"endpointCredentials\": [{\"endpoint\":\"https://pkgs.dev.azure.com/defragovuk/_packaging/DEFRA-CDP-NUGET_FEED%40Local/nuget/v3/index.json\", \"password\":\"${INTERNAL_FEED_ACCESSTOKEN}\"}]}"

RUN dotnet restore "Ajay.Legend.App.CSharp/Ajay.Legend.App.CSharp.csproj"
COPY . .
WORKDIR "/src/Ajay.Legend.App.CSharp"

RUN dotnet build "Ajay.Legend.App.CSharp.csproj" -c Release -o /app/build

# Install nodejs (for npm)
RUN apt-get update -yq \
    && curl -L https://deb.nodesource.com/setup_18.x | bash \
    && apt-get update -yq \
    && apt-get install -yq \
        nodejs

# Install gulp to run the gulp build for generating the gov.uk css
RUN npm install gulp
RUN npm run build

FROM build AS publish
RUN dotnet publish "Ajay.Legend.App.CSharp.csproj" -c Release -o /app/publish /p:UseAppHost=false

ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ajay.Legend.App.CSharp.dll"]