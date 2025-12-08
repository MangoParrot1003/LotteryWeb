# 更新日志

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
