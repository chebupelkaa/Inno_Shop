# Inno_Shop

UserService + ProductService на ASP.NET Core / .NET 10, SQL Server, JWT.

## Запуск

```bash
# по желанию: cp .env.example .env
docker compose up --build
```

| Что | Куда |
|-----|------|
| UserService | http://localhost:8081/swagger |
| ProductService | http://localhost:8082/swagger |
| MailHog | http://localhost:8025 |
| SQL Server | localhost:1433 (sa, пароль в `.env.example`) |

JWT у обоих сервисов один и тот же. Миграции накатываются при старте.

Без докера — LocalDB + User Secrets, запустить оба API из IDE.

## Тестовый flow

1. `POST /api/auth/register` (users:8081)

```json
{ "name": "Test User", "email": "user@example.com", "password": "Password1!" }
```

2. Подтвердить почту: MailHog → письмо со ссылкой, либо  
   `GET /api/auth/confirm-email?email=...&token=...`

3. `POST /api/auth/login` → взять `accessToken`

```json
{ "email": "user@example.com", "password": "Password1!" }
```

4. `POST /api/products` (products:8082) с `Authorization: Bearer ...`

```json
{ "name": "Laptop", "description": "Work laptop", "price": 1500, "availability": true }
```

Чужой продукт править/удалять нельзя (403).

## Endpoints

### UserService (`:8081`)

| Method | Path | Auth |
|--------|------|------|
| POST | `/api/auth/register` | — |
| POST | `/api/auth/send-email-confirmation` | — |
| GET | `/api/auth/confirm-email?email=&token=` | — |
| POST | `/api/auth/login` | — |
| POST | `/api/auth/refresh-token` | — |
| POST | `/api/auth/forgot-password` | — |
| POST | `/api/auth/reset-password` | — |
| GET | `/api/auth/profile` | JWT |
| GET | `/api/users` | Admin |
| GET | `/api/users/get/{id}` | Admin |
| PUT | `/api/users/update` | JWT |
| DELETE | `/api/users/{id}` | Admin |
| PATCH | `/api/users/{id}/status` | JWT |

### ProductService (`:8082`)

| Method | Path | Auth |
|--------|------|------|
| GET | `/api/products` | JWT (+ фильтры) |
| GET | `/api/products/{id}` | JWT |
| POST | `/api/products` | JWT (свой) |
| PUT | `/api/products/{id}` | JWT (свой) |
| DELETE | `/api/products/{id}` | JWT (свой) |

Фильтры на GET list: `name`, `minPrice`, `maxPrice`, `availability`, `userId`, `createdFrom`, `createdTo`.

## Тесты

```bash
dotnet test Inno_Shop.sln
```
