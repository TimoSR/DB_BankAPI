# DB_BankAPI

Solution only works out of the box on windows with docker compose, as macOS can't access localhosted ports and requires to port forwarding before connections can be established.

# Test Solution Locally on Windows

1. cd \DB_BANKAPI
2. docker-compose up -d
3. cd into the src folder of both account_service & transaction_service
4. dotnet run inside both of these folder
5. navigate to the swagger http endpoints below

# Important Endpoints

Account: https://localhost:7101/swagger/index.html

Transaction: https://localhost:7102/swagger/index.html
