FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine-amd64 AS build

WORKDIR /

COPY MawSolrIndexer.sln .
COPY src/. src/

RUN dotnet publish -o /build -c Release -r linux-musl-x64 --self-contained false


# runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine-amd64

RUN apk --no-cache add bash

WORKDIR /maw-solr-indexer

COPY --from=build /build /maw-solr-indexer/

ENTRYPOINT [ "/maw-solr-indexer/MawSolrIndexer" ]
