# Event Sourcing implementation

## First steps

```bash
# First time use
docker run --name esdb-node -it -p 2113:2113 eventstore/eventstore:24.6.0-jammy --insecure --run-projections=All --enable-atom-pub-over-http
# Tip: If you already configured the container with the command above, next time you need to run the DB just run
docker start esdb-node
```

Access via `http://localhost:2113/`