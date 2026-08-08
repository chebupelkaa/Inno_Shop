# Inno_Shop

Микросервисы на ASP.NET Core (.NET 10) + SQL Server + JWT:

- **UserService** — регистрация, подтверждение email, login, CRUD пользователей, reset password
- **ProductService** — CRUD продуктов, поиск/фильтры, ownership по JWT

## Docker Compose

```bash
docker compose up --build
```

| Сервис | URL |
|--------|-----|
| UserService Swagger | http://localhost:8081/swagger |
| ProductService Swagger | http://localhost:8082/swagger |
| MailHog UI (письма) | http://localhost:8025 |
| SQL Server | `localhost:1433` (sa / см. `.env.example`) |

Опционально: скопируй [`.env.example`](.env.example) → `.env` и задай `MSSQL_SA_PASSWORD`, `JWT_SECRET`.

Оба API используют один `Jwt__Secret` / Issuer / Audience. При старте накатываются EF-миграции.

## Локально без Docker

1. SQL Server LocalDB
2. User Secrets в API-проектах (`ConnectionStrings`, `Jwt:Secret`, email)
3. Запуск `UserService.API` и `ProductService.API`

## Endpoints

### UserService (`http://localhost:8081`)

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

### ProductService (`http://localhost:8082`)

| Method | Path | Auth |
|--------|------|------|
| GET | `/api/products` | JWT (query: name, minPrice, maxPrice, availability, userId, createdFrom, createdTo) |
| GET | `/api/products/{id}` | JWT |
| POST | `/api/products` | JWT (свой UserId) |
| PUT | `/api/products/{id}` | JWT (только свой продукт) |
| DELETE | `/api/products/{id}` | JWT (только свой продукт) |

## Тестовый flow

1. **Register**  
   `POST http://localhost:8081/api/auth/register`  
   ```json
   { "name": "Test User", "email": "user@example.com", "password": "Password1!" }
   ```

2. **Confirm email**  
   Открой http://localhost:8025 → письмо → ссылка confirm  
   или вручную:  
   `GET http://localhost:8081/api/auth/confirm-email?email=user@example.com&token=<token из письма>`

3. **Login**  
   `POST http://localhost:8081/api/auth/login`  
   ```json
   { "email": "user@example.com", "password": "Password1!" }
   ```  
   Сохрани `accessToken`.

4. **Create product**  
   `POST http://localhost:8082/api/products`  
   Header: `Authorization: Bearer <accessToken>`  
   ```json
   { "name": "Laptop", "description": "Work laptop", "price": 1500, "availability": true }
   ```

## Тесты

```bash
dotnet test Inno_Shop.sln
```

Unit (handlers/validators) + integration (`WebApplicationFactory` + InMemory).
