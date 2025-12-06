# 🎲 学生抽签系统

一个功能完整的学生随机抽签系统，采用现代化的前后端分离架构开发。

## 📖 项目简介

本系统是一个基于 Web 的学生抽签应用，支持从学生数据库中随机抽取学生，并提供多种筛选条件。系统采用前后端分离架构，后端使用 C# ASP.NET Core 提供 RESTful API，前端使用 Vue 3 + TypeScript 构建交互界面，数据存储使用 SQLite 数据库。

### 核心功能

- ✨ **随机抽签** - 公平公正地从学生库中随机抽取
- 🎯 **条件筛选** - 支持按性别、班级筛选抽签
- 📊 **统计展示** - 实时显示学生总数、性别分布、班级分布
- 🎬 **动画效果** - 流畅的抽签动画，提升用户体验
- 💾 **数据管理** - 支持通过 Navicat 等工具管理学生数据

### 技术栈

**后端技术**
- C# ASP.NET Core 8.0
- Entity Framework Core 8.0
- SQLite 数据库
- RESTful API 设计

**前端技术**
- Vue 3.5 (Composition API)
- TypeScript 5.9
- Vite 7.2
- 响应式设计

**开发工具**
- Visual Studio 2022（后端开发）
- VS Code（前端开发）
- pnpm（包管理器）
- Navicat（数据库管理）

## 🚀 快速开始

### 系统要求

- Windows 操作系统
- .NET 8.0 SDK
- Node.js 18+
- pnpm 包管理器
- 现代浏览器（Chrome、Edge、Firefox）

### 安装步骤

1. **安装 pnpm**（如果还没有）
   ```bash
   npm install -g pnpm
   ```

2. **安装前端依赖**
   ```bash
   双击：安装依赖-pnpm.bat
   ```
   或者
   ```bash
   双击：🔄迁移到pnpm.bat  # 如果之前使用 npm
   ```

### 启动系统

1. **启动后端**
   ```bash
   双击：start-backend.bat
   ```
   等待看到：`Now listening on: http://localhost:5000`

2. **启动前端**
   ```bash
   双击：start-frontend.bat
   ```
   等待看到：`Local: http://localhost:5173`

3. **访问系统**
   
   在浏览器中打开：http://localhost:5173

## 📜 脚本文件说明

| 脚本文件 | 功能说明 | 使用场景 |
|---------|---------|---------|
| `start-backend.bat` | 启动后端服务 | 每次使用系统时运行 |
| `start-frontend.bat` | 启动前端服务 | 每次使用系统时运行 |
| `test-api.bat` | 测试后端 API 接口 | 验证后端功能是否正常 |
| `安装依赖-pnpm.bat` | 安装前端依赖包 | 首次使用或更新依赖时运行 |
| `🔄迁移到pnpm.bat` | 从 npm 迁移到 pnpm | 清理旧依赖并重新安装 |

## 📊 数据统计

当前数据库包含：
- **总学生数**：266 人
- **男生**：196 人
- **女生**：70 人
- **班级数**：4 个
  - 25计科1班：56人
  - 25计科2班：55人
  - 25计科3班：55人
  - 25软工班：50人

## 🔌 API 接口

| 接口路径 | 方法 | 功能说明 | 参数 |
|---------|------|---------|------|
| `/api/lottery/students` | GET | 获取所有学生列表 | - |
| `/api/lottery/students/{id}` | GET | 获取指定学生信息 | id: 学生ID |
| `/api/lottery/draw` | GET | 随机抽取学生 | gender: 性别（可选）<br>className: 班级（可选） |
| `/api/lottery/stats` | GET | 获取统计信息 | - |

## 📁 项目结构

```
Kiro-ver/
├── lottery-backend/          # C# 后端项目
│   ├── Controllers/          # API 控制器
│   ├── Data/                 # 数据访问层
│   ├── Models/               # 数据模型
│   └── Program.cs            # 程序入口
├── lottery-frontend/         # Vue 前端项目
│   ├── src/                  # 源代码
│   │   ├── App.vue          # 主组件
│   │   ├── main.ts          # 入口文件
│   │   └── style.css        # 样式文件
│   └── package.json          # 依赖配置
├── docs/                     # 文档目录
│   ├── 使用说明.md           # 详细使用指南
│   ├── 项目说明.md           # 技术实现说明
│   ├── 部署指南.md           # 部署文档
│   └── ...                   # 其他文档
├── temp/                     # 临时文件目录
├── students.db              # SQLite 数据库
├── LotterySystem.sln        # Visual Studio 解决方案
├── README.md                # 本文件
└── *.bat                    # 启动脚本
```

## 📚 文档导航

### 快速入门
- [快速开始](docs/快速开始.txt) - 3步快速启动指南
- [🎯开始使用](docs/🎯开始使用.txt) - 最简单的使用指南
- [🎯开始使用-pnpm版](docs/🎯开始使用-pnpm版.txt) - pnpm 版本指南

### 使用指南
- [使用说明](docs/使用说明.md) - 详细的功能使用说明
- [启动检查清单](docs/启动检查清单.md) - 问题排查指南
- [快速操作-Navicat](docs/快速操作-Navicat.txt) - 数据库管理快速参考

### 技术文档
- [项目说明](docs/项目说明.md) - 技术实现和架构说明
- [部署指南](docs/部署指南.md) - 生产环境部署说明
- [如何用Visual Studio打开](docs/如何用Visual%20Studio打开.txt) - VS 使用指南

### 工具文档
- [📦使用pnpm说明](docs/📦使用pnpm说明.txt) - pnpm 完整使用指南
- [📝使用Navicat管理学生数据](docs/📝使用Navicat管理学生数据.md) - 数据库管理详细教程
- [✅已更新为pnpm](docs/✅已更新为pnpm.txt) - pnpm 更新说明

### 完整索引
- [📖文档索引](docs/📖文档索引.txt) - 所有文档的完整索引

## 🛠️ 开发指南

### 后端开发

使用 Visual Studio 打开：
```bash
双击：LotterySystem.sln
```

或使用命令行：
```bash
cd lottery-backend
dotnet run
```

### 前端开发

使用 VS Code 打开 `lottery-frontend` 文件夹，然后：
```bash
pnpm run dev    # 启动开发服务器
pnpm run build  # 构建生产版本
```

### 数据库管理

使用 Navicat 连接 `students.db` 文件进行数据管理。详细说明请查看：
- [📝使用Navicat管理学生数据](docs/📝使用Navicat管理学生数据.md)

## 🧪 测试

运行 API 测试：
```bash
双击：test-api.bat
```

这会测试所有后端 API 接口，确保功能正常。

## 🎯 使用场景

- 课堂随机提问
- 分组抽签
- 活动抽奖
- 任务分配
- 座位安排

## 💡 特色功能

1. **公平随机** - 使用真随机算法，确保公平性
2. **条件筛选** - 灵活的筛选条件，满足不同需求
3. **实时统计** - 动态显示数据统计信息
4. **流畅动画** - 精心设计的抽签动画效果
5. **易于管理** - 支持通过 Navicat 轻松管理学生数据
6. **开箱即用** - 双击启动脚本即可运行

## 🔒 数据安全

- 数据存储在本地 SQLite 数据库
- 支持数据备份和恢复
- 无需联网，保护隐私

## 📝 更新日志

### v1.1.0 (2025-12-06)
- ✅ 更改为 pnpm 包管理器
- ✅ 添加 Navicat 使用指南
- ✅ 添加 Visual Studio 解决方案
- ✅ 完善文档体系
- ✅ 整理文档目录结构

### v1.0.0 (2025-12-06)
- ✅ 初始版本发布
- ✅ C# 后端 + Vue 前端
- ✅ 完整的抽签功能
- ✅ 基础文档

## 🤝 贡献

欢迎提出问题和改进建议！

## 📄 许可证

MIT License

## 📞 技术支持

遇到问题？查看以下文档：
1. [启动检查清单](docs/启动检查清单.md) - 常见问题排查
2. [📖文档索引](docs/📖文档索引.txt) - 查找相关文档
3. [使用说明](docs/使用说明.md) - 详细使用指南

---

**开发时间**：2025年12月6日  
**版本**：1.1.0  
**开发工具**：Kiro AI Assistant

🎉 **开始使用抽签系统吧！**
