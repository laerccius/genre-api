FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
RUN apt-get update
RUN apt-get install unzip
ENV CONSUL_VERSION=1.6.1

ADD https://releases.hashicorp.com/consul/${CONSUL_VERSION}/consul_${CONSUL_VERSION}_linux_amd64.zip /tmp/consul.zip
RUN cd /bin && unzip /tmp/consul.zip && chmod +x /bin/consul && rm /tmp/consul.zip

ADD ./docker/config /etc/consul.d

ADD ./docker/consul.sh /opt

ENTRYPOINT ["dotnet", "genre-api.dll"]

ADD run.sh /opt

WORKDIR /app
COPY --from=build-env /app/out .

