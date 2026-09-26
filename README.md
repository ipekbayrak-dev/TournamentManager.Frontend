# TournamentManager — Frontend

The client for [TournamentManager.Backend](https://github.com/ipekbayrak-dev/TournamentManager.Backend) — a double-elimination tournament management system modeled after the Dota 2 professional tournament format. This project is the ASP.NET Core MVC frontend: it talks to the backend Web API over HTTP/JSON and doesn't own any data itself.

## Role in the Architecture

- **Player portal** — browse tournaments, register a team, view the live bracket, pay entry fees
- **Admin panel** — manage tournaments, teams, players, and match results (Admin role only)
- **Payment UI** — entry fee checkout via Stripe hosted Checkout sessions

## Tech Stack

| Concern | Technology |
|---|---|
| Framework | ASP.NET Core MVC (.NET 10) |
| Styling | Bootstrap 5 + custom CSS |
| Interactivity | Vanilla JS |
| Auth | JWT stored in HttpOnly cookies (AccessToken + RefreshToken) |
| HTTP Client | `IHttpClientFactory` with named client |
| Payments | Stripe hosted Checkout |

## Project Structure

```
TournamentManager.Frontend/
├── Controllers/           # MVC controllers — one per feature area
│   ├── AuthController     # Login, Register, Logout
│   ├── TournamentController
│   ├── TeamController
│   ├── MatchController
│   ├── PlayerController
│   ├── AdminController
│   └── PaymentController  # Stripe checkout redirect + success page
├── Models/                # View models and API response shapes
├── Views/
│   ├── Home/              # Landing page, Compete page
│   ├── Auth/              # Login, Register
│   ├── Tournament/        # List (with search/filter), Detail, Bracket viewer
│   ├── Team/              # Roster, Detail
│   ├── Match/             # Match detail, score display
│   ├── Admin/             # Tournaments, Teams, Players, Pending Players
│   ├── Payment/           # Success page
│   └── Shared/            # Layout (with custom delete modal), Error page
├── wwwroot/
│   ├── css/               # Global styles
│   ├── js/                # Page scripts
│   └── lib/               # Bootstrap, jQuery
└── Program.cs
```

## Pages & Routes

| Page | Route | Access |
|---|---|---|
| Home / Landing | / | Public |
| Compete | /Home/Compete | Public |
| Login | /Auth/Login | Public |
| Register | /Auth/Register | Public |
| Tournament List | /Tournament | Public |
| Tournament Detail | /Tournament/{slug} | Public |
| Bracket View | /Tournament/{slug}/Bracket | Public |
| Team Detail | /Team/{id} | Public |
| Register for Tournament | POST /Tournament/RegisterTeam | Captain |
| Payment Checkout | /Payment/Checkout | Authenticated |
| Payment Success | /Payment/Success | Authenticated |
| Admin Dashboard | /Admin | Admin |
| Manage Tournaments | /Admin/Tournaments | Admin |
| Manage Teams | /Admin/Teams | Admin |
| Manage Players | /Admin/Players | Admin |
| Pending Players | /Admin/PendingPlayers | Admin |

## Authentication Flow

1. User logs in via `/Auth/Login` → Frontend POSTs to `POST /api/Auth/login`
2. JWT access token and refresh token stored in **HttpOnly cookies**
3. All subsequent API calls read the token from the cookie and forward it as `Authorization: Bearer {token}`
4. Token expiry handled automatically via the refresh endpoint
5. Roles decoded from JWT claims — Admin gets additional UI controls

## Prerequisites

- .NET 10 SDK
- [TournamentManager.Backend](https://github.com/ipekbayrak-dev/TournamentManager.Backend) running locally

## Getting Started

**1. Start the backend first** (see backend README)

**2. Start Stripe webhook forwarding** (in a separate terminal):
```
stripe listen --forward-to https://localhost:7008/api/Payment/webhook
```

**3. Run the frontend:**
```
dotnet run
```

The frontend runs at `https://localhost:7049` by default.

## Related

- [TournamentManager.Backend](https://github.com/ipekbayrak-dev/TournamentManager.Backend)

## License

MIT — see [LICENSE](LICENSE)
