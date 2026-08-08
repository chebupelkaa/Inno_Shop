# Inno_Shop

Микросервисы управления пользователями и продуктами (ASP.NET Core + SQL Server + JWT).

## Локальный запуск (Docker Compose)

```bash
docker compose up --build
```

| Сервис | URL |
|--------|-----|
| UserService Swagger | http://localhost:8081/swagger |
| ProductService Swagger | http://localhost:8082/swagger |
| MailHog UI | http://localhost:8025 |
| SQL Server | localhost:1433 (sa / см. `.env.example`) |

Опционально скопируй [`.env.example`](.env.example) → `.env` и задай `MSSQL_SA_PASSWORD` / `JWT_SECRET`.

Оба API используют один и тот же `Jwt__Secret` / Issuer / Audience. При старте применяются EF-миграции (`Database.Migrate`).

Письма (confirm / reset password) уходят в MailHog (`mailhog:1025`).

## Локальный запуск без Docker

- SQL Server LocalDB + User Secrets (connection string, JWT, email)
- Запуск `UserService.API` и `ProductService.API` из IDE
