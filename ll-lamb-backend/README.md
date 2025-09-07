# LL Lamb Backend

基于 ASP.NET Core Web API 的后端项目，集成了 Azure SQL Database、Entity Framework Core 和 Azure OpenAI Service。

## 项目结构

```
ll-lamb-backend/
├── Controllers/          # API 控制器
│   ├── AuthController.cs      # 身份验证
│   ├── ChatController.cs      # 聊天功能
│   └── ModelsController.cs    # 模型管理
├── Data/                 # 数据访问层
│   └── ApplicationDbContext.cs
├── DTOs/                 # 数据传输对象
├── Models/               # 数据模型
├── Repositories/         # 仓储层
├── Services/             # 业务逻辑层
└── Database/             # 数据库脚本
    └── CreateTables.sql
```

## 功能特性

- ✅ 用户注册/登录（JWT 身份验证）
- ✅ 获取可用模型列表
- ✅ 创建聊天会话
- ✅ 发送/接收消息（集成 Azure OpenAI）
- ✅ 获取聊天历史记录
- ✅ RESTful API 设计
- ✅ 分层架构（Controller/Service/Repository）

## 技术栈

- ASP.NET Core 8.0 Web API
- Entity Framework Core
- Azure SQL Database
- JWT 身份验证
- Azure OpenAI Service
- Swagger/OpenAPI

## 快速开始

### 1. 环境准备

- .NET 8.0 SDK
- Azure SQL Database 实例
- Azure OpenAI Service 资源

### 2. 配置设置

更新 `appsettings.json` 中的配置：

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "你的Azure SQL连接字符串"
  },
  "JwtSettings": {
    "SecretKey": "你的JWT密钥",
    "Issuer": "LlLambBackend",
    "Audience": "LlLambFrontend",
    "ExpiryMinutes": "60"
  },
  "AzureOpenAI": {
    "Endpoint": "你的Azure OpenAI端点",
    "ApiKey": "你的Azure OpenAI API密钥"
  }
}
```

### 3. 数据库设置

1. 在 Azure 中创建 SQL Database
2. 执行 `Database/CreateTables.sql` 创建表结构
3. 更新模型数据中的 API 密钥和端点

### 4. 运行项目

```bash
# 还原包
dotnet restore

# 运行项目
dotnet run
```

访问 `https://localhost:5001/swagger` 查看 API 文档。

## API 端点

### 身份验证
- `POST /api/auth/register` - 用户注册
- `POST /api/auth/login` - 用户登录
- `POST /api/auth/refresh` - 刷新令牌
- `POST /api/auth/revoke` - 撤销令牌

### 模型管理
- `GET /api/models` - 获取可用模型列表

### 聊天功能
- `POST /api/chat/session` - 创建聊天会话
- `GET /api/chat/sessions` - 获取用户会话列表
- `POST /api/chat/send` - 发送消息
- `GET /api/chat/history/{sessionId}` - 获取聊天历史

## 数据库表结构

### Users（用户表）
- UserId (UNIQUEIDENTIFIER, PK)
- Username (NVARCHAR(50))
- Email (NVARCHAR(100))
- PasswordHash (NVARCHAR(255))
- Salt (NVARCHAR(255))
- CreatedAt, UpdatedAt, IsActive

### Models（模型表）
- ModelId (UNIQUEIDENTIFIER, PK)
- Name (NVARCHAR(50))
- Provider (NVARCHAR(50))
- Endpoint (NVARCHAR(200))
- ApiKey (NVARCHAR(200))
- Status (BIT)

### ChatSessions（聊天会话表）
- SessionId (UNIQUEIDENTIFIER, PK)
- UserId (FK)
- ModelId (FK)
- Title (NVARCHAR(100))
- CreatedAt, UpdatedAt, IsActive

### Messages（消息表）
- MessageId (UNIQUEIDENTIFIER, PK)
- SessionId (FK)
- Role (User/Assistant/System)
- Content (NVARCHAR(MAX))
- CreatedAt
- TokenCount (INT)

## 部署说明

### Azure 部署
1. 创建 Azure App Service
2. 配置应用设置（连接字符串、JWT设置等）
3. 部署代码到 App Service

### 环境变量
生产环境中建议使用环境变量或 Azure Key Vault 管理敏感信息。

## 开发说明

### 添加新功能
1. 在 `Models/` 中定义数据模型
2. 在 `DTOs/` 中定义传输对象
3. 在 `Repositories/` 中实现数据访问
4. 在 `Services/` 中实现业务逻辑
5. 在 `Controllers/` 中暴露 API

### 数据库迁移
使用 Entity Framework Core 命令：
```bash
# 添加迁移
dotnet ef migrations add MigrationName

# 更新数据库
dotnet ef database update
```

## 许可证

MIT License