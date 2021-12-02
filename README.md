# maw-solr-indexer

A small utility to populate the solr index for mikeandwan.us, now that data import is being deprecated.

## Example Usage
./MawSolrIndexer 'Server=localhost;Port=5432;Database=maw_website;User Id=svc_www_maw;Password=???' http://localhost:8983/solr/multimedia-categories/update/?commit=true
