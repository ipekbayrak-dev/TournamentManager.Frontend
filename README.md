# TournamentManager.Frontend

The client for [TournamentManager](https://github.com/ipekbayrak-dev/TournamentManager.Backend) — a double-elimination tournament management system modeled after the Dota 2 tournament format. This project is the ASP.NET Core MVC "Solution 1" in the overall architecture: it talks to the backend Web API over HTTP/JSON with JWT auth, it doesn't own any data itself.

## Role in the architecture

- **Player portal** — browse tournaments, register a team, view the live bracket
- **Admin panel** — manage tournaments, report match results (Admin role only)
- **Payment UI** — entry fee checkout, hosted payment page, success/cancel pages *(planned — backend Stripe integration isn't wired up yet, see Status below)*

## Tech Stack

- **.NET 10** — ASP.NET Core MVC
- **HttpClient** — calls the backend API (JWT bearer auth)
- Bootstrap + jQuery (template defaults)

## Prerequisites

- .NET 10 SDK
- [TournamentManager.Backend](https://github.com/ipekbayrak-dev/TournamentManager.Backend) running locally (`https://localhost:7008` / `http://localhost:5147` by default)

## Getting Started

```
dotnet run
```

API base URL configuration and auth wiring are not implemented yet — see Status.

## Status

This repo currently holds the bare `dotnet new mvc` scaffold and nothing else. No auth flow, API client, or feature pages have been built yet. Being built incrementally, mentorship-style, alongside the backend.

## License

MIT — see [LICENSE](LICENSE)
