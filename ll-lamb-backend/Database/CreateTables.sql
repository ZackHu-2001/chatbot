-- Azure SQL Database 建表语句

-- 用户表
CREATE TABLE Users (
    UserId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Salt NVARCHAR(255) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    IsActive BIT DEFAULT 1
);

-- 用户会话表（用于JWT令牌管理）
CREATE TABLE UserSessions (
    SessionId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    RefreshToken NVARCHAR(500) NOT NULL,
    ExpiresAt DATETIME2 NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    IsRevoked BIT DEFAULT 0,
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
);

-- 模型表
CREATE TABLE Models (
    ModelId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(50) NOT NULL,
    Provider NVARCHAR(50) NOT NULL,
    Endpoint NVARCHAR(200) NOT NULL,
    ApiKey NVARCHAR(200) NOT NULL,
    Status BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE()
);

-- 聊天会话表
CREATE TABLE ChatSessions (
    SessionId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    ModelId UNIQUEIDENTIFIER NOT NULL,
    Title NVARCHAR(100) NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 DEFAULT GETUTCDATE(),
    IsActive BIT DEFAULT 1,
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
    FOREIGN KEY (ModelId) REFERENCES Models(ModelId)
);

-- 消息表
CREATE TABLE Messages (
    MessageId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SessionId UNIQUEIDENTIFIER NOT NULL,
    Role NVARCHAR(20) NOT NULL CHECK (Role IN ('User', 'Assistant', 'System')),
    Content NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    TokenCount INT NULL,
    FOREIGN KEY (SessionId) REFERENCES ChatSessions(SessionId) ON DELETE CASCADE
);

-- 创建索引
CREATE INDEX IX_Users_Username ON Users(Username);
CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_UserSessions_UserId ON UserSessions(UserId);
CREATE INDEX IX_UserSessions_RefreshToken ON UserSessions(RefreshToken);
CREATE INDEX IX_ChatSessions_UserId ON ChatSessions(UserId);
CREATE INDEX IX_ChatSessions_ModelId ON ChatSessions(ModelId);
CREATE INDEX IX_Messages_SessionId ON Messages(SessionId);
CREATE INDEX IX_Messages_CreatedAt ON Messages(CreatedAt);

-- 插入示例模型数据
INSERT INTO Models (ModelId, Name, Provider, Endpoint, ApiKey, Status) VALUES
(NEWID(), 'GPT-4', 'Azure OpenAI', 'https://your-resource.openai.azure.com/', 'your-api-key', 1),
(NEWID(), 'GPT-3.5-Turbo', 'Azure OpenAI', 'https://your-resource.openai.azure.com/', 'your-api-key', 1),
(NEWID(), 'Claude-3-Sonnet', 'Anthropic', 'https://api.anthropic.com/', 'your-api-key', 1);