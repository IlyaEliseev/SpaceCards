# SpaceCards

Space Cards is a application for guessing foreign words. App allows create a foreign word card where is stored the word and translation and combine words into groups. You can get statistics guessed cards to mail and telegram.

### Setup

Follow these steps to get your development environment set up:

1. Install `wsl` and Linux distribution:

   ```
   wsl --install
   wsl --install -d <Distribution Name>
   ```

2. Install `Docker desktop` https://docs.docker.com/desktop/install/windows-install/

3. Clone the repository

4. At the root directory, restore required packages by running:

   ```
   dotnet restore
   ```

5. At the root directory, restore required packages by running:
   ```
   dotnet restore
   ```
6. Next, build the solution by running:

   ```
   dotnet build
   ```

7. Next, run `cmd` or `PowerShell` and install `dotnet ef`:

   ```
   dotnet tool install --global dotnet-ef
   ```

8. Next, within the `\backend` directory run `cmd` and update database:

   ```
   dotnet ef database updata -p SpaceCards.DataAccess.Postgre -s SpaceCards.API
   ```

9. Next, within the `\backend\SpaceCards.API` directory, launch the back end by running:

   ```
   dotnet run
   ```

10. Next, within the `\frontend\spacecards` directory, launch the front end by running:

```
npm start
```

11. Launch [https://localhost:49394/swagger/index.html](https://localhost:49394/swagger/index.html) in your browser to view the API

## Technologies

### Backend

- .NET 6
- ASP.NET Core
- Entity Framework Core
- PostgreSql
- Docker

### Frontend

- React
- TypeScript

### Logging and Tracing

- Seq
- Jaeger

### Unit and Integration tests

- XUnit

## Architecture and design Api

![DesignApi](https://github.com/IlyaEliseev/SpaceCards/blob/main/Docs/ArchitectureDesignApi.jpg)

## Database schema

![DbSchema](https://github.com/IlyaEliseev/SpaceCards/blob/main/Docs/SpaceCardsDbSchema.jpg)
