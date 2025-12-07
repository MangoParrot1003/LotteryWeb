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

### 端口说明

系统使用以下端口：

| 服务         | 端口 | 说明                  | 访问地址              |
| ------------ | ---- | --------------------- | --------------------- |
| 后端 API     | 5000 | ASP.NET Core 后端服务 | http://localhost:5000 |
| 前端开发     | 8501 | Vite 开发服务器       | http://localhost:8501 |
| Swagger 文档 | 5000 | API 接口文档          | http://localhost:5000 |

**注意事项**：

- 确保端口 5000 和 8501 未被占用
- 如需局域网访问，使用 `http://你的IP:端口号`
- 防火墙需要允许这两个端口的入站连接

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

   **本地开发环境（Windows）**：

   ```bash
   双击：start-backend.bat
   ```

   等待看到：`Now listening on: http://localhost:5000`

   **服务器环境（宝塔面板）**：

   - 打开宝塔终端：https://8.148.202.248:19030/xterm
   - 查看占用端口的进程：`sudo lsof -t -i:5000`
   - 杀掉旧进程：`kill -9 进程ID`（例如：`kill -9 9630`）
   - 进入部署目录：`cd /home/ying_ying/web_test/release/`
   - 启动后端：`./lottery-backend`
2. **启动前端**

   ```bash
   双击：start-frontend.bat
   ```

   等待看到：`Local: http://localhost:5173`
3. **访问系统**

   在浏览器中打开：http://localhost:5173

## 📜 脚本文件说明

| 脚本文件               | 功能说明           | 使用场景                 |
| ---------------------- | ------------------ | ------------------------ |
| `start-backend.bat`  | 启动后端服务       | 每次使用系统时运行       |
| `start-frontend.bat` | 启动前端服务       | 每次使用系统时运行       |
| `test-api.bat`       | 测试后端 API 接口  | 验证后端功能是否正常     |
| `安装依赖-pnpm.bat`  | 安装前端依赖包     | 首次使用或更新依赖时运行 |
| `🔄迁移到pnpm.bat`   | 从 npm 迁移到 pnpm | 清理旧依赖并重新安装     |

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

| 接口路径                       | 方法 | 功能说明         | 参数                                                |
| ------------------------------ | ---- | ---------------- | --------------------------------------------------- |
| `/api/lottery/students`      | GET  | 获取所有学生列表 | -                                                   |
| `/api/lottery/students/{id}` | GET  | 获取指定学生信息 | id: 学生ID                                          |
| `/api/lottery/draw`          | GET  | 随机抽取学生     | gender: 性别（可选）`<br>`className: 班级（可选） |
| `/api/lottery/stats`         | GET  | 获取统计信息     | -                                                   |

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

### 更新部署到服务器

若要用新代码覆盖旧有代码，请在宝塔面板上用 Total Commander 复制前后端文件进行覆盖：

1. **打包新版本**：

   ```bash
   双击：build-all-linux.bat
   ```
2. **上传到服务器**：

   - 使用宝塔面板的文件管理器
   - 或使用 Total Commander 的 SFTP 功能
   - 上传 `deploy-linux` 文件夹
3. **覆盖文件**：

   - 后端：覆盖 `/home/ying_ying/web_test/release/` 目录
   - 前端：覆盖 `/var/www/lottery/frontend/` 目录
4. **重启服务**：

   - 杀掉旧的后端进程：`kill -9 进程ID`
   - 启动新的后端：`./lottery-backend`
   - 重新加载 Nginx：`sudo systemctl reload nginx`

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

## 🌐 服务器部署说明

### 本地开发环境

**默认配置**：

- 后端监听：`http://localhost:5000`
- 前端访问：`http://localhost:8501`

### 局域网访问

**后端配置**：
后端已配置为监听所有网络接口（`0.0.0.0:5000`），支持局域网访问。

**前端配置**：
前端会自动检测访问地址，无需手动配置：

- 本机访问：`http://localhost:8501` → API: `http://localhost:5000`
- 局域网访问：`http://192.168.1.100:8501` → API: `http://192.168.1.100:5000`

**使用步骤**：

1. 启动后端和前端服务
2. 运行 `get-ip.bat` 获取本机 IP 地址
3. 在其他设备上访问 `http://你的IP:8501`

### 服务器部署

**通过宝塔面板管理前端界面**：

1. 登录宝塔面板
2. 点击左侧菜单 **网站**
3. 找到对应的网站项目
4. 点击 **设置** 按钮
5. 在设置页面找到 **端口** 选项卡
6. 在此处可以管理和修改网站端口

![1765091245902](image/README/1765091245902.png)

**端口映射**：
如果部署到云服务器，需要配置以下端口映射：

| 内部端口 | 外部端口 | 协议 | 说明          |
| -------- | -------- | ---- | ------------- |
| 5000     | 8502     | HTTP | 后端 API 服务 |
| 5173     | 8501     | HTTP | 前端页面服务  |

**示例配置**（Nginx 反向代理）：

```nginx
# 后端 API
server {
    listen 8502;
    location / {
        proxy_pass http://localhost:5000;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
    }
}

# 前端页面
server {
    listen 8501;
    location / {
        proxy_pass http://localhost:5173;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
    }
}
```

**防火墙配置**：

```powershell
# 允许后端端口
New-NetFirewallRule -DisplayName "抽签系统-后端" -Direction Inbound -LocalPort 5000,8502 -Protocol TCP -Action Allow

# 允许前端端口
New-NetFirewallRule -DisplayName "抽签系统-前端" -Direction Inbound -LocalPort 5173,8501 -Protocol TCP -Action Allow
```

**访问地址**：

- 前端：`http://服务器IP:8501`
- 后端 API：`http://服务器IP:8502`
- Swagger 文档：`http://服务器IP:8502`

## 📝 更新日志

### v0.2.1 (2025-12-06)

- ✨ 新增抽签历史记录功能
- ✨ 新增批量抽签功能（1-10人）
- ✨ 新增防重复抽签机制
- ✨ 批量抽签依次展示效果
- ✨ 最新抽中学生高亮动画
- 🏗️ 实现仓储模式架构
- 🏗️ 集成 Entity Framework Core
- 📚 新增服务器部署说明
- 📚 新增端口配置文档

### v0.2.0 (2025-12-06)

- 🎉 后端重构使用 Dapper ORM
- 🎉 添加 Swagger API 文档
- 🎉 前端组件化重构
- 🎉 支持局域网访问
- 🎉 真正的前后端串联
- 📚 完善技术文档体系

### v0.1.0 (2025-12-06)

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
**当前版本**：v0.2.1
**开发工具**：Kiro AI Assistant

🎉 **开始使用抽签系统吧！**

## 🔗 相关链接

- [项目开发复盘与技术总结](docs/项目开发复盘与技术总结.md) - 完整的开发历程和技术决策
- [CHANGELOG](CHANGELOG.md) - 详细的版本更新记录
- [局域网使用指南](docs/局域网使用指南.md) - 局域网配置详解
- [测试和部署指南](docs/测试和部署指南.md) - 生产环境部署
