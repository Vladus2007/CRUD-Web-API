# 📝 NoteFlow API

![.NET](https://img.shields.io/badge/.NET-7.0-purple)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-7.0-blue)
![Entity Framework](https://img.shields.io/badge/Entity_Framework-7.0-green)
![JWT](https://img.shields.io/badge/JWT-Auth-orange)
![SQLite](https://img.shields.io/badge/SQLite-Database-lightgrey)

**NoteFlow** — это современное Web API для управления персональными заметками с полной системой аутентификации и авторизации. Платформа предоставляет безопасный и масштабируемый backend для заметочных приложений.

## ✨ Возможности

### 🔐 Безопасность
- **JWT аутентификация** с refresh токенами
- **Ролевая авторизация** (Admin, User)
- **Хеширование паролей** по стандартам Identity
- **Защита от brute-force** атак

### 📋 Управление заметками
- ✅ Создание и редактирование заметок
- ✅ Получение списка заметок пользователя
- ✅ Поиск и фильтрация заметок
- ✅ Автоматическое обновление дат изменений

### 🛠 Технологии
- **ASP.NET Core 7.0** - современный web framework
- **Entity Framework Core 7.0** - ORM для работы с данными
- **SQLite** - легковесная база данных
- **JWT Bearer Authentication** - безопасная аутентификация
- **Swagger/OpenAPI** - документация и тестирование API

## 🚀 Быстрый старт

### Предварительные требования

- [.NET 7.0 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) или [VS Code](https://code.visualstudio.com/)
- [Postman](https://www.postman.com/) (для тестирования API)

### Установка и запуск

```bash
# Клонирование репозитория
git clone https://github.com/Vladus2007/CRUD-Web-API.git
cd noteflow-api

# Восстановление зависимостей
dotnet restore

# Создание базы данных и применение миграций
dotnet ef database update

# Запуск приложения
dotnet run
```

Приложение будет доступно по адресу: `https://localhost:7171`

## 📖 Документация API

### 🔐 Аутентификация

#### Регистрация нового пользователя
```http
POST /api/auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePassword123!",
  "firstName": "Иван",
  "lastName": "Иванов"
}
```

#### Вход в систему
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePassword123!"
}
```

**Ответ:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5...",
  "expires": "2024-12-31T23:59:59Z",
  "user": {
    "id": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
    "email": "user@example.com",
    "firstName": "Иван",
    "lastName": "Иванов"
  }
}
```

### 📝 Управление заметками

#### Получить все заметки пользователя
```http
GET /api/notes
Authorization: Bearer {token}
```

#### Создать новую заметку
```http
POST /api/notes
Authorization: Bearer {token}
Content-Type: application/json

{
  "title": "Моя первая заметка",
  "description": "Это содержание моей заметки"
}
```

#### Обновить заметку
```http
PUT /api/notes/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
  "title": "Обновленный заголовок",
  "description": "Обновленное содержание"
}
```

#### Удалить заметку
```http
DELETE /api/notes/{id}
Authorization: Bearer {token}
```

## ⚙️ Конфигурация

### Настройки JWT в appsettings.json

```json
{
  "JwtSettings": {
    "Secret": "YourSuperSecretKeyHereAtLeast32CharactersLong123!",
    "ExpiryInMinutes": 60,
    "RefreshTokenExpiryInDays": 7
  },
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=NoteFlow.db"
  }
}
```

### Настройки пароля

```csharp
options.Password.RequiredLength = 14;
options.Password.RequireDigit = false;
options.Password.RequireLowercase = false;
options.Password.RequireUppercase = false;
options.Password.RequireNonAlphanumeric = false;
```

## 🏗 Архитектура

```
NoteFlow.API/
│
├── Controllers/
│   ├── AuthController.cs      # Аутентификация и регистрация
│   └── NotesController.cs     # Управление заметками
│
├── Models/
│   ├── UserModel.cs           # Модель пользователя
│   ├── Note.cs               # Модель заметки
│   └── Requests/
│       ├── RegisterRequest.cs
│       └── LoginRequest.cs
│
├── AppDbContext/
│   └── ApplicationContext.cs  # Контекст базы данных
│
├── Migrations/               # Миграции Entity Framework
└── Program.cs               # Конфигурация приложения
```

## 🔒 Безопасность

### Реализованные меры безопасности:
- ✅ Валидация входных данных
- ✅ Защита от XSS атак
- ✅ SQL injection protection через EF Core
- ✅ Rate limiting на endpoint аутентификации
- ✅ HTTPS перенаправление

### Ролевая модель:
- **User** - базовые права: CRUD собственных заметок
- **Admin** - расширенные права: управление пользователями

## 🧪 Тестирование

### Использование Swagger
Откройте в браузере: `https://localhost:7171/swagger`

### Примеры запросов через curl

```bash
# Регистрация
curl -X POST "https://localhost:7171/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","password":"password12345","firstName":"Test","lastName":"User"}'

# Получение заметок
curl -X GET "https://localhost:7171/api/notes" \
  -H "Authorization: Bearer your_jwt_token_here"
```

## 🚀 Развертывание

### Docker развертывание

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["NoteFlow.API.csproj", "."]
RUN dotnet restore "NoteFlow.API.csproj"
COPY . .
RUN dotnet build "NoteFlow.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NoteFlow.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NoteFlow.API.dll"]
```

## 📈 Мониторинг и логирование

Приложение включает:
- Structured logging в консоль и файлы
- Health check endpoints
- Request/response logging
- Error handling middleware

## 🤝 Вклад в проект

1. Форкните репозиторий
2. Создайте feature branch: `git checkout -b feature/AmazingFeature`
3. Закоммитьте изменения: `git commit -m 'Add AmazingFeature'`
4. Запушьте branch: `git push origin feature/AmazingFeature`
5. Откройте Pull Request

## 📄 Лицензия

Этот проект лицензирован под MIT License - смотрите файл [LICENSE](LICENSE) для деталей.

## 👥 Авторы

- Сушко Владислав - https://github.com/Vladus2007

## 🙏 Благодарности

- ASP.NET Core Team
- Entity Framework Team
- JWT Community

---

**⭐ Если вам нравится этот проект, не забудьте поставить звезду на GitHub!**

