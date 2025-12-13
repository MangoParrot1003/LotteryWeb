# 学生抽签系统 - API 接口文档

## 📋 基本信息

- **API 版本**：v1.1
- **Base URL**：`http://localhost:5000/api/lottery`
- **Content-Type**：`application/json`
- **字符编码**：UTF-8
- **Swagger 文档**：`http://localhost:5000` (开发环境)
- **最后更新**：2025-12-13

## 🔌 接口列表

### 1. 获取所有学生列表

获取数据库中所有学生的信息。

**接口地址**
```
GET /api/lottery/students
```

**请求参数**

无

**响应示例**
```json
[
  {
    "id": 1,
    "serialNumber": 1,
    "studentId": "202412001",
    "name": "张三",
    "gender": "男",
    "major": "计算机科学与技术",
    "class": "25计科1班"
  },
  {
    "id": 2,
    "serialNumber": 2,
    "studentId": "202412002",
    "name": "李四",
    "gender": "女",
    "major": "软件工程",
    "class": "25软工班"
  }
]
```

**响应字段说明**

| 字段 | 类型 | 说明 |
|------|------|------|
| id | number | 学生ID（主键） |
| serialNumber | number \| null | 序号 |
| studentId | string | 学号 |
| name | string | 姓名 |
| gender | string \| null | 性别 |
| major | string \| null | 专业 |
| class | string \| null | 班级 |

**状态码**

| 状态码 | 说明 |
|--------|------|
| 200 | 成功 |
| 500 | 服务器错误 |

---

### 2. 获取单个学生信息

根据学生ID获取指定学生的详细信息。

**接口地址**
```
GET /api/lottery/students/{id}
```

**路径参数**

| 参数 | 类型 | 必填 | 说明 |
|------|------|------|------|
| id | number | 是 | 学生ID |

**请求示例**
```
GET /api/lottery/students/1
```

**响应示例**
```json
{
  "id": 1,
  "serialNumber": 1,
  "studentId": "202412001",
  "name": "张三",
  "gender": "男",
  "major": "计算机科学与技术",
  "class": "25计科1班"
}
```

**错误响应**
```json
{
  "message": "学生不存在"
}
```

**状态码**

| 状态码 | 说明 |
|--------|------|
| 200 | 成功 |
| 404 | 学生不存在 |
| 500 | 服务器错误 |

---

### 3. 随机抽取学生

从学生库中随机抽取一名学生，支持按性别和班级筛选。

**接口地址**
```
GET /api/lottery/draw
```

**查询参数**

| 参数 | 类型 | 必填 | 说明 |
|------|------|------|------|
| gender | string | 否 | 性别筛选（男/女） |
| className | string | 否 | 班级筛选 |

**请求示例**

```
# 无筛选条件
GET /api/lottery/draw

# 只筛选性别
GET /api/lottery/draw?gender=男

# 只筛选班级
GET /api/lottery/draw?className=25计科1班

# 同时筛选性别和班级
GET /api/lottery/draw?gender=女&className=25计科2班
```

**响应示例**
```json
{
  "id": 15,
  "serialNumber": 15,
  "studentId": "202412015",
  "name": "王五",
  "gender": "男",
  "major": "计算机科学与技术",
  "class": "25计科1班"
}
```

**错误响应**
```json
{
  "message": "没有符合条件的学生"
}
```

**状态码**

| 状态码 | 说明 |
|--------|------|
| 200 | 成功 |
| 404 | 没有符合条件的学生 |
| 500 | 服务器错误 |

**注意事项**
- 抽签结果是真随机的，每次调用可能返回不同的学生
- 如果筛选条件过于严格导致没有符合条件的学生，会返回 404

---

### 4. 获取统计信息

获取学生的统计信息，包括总数、性别分布、班级分布。

**接口地址**
```
GET /api/lottery/stats
```

**请求参数**

无

**响应示例**
```json
{
  "total": 266,
  "genderStats": [
    {
      "gender": "男",
      "count": 196
    },
    {
      "gender": "女",
      "count": 70
    }
  ],
  "classStats": [
    {
      "class": "25计科1班",
      "count": 56
    },
    {
      "class": "25计科2班",
      "count": 55
    },
    {
      "class": "25计科3班",
      "count": 55
    },
    {
      "class": "25软工班",
      "count": 50
    }
  ]
}
```

**响应字段说明**

| 字段 | 类型 | 说明 |
|------|------|------|
| total | number | 学生总数 |
| genderStats | array | 性别统计数组 |
| genderStats[].gender | string | 性别 |
| genderStats[].count | number | 该性别的学生数量 |
| classStats | array | 班级统计数组 |
| classStats[].class | string | 班级名称 |
| classStats[].count | number | 该班级的学生数量 |

**状态码**

| 状态码 | 说明 |
|--------|------|
| 200 | 成功 |
| 500 | 服务器错误 |

---

### 5. 获取班级列表

获取所有不重复的班级列表，用于前端筛选下拉框。

**接口地址**
```
GET /api/lottery/classes
```

**请求参数**

无

**响应示例**
```json
[
  "25计科1班",
  "25计科2班",
  "25计科3班",
  "25软工班"
]
```

**状态码**

| 状态码 | 说明 |
|--------|------|
| 200 | 成功 |
| 500 | 服务器错误 |

---

### 6. 创建会员记录

创建用户的会员记录，支持单次导出和包月会员两种类型。

**接口地址**
```
POST /api/lottery/membership
```

**请求体**
```json
{
  "userId": "user_123",
  "membershipType": "single",
  "orderNo": "ORDER_20251213_001",
  "amount": 1.00,
  "remainingCount": 1,
  "expiryDate": null
}
```

**请求字段说明**

| 字段 | 类型 | 必填 | 说明 |
|------|------|------|------|
| userId | string | 是 | 用户ID |
| membershipType | string | 是 | 会员类型（single/monthly） |
| orderNo | string | 是 | 订单号 |
| amount | number | 是 | 金额 |
| remainingCount | number | 是 | 剩余次数（single为1，monthly为无限） |
| expiryDate | datetime \| null | 否 | 到期日期（monthly必填） |

**响应示例**
```json
{
  "id": 1,
  "userId": "user_123",
  "membershipType": "single",
  "orderNo": "ORDER_20251213_001",
  "amount": 1.00,
  "remainingCount": 1,
  "expiryDate": null,
  "status": "active",
  "createdAt": "2025-12-13T10:30:00"
}
```

**状态码**

| 状态码 | 说明 |
|--------|------|
| 200 | 成功 |
| 400 | 请求参数错误 |
| 500 | 服务器错误 |

---

### 7. 查询会员状态

查询指定用户的会员状态和剩余权限。

**接口地址**
```
GET /api/lottery/membership/check/{userId}
```

**路径参数**

| 参数 | 类型 | 必填 | 说明 |
|------|------|------|------|
| userId | string | 是 | 用户ID |

**请求示例**
```
GET /api/lottery/membership/check/user_123
```

**响应示例**
```json
{
  "id": 1,
  "userId": "user_123",
  "membershipType": "monthly",
  "orderNo": "ORDER_20251213_002",
  "amount": 9.90,
  "remainingCount": null,
  "expiryDate": "2026-01-13T23:59:59",
  "status": "active",
  "isValid": true,
  "daysRemaining": 31
}
```

**响应字段说明**

| 字段 | 类型 | 说明 |
|------|------|------|
| id | number | 会员记录ID |
| userId | string | 用户ID |
| membershipType | string | 会员类型 |
| orderNo | string | 订单号 |
| amount | number | 金额 |
| remainingCount | number \| null | 剩余次数 |
| expiryDate | datetime \| null | 到期日期 |
| status | string | 状态（active/expired） |
| isValid | boolean | 是否有效 |
| daysRemaining | number \| null | 剩余天数 |

**错误响应**
```json
{
  "message": "用户无有效会员"
}
```

**状态码**

| 状态码 | 说明 |
|--------|------|
| 200 | 成功 |
| 404 | 用户无有效会员 |
| 500 | 服务器错误 |

---

### 8. 导出数据为 CSV

导出指定类型的数据为 CSV 格式文件，需要有效的会员权限。

**接口地址**
```
POST /api/lottery/export/excel
```

**请求体**
```json
{
  "userId": "user_123",
  "exportType": "draw_history"
}
```

**请求字段说明**

| 字段 | 类型 | 必填 | 说明 |
|------|------|------|------|
| userId | string | 是 | 用户ID |
| exportType | string | 是 | 导出类型（draw_history/prize_history/students） |

**导出类型说明**

| 类型 | 说明 | 包含字段 |
|------|------|----------|
| draw_history | 抽签历史 | 学号、姓名、性别、班级、抽签时间 |
| prize_history | 抽奖记录 | 奖项名称、中奖者、抽奖时间 |
| students | 学生名单 | 学号、姓名、性别、专业、班级 |

**响应示例**

返回 CSV 文件内容（二进制）

```
学号,姓名,性别,班级,抽签时间
202412001,张三,男,25计科1班,2025-12-13 10:30:00
202412002,李四,女,25计科2班,2025-12-13 10:31:00
```

**错误响应**
```json
{
  "message": "用户无有效会员或导出次数已用尽"
}
```

**状态码**

| 状态码 | 说明 |
|--------|------|
| 200 | 成功 |
| 403 | 无权限或次数已用尽 |
| 400 | 请求参数错误 |
| 500 | 服务器错误 |

**注意事项**
- 单次导出会自动扣减剩余次数
- 包月会员无限制导出
- 导出成功后自动下载 CSV 文件

---

### 9. 单项抽奖

执行单项抽奖操作。

**接口地址**
```
POST /api/lottery/prize-draw
```

**请求体**
```json
{
  "prizeName": "一等奖",
  "count": 1
}
```

**响应示例**
```json
{
  "id": 1,
  "prizeName": "一等奖",
  "winnersList": "[{\"id\":1,\"name\":\"张三\",\"studentId\":\"202412001\"}]",
  "drawTime": "2025-12-13T10:30:00"
}
```

**状态码**

| 状态码 | 说明 |
|--------|------|
| 200 | 成功 |
| 400 | 请求参数错误 |
| 500 | 服务器错误 |

---

### 10. 批量抽奖

执行多个奖项的批量抽奖操作。

**接口地址**
```
POST /api/lottery/prize-draw-batch
```

**请求体**
```json
{
  "prizes": [
    {
      "prizeName": "一等奖",
      "count": 1
    },
    {
      "prizeName": "二等奖",
      "count": 2
    }
  ]
}
```

**响应示例**
```json
[
  {
    "id": 1,
    "prizeName": "一等奖",
    "winnersList": "[{\"id\":1,\"name\":\"张三\",\"studentId\":\"202412001\"}]",
    "drawTime": "2025-12-13T10:30:00"
  },
  {
    "id": 2,
    "prizeName": "二等奖",
    "winnersList": "[{\"id\":2,\"name\":\"李四\",\"studentId\":\"202412002\"},{\"id\":3,\"name\":\"王五\",\"studentId\":\"202412003\"}]",
    "drawTime": "2025-12-13T10:30:01"
  }
]
```

**状态码**

| 状态码 | 说明 |
|--------|------|
| 200 | 成功 |
| 400 | 请求参数错误 |
| 500 | 服务器错误 |

---

### 11. 获取抽奖历史

获取所有抽奖历史记录。

**接口地址**
```
GET /api/lottery/prize-history
```

**响应示例**
```json
[
  {
    "id": 1,
    "prizeName": "一等奖",
    "winnersList": "[{\"id\":1,\"name\":\"张三\",\"studentId\":\"202412001\"}]",
    "drawTime": "2025-12-13T10:30:00"
  }
]
```

**状态码**

| 状态码 | 说明 |
|--------|------|
| 200 | 成功 |
| 500 | 服务器错误 |

---

### 12. 删除抽奖历史

删除指定的抽奖历史记录。

**接口地址**
```
DELETE /api/lottery/prize-history/{id}
```

**路径参数**

| 参数 | 类型 | 必填 | 说明 |
|------|------|------|------|
| id | number | 是 | 抽奖历史ID |

**响应示例**
```json
{
  "message": "删除成功"
}
```

**状态码**

| 状态码 | 说明 |
|--------|------|
| 200 | 成功 |
| 404 | 记录不存在 |
| 500 | 服务器错误 |

---

### 13. 清空抽奖历史

清空所有抽奖历史记录。

**接口地址**
```
DELETE /api/lottery/prize-history
```

**响应示例**
```json
{
  "message": "清空成功"
}
```

**状态码**

| 状态码 | 说明 |
|--------|------|
| 200 | 成功 |
| 500 | 服务器错误 |

---

## 🔧 使用示例

### JavaScript/TypeScript

```typescript
// 获取所有学生
async function getAllStudents() {
  const response = await fetch('http://localhost:5000/api/lottery/students');
  const students = await response.json();
  return students;
}

// 随机抽签
async function drawStudent(gender?: string, className?: string) {
  const params = new URLSearchParams();
  if (gender) params.append('gender', gender);
  if (className) params.append('className', className);
  
  const url = `http://localhost:5000/api/lottery/draw${params.toString() ? '?' + params.toString() : ''}`;
  const response = await fetch(url);
  
  if (!response.ok) {
    throw new Error('抽签失败');
  }
  
  return response.json();
}

// 获取统计信息
async function getStatistics() {
  const response = await fetch('http://localhost:5000/api/lottery/stats');
  return response.json();
}

// 创建会员记录
async function createMembership(userId: string, membershipType: string, orderNo: string, amount: number) {
  const response = await fetch('http://localhost:5000/api/lottery/membership', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      userId,
      membershipType,
      orderNo,
      amount,
      remainingCount: membershipType === 'single' ? 1 : null,
      expiryDate: membershipType === 'monthly' ? new Date(Date.now() + 30 * 24 * 60 * 60 * 1000) : null
    })
  });
  
  return response.json();
}

// 查询会员状态
async function checkMembership(userId: string) {
  const response = await fetch(`http://localhost:5000/api/lottery/membership/check/${userId}`);
  
  if (!response.ok) {
    throw new Error('用户无有效会员');
  }
  
  return response.json();
}

// 导出数据
async function exportData(userId: string, exportType: string) {
  const response = await fetch('http://localhost:5000/api/lottery/export/excel', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      userId,
      exportType
    })
  });
  
  if (!response.ok) {
    throw new Error('导出失败');
  }
  
  // 下载文件
  const blob = await response.blob();
  const url = window.URL.createObjectURL(blob);
  const a = document.createElement('a');
  a.href = url;
  a.download = `export_${exportType}_${Date.now()}.csv`;
  a.click();
}

// 执行单项抽奖
async function performPrizeDraw(prizeName: string, count: number) {
  const response = await fetch('http://localhost:5000/api/lottery/prize-draw', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      prizeName,
      count
    })
  });
  
  return response.json();
}
```

### cURL

```bash
# 获取所有学生
curl http://localhost:5000/api/lottery/students

# 获取单个学生
curl http://localhost:5000/api/lottery/students/1

# 随机抽签（无筛选）
curl http://localhost:5000/api/lottery/draw

# 随机抽签（性别筛选）
curl "http://localhost:5000/api/lottery/draw?gender=男"

# 随机抽签（班级筛选）
curl "http://localhost:5000/api/lottery/draw?className=25计科1班"

# 获取统计信息
curl http://localhost:5000/api/lottery/stats

# 获取班级列表
curl http://localhost:5000/api/lottery/classes
```

### PowerShell

```powershell
# 获取所有学生
Invoke-RestMethod -Uri "http://localhost:5000/api/lottery/students" -Method Get

# 随机抽签
Invoke-RestMethod -Uri "http://localhost:5000/api/lottery/draw?gender=男" -Method Get

# 获取统计信息
Invoke-RestMethod -Uri "http://localhost:5000/api/lottery/stats" -Method Get
```

## 🔒 错误处理

### 错误响应格式

```json
{
  "message": "错误描述信息"
}
```

### 常见错误

| 状态码 | 错误信息 | 原因 | 解决方案 |
|--------|----------|------|----------|
| 404 | 学生不存在 | 指定的学生ID不存在 | 检查学生ID是否正确 |
| 404 | 没有符合条件的学生 | 筛选条件过于严格 | 放宽筛选条件 |
| 500 | 服务器内部错误 | 后端异常 | 查看后端日志 |

## 📝 注意事项

1. **CORS 配置**
   - 开发环境允许 `http://localhost:5173` 和 `http://localhost:5174` 跨域访问
   - 生产环境需要配置实际的前端域名

2. **性能考虑**
   - 获取所有学生接口可能返回大量数据，建议前端做分页处理
   - 统计信息接口会实时计算，数据量大时可能较慢

3. **数据一致性**
   - 所有接口都是实时查询数据库
   - 修改数据库后立即生效，无需重启服务

4. **随机性**
   - 抽签使用 SQLite 的 `RANDOM()` 函数
   - 保证真随机性和公平性

## 🧪 测试工具

### Swagger UI

访问 `http://localhost:5000` 可以看到 Swagger UI 界面，可以直接在浏览器中测试所有 API 接口。

### 测试脚本

项目提供了 `test-api.bat` 脚本，可以快速测试所有接口：

```bash
# 运行测试脚本
./test-api.bat
```

## 📞 技术支持

如遇 API 问题，请：
1. 检查后端服务是否正常运行
2. 查看后端控制台日志
3. 使用 Swagger UI 测试接口
4. 检查 CORS 配置

---

**文档版本**：v1.1  
**最后更新**：2025-12-13
