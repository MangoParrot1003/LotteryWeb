# 抽签系统后端 (C# ASP.NET Core)

## 运行步骤

1. 确保 students.db 数据库文件在项目根目录

2. 运行后端服务：
```bash
cd lottery-backend
dotnet run
```

后端将运行在 http://localhost:5000

## API 接口

- `GET /api/lottery/students` - 获取所有学生
- `GET /api/lottery/students/{id}` - 获取指定学生
- `GET /api/lottery/draw?gender=男&className=1班` - 抽签（可选筛选条件）
- `GET /api/lottery/stats` - 获取统计信息
