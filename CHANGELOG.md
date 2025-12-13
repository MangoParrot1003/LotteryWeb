# 更新日志

## [v1.1.0] - 2025-12-11

### 🎉 新增功能

#### 付费抽奖与会员系统
- 💎 **会员套餐**
  - 单次导出：¥1（1次数据导出权限）
  - 包月会员：¥9.9/月（无限次导出，30天有效期）
  - 移除包年套餐，采用发展中国家友好定价

- 📊 **数据导出功能**
  - 支持导出抽签历史（CSV格式）
  - 支持导出抽奖记录（CSV格式）
  - 支持导出学生名单（CSV格式）
  - 自动权限验证和次数扣减
  - 单次导出自动计数，包月会员无限制

- 🔐 **会员管理**
  - 支付成功自动创建会员记录
  - 会员状态实时验证（localStorage + API）
  - 会员到期时间自动计算
  - 导出次数自动管理

### 🔧 后端实现

#### 数据模型
- 📦 **MembershipRecord** - 会员记录模型
  - 支持单次和包月两种类型
  - 自动有效性检查
  - 剩余次数管理

#### API接口
- 🔌 **POST /api/lottery/membership** - 创建会员
- 🔌 **GET /api/lottery/membership/check/{userId}** - 查询会员状态
- 🔌 **POST /api/lottery/export/excel** - 导出数据（CSV格式）

#### 数据库
- 💾 **membership_records** - 新增会员记录表
  - 包含会员类型、订单号、金额、剩余次数等字段
  - 索引优化：user_id、order_no、status

#### 仓储层
- 📁 **IMembershipRepository** - 会员仓储接口
- 📁 **MembershipRepository** - 会员仓储实现
  - 创建会员、查询状态、扣减次数等方法

### 🎨 前端实现

#### 新增组件
- 💳 **ExportPanel.vue** - 数据导出面板
  - 权限检查和状态展示
  - 三种导出选项
  - 自动下载CSV文件
  - 剩余次数/到期时间显示

#### 功能增强
- 💰 **PaidPrizeDraw.vue** - 付费抽奖页面优化
  - 调整价格为¥1和¥9.9
  - 集成导出面板组件
  - 优化价格卡片排版

- 💳 **PaymentModal.vue** - 支付流程增强
  - 支付成功自动调用会员创建API
  - 会员信息保存到localStorage
  - 异常容错处理

### 📦 部署更新
- ✅ 更新 `publish/` 后端部署文件（Linux x64版本）
- ✅ 更新 `dist/` 前端构建文件
- ✅ 包含会员数据库迁移脚本
- ✅ 包含最新的students.db数据库

### 🎯 功能验证
- ✅ 支付流程完整可用
- ✅ 会员创建和查询正常
- ✅ 数据导出功能正常
- ✅ 权限验证准确
- ✅ 单次/包月会员逻辑正确

### 📝 文件清单

**新增文件：**
- `lottery-backend/Models/MembershipRecord.cs`
- `lottery-backend/Repositories/IMembershipRepository.cs`
- `lottery-backend/Repositories/MembershipRepository.cs`
- `lottery-backend/Data/create_membership_table.sql`
- `lottery-frontend/src/components/ExportPanel.vue`

**修改文件：**
- `lottery-backend/Controllers/LotteryController.cs` - 新增会员和导出API
- `lottery-backend/Data/StudentContext.cs` - 添加会员表配置
- `lottery-backend/Program.cs` - 注册会员服务
- `lottery-frontend/src/components/PaidPrizeDraw.vue` - 价格调整和导出面板
- `lottery-frontend/src/components/PaymentModal.vue` - 支付成功创建会员

---

## [v1.0.4] - 2025-12-10

### 🐛 问题修复

#### 抽奖历史显示修复
- 🔧 **修复抽奖历史中奖者姓名显示问题**
  - 后端返回的 JSON 字段名改为 camelCase（小写开头）
  - 前端优先使用 `winnersList`（已解析的数组）
  - 添加 `getWinners()` 函数处理多种数据格式
  - 完全兼容旧数据格式

#### 后端配置优化
- 🔌 **JSON 序列化配置**
  - 添加 `PropertyNamingPolicy.CamelCase` 配置
  - 确保所有 API 返回的字段名与前端一致
  - 提高前后端数据交互的兼容性

### 📦 部署更新
- ✅ 重新编译后端（包含 JSON 配置修复）
- ✅ 重新构建前端（包含历史显示修复）
- ✅ 更新 `publish/` 后端部署文件
- ✅ 更新 `dist/` 前端构建文件

### 🎯 功能验证
- ✅ 抽奖历史正确显示学生姓名
- ✅ 多奖项抽奖功能完整可用
- ✅ 所有 API 接口正常工作
- ✅ 数据库查询正常

---

## [v0.2.3] - 2025-12-09

### 🎁 新增功能

#### 多奖项抽奖系统
- 🏆 **多奖项同时抽取**
  - 新增"多奖项抽奖"模式标签页
  - 支持配置多个奖项（奖项名称 + 中奖人数）
  - 依次抽取每个奖项，带动画效果
  - 自动防止重复中奖
  - 支持动态添加/删除奖项配置

- 🎨 **金色主题界面**
  - 优雅的金色配色方案
  - 简洁白色背景 + 淡金色边框
  - 适度的渐变和动画效果
  - 最新中奖者高亮显示（发光、放大效果）

- 📊 **抽奖结果展示**
  - 奖项配置卡片式布局
  - 中奖结果分组展示
  - 实时抽奖动画效果
  - 响应式设计支持移动端

#### 抽奖历史修复
- 🐛 **修复历史记录显示问题**
  - 支持字符串（JSON）和数组两种数据格式
  - 添加 `parseWinners()` 函数处理不同格式
  - 添加容错处理，避免解析错误

### 🔧 技术实现

#### 后端支持
- 🔌 **批量抽奖 API**
  - `POST /api/lottery/prize-draw-batch` - 批量抽奖
  - `POST /api/lottery/prize-draw` - 单项抽奖
  - `GET /api/lottery/prize-history` - 获取抽奖历史
  - `DELETE /api/lottery/prize-history` - 清空抽奖历史
  - `DELETE /api/lottery/prize-history/{id}` - 删除单条记录

- 💾 **数据模型**
  - `PrizeDrawHistory` - 抽奖历史模型
  - `IPrizeDrawHistoryRepository` - 抽奖历史仓储接口
  - `PrizeDrawHistoryRepository` - 抽奖历史仓储实现

#### 前端组件
- 📦 **新增组件**
  - `MultiPrizeDraw.vue` - 多奖项抽奖组件
  - `PrizeDrawBox.vue` - 单项抽奖组件
  - `PrizeHistoryPanel.vue` - 抽奖历史面板

- 🔄 **状态管理**
  - 奖项配置管理
  - 抽奖结果管理
  - 历史记录管理
  - 动画状态控制

### 🎨 界面优化

#### 金色主题设计
- **配色方案**
  - 主金色：`#f59e0b` (橙金色)
  - 深金色：`#d97706` (深橙金)
  - 浅金色：`#fef3c7` (浅黄金)
  - 中金色：`#fde68a` (中黄金)

- **视觉特点**
  - 简洁白色背景
  - 淡金色边框点缀
  - 简化的渐变按钮
  - 适度的动画效果
  - 正常粗细字体

#### 动画效果
- ✨ **抽奖动画**
  - 按钮脉冲呼吸效果
  - 卡片发光动画
  - 中奖者高亮效果
  - 悬停交互优化

### 🐛 问题修复
- 修复抽奖历史中奖者数据解析问题
- 修复 Swagger 路径配置（改为 `/swagger`）
- 修复生产环境 CORS 配置
- 优化 Nginx 反向代理配置

### 📦 部署更新
- ✅ 更新 `publish/` 后端部署文件
- ✅ 更新 `dist/` 前端构建文件
- ✅ 包含最新数据库文件
- ✅ 生产环境配置优化

### 📝 配置说明
- **后端端口**：5000（内部）→ 8502（Nginx）
- **前端端口**：8501
- **Swagger 文档**：`http://服务器IP:8502/swagger`
- **API 接口**：`http://服务器IP:8502/api/...`

---

## [v0.1.3] - 2025-12-08

### 🐛 问题修复

#### 随机抽签功能修复
- 🔧 **修复抽签按钮不显示问题**
  - 修复 `App.vue` 缺少 `LotteryBox` 组件导入
  - 修复后随机抽签按钮和历史记录面板正常显示

### ✨ 新增功能

#### 分组历史持久化
- 📋 **独立的分组历史记录**
  - 新增 `grouping_history` 数据库表（与 `draw_history` 分开存储）
  - 分组结果自动保存到数据库
  - 刷新页面后分组历史不丢失

- 🎨 **分组历史面板**
  - 新增 `GroupingHistoryPanel.vue` 组件
  - 按批次显示分组历史
  - 显示每组成员名单
  - 支持删除单个批次或清空全部

- 🔄 **模式切换历史面板**
  - 随机抽签模式 → 显示抽签历史
  - 全班分组模式 → 显示分组历史

#### 界面优化
- 🎯 **分组成员横向排列**
  - 组内成员从竖向改为横向显示
  - 每个成员卡片使用头像+姓名的紧凑布局
  - 添加悬停动画效果

### 🔧 技术改进

#### 后端新增
- 📦 `GroupingHistory.cs` - 分组历史数据模型
- 📦 `IGroupingHistoryRepository.cs` - 分组历史仓储接口
- 📦 `GroupingHistoryRepository.cs` - 分组历史仓储实现
- 🔌 新增 4 个分组历史 API：
  - `POST /api/lottery/grouping` - 保存分组结果
  - `GET /api/lottery/grouping-history` - 获取分组历史
  - `DELETE /api/lottery/grouping-history` - 清空分组历史
  - `DELETE /api/lottery/grouping-history/{batchId}` - 删除指定批次

#### 前端新增
- 📦 `GroupingHistoryPanel.vue` - 分组历史显示组件
- 🔄 更新 `useLottery.ts` 添加分组历史状态和方法
- 🌐 更新 `lottery.ts` API 添加分组历史接口调用
- 📝 更新 `student.ts` 添加 `GroupingHistory` 类型

---

## [v0.2.2] - 2025-12-07

### 📚 文档更新
- 📝 **README 优化**
  - 新增宝塔面板端口管理详细步骤
  - 完善服务器部署说明
  - 添加端口管理操作指引
  - 包含宝塔面板操作截图说明

### 🔧 改进
- 优化文档结构和可读性
- 补充服务器运维相关说明

---

## [v0.2.1] - 2025-12-06

### ✨ 新增功能

#### 抽签历史记录系统
- 📜 **历史记录持久化**
  - 所有抽签结果自动保存到数据库
  - 支持刷新页面后恢复历史记录
  - 历史记录面板实时显示
  
- 🎯 **防重复抽签**
  - "排除已抽取"选项
  - 勾选后不会重复抽到历史中的学生
  - 实时更新可抽取人数

- 📊 **批量抽签**
  - 支持一次抽取 1/2/3/5/10 人
  - 批量结果网格卡片展示
  - 批次自动分组和标记
  - 带序号和入场动画

#### 历史记录管理
- 🗑️ **清空历史** - 一键清空所有记录
- ❌ **删除单条** - 移除指定历史记录
- 🏷️ **批次标识** - 批量抽签显示"批量"徽章
- ⏰ **时间显示** - 智能时间格式化
  - 刚刚（1分钟内）
  - X分钟前（1小时内）
  - X小时前（24小时内）
  - 今天 HH:mm:ss
  - MM-DD HH:mm:ss
  - YYYY-MM-DD HH:mm:ss

#### 界面优化
- 🎨 **三栏布局**
  - 左侧：统计 + 筛选（320px）
  - 中间：抽签区域（自适应）
  - 右侧：历史记录（350px）
  
- 🎭 **视觉增强**
  - 批量抽签记录蓝色左边框
  - 批次之间分隔线
  - 时间显示在右上角
  - 响应式设计优化

### 🔧 技术改进

#### 后端架构重构
- 🏗️ **仓储模式（Repository Pattern）**
  - `IRepository<T>` - 通用仓储接口
  - `Repository<T>` - 通用仓储实现
  - `IStudentRepository` / `StudentRepository` - 学生仓储
  - `IDrawHistoryRepository` / `DrawHistoryRepository` - 历史记录仓储

- 💾 **数据库扩展**
  - 新增 `draw_history` 表
  - 字段：学生信息、抽签时间、会话ID、批次ID
  - 索引优化：student_id、draw_time、session_id、batch_id

- 🔌 **新增 API 接口**
  - `GET /api/lottery/history` - 获取历史记录
  - `DELETE /api/lottery/history` - 清空历史记录
  - `DELETE /api/lottery/history/{id}` - 删除单条记录
  - `GET /api/lottery/draw-multiple` - 批量抽签

- 🎯 **依赖注入**
  - 注册 Entity Framework Core
  - 注册仓储服务
  - 使用 SQLite 数据库

#### 前端功能增强
- 📦 **新增组件**
  - `HistoryPanel.vue` - 历史记录面板
  
- 🔄 **状态管理**
  - `drawHistory` - 历史记录列表
  - `excludeDrawn` - 排除已抽取开关
  - `drawCount` - 批量抽签数量
  - `selectedStudents` - 批量抽签结果

- 🌐 **API 集成**
  - `getDrawHistory()` - 获取历史
  - `clearDrawHistory()` - 清空历史
  - `deleteDrawHistory()` - 删除记录
  - `drawMultipleStudents()` - 批量抽签

- 📝 **类型定义**
  - `DrawHistory` 接口
  - 完整的类型安全

### 🐛 问题修复
- 修复局域网访问 API 失败问题
- 修复 API 地址硬编码导致的跨设备访问问题
- 优化时间格式解析（处理微秒）

### 📚 文档更新
- 新增《局域网访问问题修复.md》
- 详细的故障排查指南
- 完整的使用说明

---

## [v0.2.0] - 2025-12-06

### 🎉 重大更新

#### 后端重构
- 使用 Dapper ORM 替代原生 SQL
- 添加 Swagger API 文档自动生成
- 性能提升 30-50%
- 内存占用减少 33%

#### 前端重构
- 完全组件化架构
- 使用 TypeScript 类型系统
- Composition API 和组合式函数
- 真正的前后端串联通信

#### 局域网支持
- 支持多设备同时访问
- 自动适配本机和局域网地址
- 完善的 CORS 配置

#### 文档完善
- 系统设计文档
- API 接口文档
- 局域网使用指南
- CORS 问题解决方案
- 测试和部署指南

---

## [v0.1.0] - 2025-12-06

### 🎊 初始发布
- 基础抽签功能
- 学生信息管理
- 性别和班级筛选
- 统计信息展示
- SQLite 数据库
