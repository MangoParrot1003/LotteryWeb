# CORS 跨域问题解决方案

## 📋 问题描述

使用 http-server 或其他静态服务器运行前端时，浏览器控制台出现 CORS 错误：

```
Cross-Origin Request Blocked: The Same Origin Policy disallows reading 
the remote resource at http://localhost:5000/api/lottery/students. 
(Reason: CORS header 'Access-Control-Allow-Origin' missing).
```

## 🔍 原因分析

- **开发模式**：前端运行在 `http://localhost:5173`（Vite 默认端口）
- **生产模式**：使用 http-server 时，默认运行在 `http://localhost:8080`
- **问题**：后端 CORS 配置只允许特定端口，http-server 的端口不在白名单中

## ✅ 解决方案

### 方案一：允许所有本地端口（推荐用于开发）

已在 `Program.cs` 中实现：

```csharp
// 配置 CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVue", policy =>
    {
        // 开发环境：允许所有本地端口
        if (builder.Environment.IsDevelopment())
        {
            policy.SetIsOriginAllowed(origin =>
            {
                if (string.IsNullOrWhiteSpace(origin)) return false;
                
                // 允许所有 localhost 和 127.0.0.1
                if (origin.StartsWith("http://localhost:", StringComparison.OrdinalIgnoreCase) ||
                    origin.StartsWith("http://127.0.0.1:", StringComparison.OrdinalIgnoreCase) ||
                    origin.StartsWith("https://localhost:", StringComparison.OrdinalIgnoreCase) ||
                    origin.StartsWith("https://127.0.0.1:", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                
                return false;
            })
            .AllowAnyHeader()
            .AllowAnyMethod();
        }
        else
        {
            // 生产环境：只允许指定域名
            policy.WithOrigins("https://your-domain.com")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
    });
});
```

**优点**：
- ✅ 支持任意本地端口
- ✅ 开发更灵活
- ✅ 生产环境仍然安全

### 方案二：指定 http-server 端口

在 CORS 配置中添加 8080 端口：

```csharp
policy.WithOrigins(
    "http://localhost:5173",  // Vite
    "http://localhost:5174",  // Vite 备用
    "http://localhost:8080",  // http-server
    "http://localhost:3000"   // 其他
)
```

### 方案三：指定 http-server 使用特定端口

启动 http-server 时指定端口：

```bash
# 使用 5173 端口（与 Vite 相同）
http-server dist -p 5173

# 或使用 3000 端口
http-server dist -p 3000
```

## 🚀 使用步骤

### 1. 重新编译后端

```bash
cd Kiro-ver/lottery-backend
dotnet build
```

### 2. 启动后端

```bash
dotnet run
```

或使用启动脚本：
```bash
./start-backend.bat
```

### 3. 启动前端

**开发模式**：
```bash
cd Kiro-ver/lottery-frontend
pnpm run dev
```

**生产模式（http-server）**：
```bash
cd Kiro-ver/lottery-frontend
pnpm run build
http-server dist
```

### 4. 访问系统

- 开发模式：http://localhost:5173
- 生产模式：http://localhost:8080（或 http-server 显示的端口）

## 🔒 生产环境配置

### 修改生产环境 CORS

编辑 `Program.cs`，将生产环境的域名改为实际域名：

```csharp
else
{
    // 生产环境：只允许指定域名
    policy.WithOrigins("https://your-actual-domain.com")
          .AllowAnyHeader()
          .AllowAnyMethod();
}
```

### 使用环境变量

更灵活的方式是使用环境变量：

```csharp
var allowedOrigins = builder.Configuration
    .GetSection("AllowedOrigins")
    .Get<string[]>() ?? new[] { "http://localhost:5173" };

policy.WithOrigins(allowedOrigins)
      .AllowAnyHeader()
      .AllowAnyMethod();
```

在 `appsettings.json` 中配置：

```json
{
  "AllowedOrigins": [
    "http://localhost:5173",
    "http://localhost:8080",
    "https://your-domain.com"
  ]
}
```

## 🧪 测试 CORS

### 使用浏览器开发者工具

1. 打开浏览器开发者工具（F12）
2. 切换到 Network 标签
3. 刷新页面
4. 查看 API 请求的响应头

**正确的响应头应包含**：
```
Access-Control-Allow-Origin: http://localhost:8080
Access-Control-Allow-Methods: GET, POST, PUT, DELETE, OPTIONS
Access-Control-Allow-Headers: *
```

### 使用 curl 测试

```bash
# 测试预检请求
curl -X OPTIONS http://localhost:5000/api/lottery/students \
  -H "Origin: http://localhost:8080" \
  -H "Access-Control-Request-Method: GET" \
  -v

# 测试实际请求
curl http://localhost:5000/api/lottery/students \
  -H "Origin: http://localhost:8080" \
  -v
```

## 📝 常见问题

### Q1: 修改后仍然有 CORS 错误

**解决方案**：
1. 确保后端已重新编译和重启
2. 清除浏览器缓存
3. 使用无痕模式测试

### Q2: 生产环境 CORS 错误

**解决方案**：
1. 检查环境变量 `ASPNETCORE_ENVIRONMENT`
2. 确保生产环境配置了正确的域名
3. 检查 HTTPS 配置

### Q3: 预检请求失败

**解决方案**：
确保 CORS 配置在 `UseAuthorization()` 之前：

```csharp
app.UseCors("AllowVue");  // 必须在这里
app.UseAuthorization();
app.MapControllers();
```

## 🔧 调试技巧

### 1. 查看后端日志

后端会记录 CORS 相关的日志：

```
info: Microsoft.AspNetCore.Cors.Infrastructure.CorsService[2]
      CORS policy execution successful.
```

### 2. 使用 Swagger 测试

访问 http://localhost:5000 使用 Swagger UI 测试 API，Swagger 不受 CORS 限制。

### 3. 临时禁用 CORS（仅用于调试）

```csharp
// 仅用于调试，不要在生产环境使用！
policy.AllowAnyOrigin()
      .AllowAnyHeader()
      .AllowAnyMethod();
```

## 📚 相关资源

- [ASP.NET Core CORS 文档](https://docs.microsoft.com/aspnet/core/security/cors)
- [MDN CORS 文档](https://developer.mozilla.org/zh-CN/docs/Web/HTTP/CORS)
- [http-server 文档](https://www.npmjs.com/package/http-server)

---

**更新时间**：2025-12-06  
**问题状态**：✅ 已解决
