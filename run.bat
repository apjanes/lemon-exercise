start cmd /k dotnet run --project TaskList.WebApi
start cmd /k dotnet run --project TaskList.WebApp

:: wait a bit for servers to start (5 seconds)
timeout /t 5 /nobreak >nul

:: open in default browser
start "" https://localhost:4000