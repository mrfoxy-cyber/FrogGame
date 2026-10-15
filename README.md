# Froggy Puzzle Pop

Froggy Puzzle Pop is a .NET game built as a Clean Architecture exercise. The API owns game sessions, board rules, moves, scores, and level progression. A separate frontend project owns presentation, assets, and animation.

## Versions in this repository

- `src/`, `tests/`, and `FrogGame.slnx`: the current C# / ASP.NET Core version described below.
- `web/`: the earlier standalone browser version.
- `Assets/`, `Packages/`, and `ProjectSettings/`: the preserved original Unity project.

The earlier versions and their Git history are retained for reference.

## Architecture

Dependencies point inward:

```text
FrogGame.Frontend ──HTTP──→ FrogGame.Web (API)
                                  │
                                  ├→ FrogGame.Application ──→ FrogGame.Domain
                                  └→ FrogGame.Infrastructure ────────┘
```

- **Domain** contains frogs, boards, scores, matching, gravity, combos, and levels.
- **Application** coordinates use cases and owns ports for persistence and random frog selection.
- **Infrastructure** implements those ports with JSON files, memory, and the system random source.
- **Web** is the API composition root and translates HTTP requests into application commands.
- **Frontend** is an independent web app containing the HTML, CSS, JavaScript, and game assets.

`FrogColor` and `Score` are values. Interfaces describe behavior or an outside boundary: `IScoreCalculator`, `IGameRepository`, `IScoreRepository`, and `IFrogProvider`.

## Run it

You need the .NET 10 SDK.

Start the API in one terminal:

```powershell
dotnet run --project src/FrogGame.Web
```

Start the frontend in a second terminal:

```powershell
dotnet run --project src/FrogGame.Frontend
```

The game opens at `http://localhost:64910` and calls the API at `http://localhost:64909`. Change `ApiBaseUrl` in `src/FrogGame.Frontend/appsettings.json` and `FrontendOrigins` in `src/FrogGame.Web/appsettings.json` when deploying to different addresses.

## API

- `GET /api/health` checks that the server is running.
- `POST /api/games` starts an authoritative game session.
- `GET /api/games/{id}` returns its current state.
- `POST /api/games/{id}/moves` drops a frog into a column.
- `GET /api/scores` returns the top score, best combo, and ten highest runs.

The Application layer records a leaderboard entry only when a game reaches a terminal state. There is deliberately no public score-writing endpoint, so the browser cannot submit an invented score.

Scores are saved in `src/FrogGame.Web/App_Data/scores.json` when the first run is submitted. That runtime file is intentionally excluded from Git. Active game sessions currently live in memory and reset when the server restarts.

## Verify the game

```powershell
dotnet build FrogGame.slnx -m:1
dotnet run --project tests/FrogGame.Domain.Specs
node tools/verify-game.cjs
node tools/verify-audio.cjs
```
