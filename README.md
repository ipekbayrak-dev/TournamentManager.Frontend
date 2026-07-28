# TournamentManager — Frontend

The client for [TournamentManager.Backend](https://github.com/ipekbayrak-dev/TournamentManager.Backend) — a double-elimination tournament management system modeled after the Dota 2 professional tournament format. This project is the ASP.NET Core MVC frontend: it talks to the backend Web API over HTTP/JSON with JWT auth and doesn't own any data itself.

## Role in the Architecture

- **Player portal** — browse tournaments, register a team, view the live bracket
- **Admin panel** — manage tournaments, report match results (Admin role only)
- **Payment UI** — entry fee checkout via Stripe Elements *(planned — backend Stripe integration is wired and ready)*

## Tech Stack

| Concern | Technology |
|---|---|
| Framework | ASP.NET Core MVC (.NET 10) |
| Styling | Bootstrap 5 + custom CSS |
| Interactivity | Vanilla JS + jQuery |
| Auth | JWT (forwarded to API via HttpClient) |
| HTTP Client | Typed `HttpClient` services |
| Payment UI | Stripe Elements *(planned)* |

## Project Structure

```
TournamentManager.Frontend/
├── Controllers/           # MVC controllers — one per feature area
├── Models/                # View models and API response shapes
├── Views/
│   ├── Home/              # Landing page
│   ├── Auth/              # Login, Register
│   ├── Tournament/        # List, Detail, Bracket viewer
│   ├── Team/              # Roster, Detail
│   ├── Match/             # Match detail, score display
│   ├── Admin/             # Admin dashboard
│   └── Shared/            # Layout, partials
├── Services/              # Typed HttpClient wrappers per API resource
├── wwwroot/
│   ├── css/               # Global styles
│   ├── js/                # Page scripts
│   └── lib/               # Bootstrap, jQuery
└── Program.cs
```

## Planned Pages & Routes

| Page | Route | Access |
|---|---|---|
| Home / Landing | / | Public |
| Login | /Auth/Login | Public |
| Register | /Auth/Register | Public |
| Tournament List | /Tournament | Public |
| Tournament Detail | /Tournament/{id} | Public |
| Bracket View | /Tournament/{id}/Bracket | Public |
| Team Detail | /Team/{id} | Public |
| Register for Tournament | /Tournament/{id}/Register | Player |
| Admin Dashboard | /Admin | Admin |
| Manage Tournaments | /Admin/Tournament | Admin |
| Manage Teams | /Admin/Team | Admin |
| Manage Matches & Results | /Admin/Match | Admin |

## Authentication Flow

1. User logs in via `/Auth/Login` → Frontend POSTs to `POST /api/Auth/login`
2. JWT access token and refresh token stored in session
3. All subsequent API calls include `Authorization: Bearer {token}`
4. Token expiry handled automatically via refresh endpoint
5. Roles decoded from JWT claims — Admin/Captain/Player get different UI

## Prerequisites

- .NET 10 SDK
- [TournamentManager.Backend](https://github.com/ipekbayrak-dev/TournamentManager.Backend) running locally (`https://localhost:7008` / `http://localhost:5147`)

## Getting Started

**1. Start the backend first** (see backend README)

**2. Configure the API base URL** in `appsettings.json`:
```json
"ApiSettings": {
  "BaseUrl": "http://localhost:5147"
}
```

**3. Run the frontend:**
```
dotnet run
```

## Status

Active development. Auth flow, typed API clients, and feature pages are being built incrementally. Backend is complete and fully tested — frontend is next.

- [x] Project scaffold
- [ ] Auth flow (login, register, token refresh)
- [ ] Typed HttpClient services for all API resources
- [ ] Tournament pages
- [ ] Bracket viewer
- [ ] Team & player pages
- [ ] Admin panel
- [ ] Stripe payment UI

## Related

- [TournamentManager.Backend](https://github.com/ipekbayrak-dev/TournamentManager.Backend)

## License

MIT — see [LICENSE](LICENSE)
