# 学生抽签系统 - API 接口文档

## 📋 基本信息

- **API 版本**：v1
- **Base URL**：`http://localhost:5000/api/lottery`
- **Content-Type**：`application/json`
- **字符编码**：UTF-8
- **Swagger 文档**：`http://localhost:5000` (开发环境)

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

**文档版本**：v1.0  
**最后更新**：2025-12-06
