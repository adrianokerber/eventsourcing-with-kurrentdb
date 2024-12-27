# Event Sourcing implementation

## First steps

```bash
# First time use
docker run --name esdb-node -it -p 2113:2113 eventstore/eventstore:24.6.0-jammy --insecure --run-projections=All --enable-atom-pub-over-http
# Tip: If you already configured the container with the command above, next time you need to run the DB just run
docker start esdb-node
```

Access via `http://localhost:2113/`

## API Endpoints

### Create Payroll Loan Event

```bash
curl -X POST http://localhost:5170/api/payroll-loans \
  -H "Content-Type: application/json" \
  -d '{
    "id": "loan-123",
    "type": "PayrollLoanCreated",
    "data": "{\"amount\": 1000, \"interestRate\": 2.5, \"termMonths\": 12}",
    "createdAt": "2024-12-26T15:27:43-03:00"
  }'
```

### Get All Payroll Loan Events

```bash
curl -X GET http://localhost:5170/api/payroll-loans
```

## Run with Docker Compose

```bash
docker compose up -d
```