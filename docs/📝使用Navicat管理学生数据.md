# 📝 使用 Navicat 管理学生数据

## 🎯 概述

使用 Navicat 可以方便地查看、添加、修改、删除学生信息。

## 📊 数据库信息

- **数据库类型**：SQLite
- **数据库文件**：`Kiro-ver/students.db`
- **表名**：`students`
- **当前记录数**：266 条

## 🔌 连接数据库

### 步骤 1：打开 Navicat
1. 启动 Navicat Premium 或 Navicat for SQLite
2. 点击左上角"连接"按钮

### 步骤 2：新建 SQLite 连接
1. 选择"SQLite"连接类型
2. 填写连接信息：
   - **连接名**：学生抽签系统（自定义）
   - **数据库文件**：点击"..."浏览选择
     ```
     D:\Users\YingYing\Desktop\output\抽签程序\Kiro-ver\students.db
     ```
3. 点击"测试连接"
4. 点击"确定"保存

### 步骤 3：打开数据库
1. 双击刚创建的连接
2. 展开"main"
3. 展开"表"
4. 双击"students"表

## 📋 表结构说明

| 字段名 | 类型 | 说明 | 是否必填 |
|--------|------|------|----------|
| id | INTEGER | 主键（自动递增） | 自动生成 |
| serial_number | INTEGER | 序号 | 可选 |
| student_id | TEXT | 学号 | 必填 |
| name | TEXT | 姓名 | 必填 |
| gender | TEXT | 性别（男/女） | 可选 |
| major | TEXT | 专业 | 可选 |
| class | TEXT | 班级 | 可选 |

## ➕ 添加学生

### 方法一：使用表格界面（推荐）
1. 打开 students 表
2. 点击底部的"+"按钮（添加记录）
3. 填写学生信息：
   ```
   id: （留空，自动生成）
   serial_number: 267
   student_id: 202412345
   name: 张三
   gender: 男
   major: 计算机科学与技术
   class: 25计科1班
   ```
4. 按 Enter 或点击"√"保存

### 方法二：使用 SQL 语句
1. 点击"查询"→"新建查询"
2. 输入 SQL：
   ```sql
   INSERT INTO students (serial_number, student_id, name, gender, major, class)
   VALUES (267, '202412345', '张三', '男', '计算机科学与技术', '25计科1班');
   ```
3. 点击"运行"（F5）

### 批量添加多个学生
```sql
INSERT INTO students (serial_number, student_id, name, gender, major, class)
VALUES 
(267, '202412345', '张三', '男', '计算机科学与技术', '25计科1班'),
(268, '202412346', '李四', '女', '软件工程', '25软工班'),
(269, '202412347', '王五', '男', '计算机科学与技术', '25计科2班');
```

## ✏️ 修改学生信息

### 方法一：直接编辑（推荐）
1. 打开 students 表
2. 找到要修改的学生记录
3. 双击单元格直接编辑
4. 修改完成后按 Enter 保存

### 方法二：使用 SQL 语句
```sql
-- 修改学生姓名
UPDATE students 
SET name = '张三丰' 
WHERE student_id = '202412345';

-- 修改学生班级
UPDATE students 
SET class = '25计科3班' 
WHERE id = 267;

-- 修改多个字段
UPDATE students 
SET name = '李四光', gender = '男', class = '25计科1班'
WHERE student_id = '202412346';
```

## 🗑️ 删除学生

### 方法一：使用表格界面（推荐）
1. 打开 students 表
2. 选中要删除的记录（点击行号）
3. 点击底部的"-"按钮（删除记录）
4. 确认删除

### 方法二：使用 SQL 语句
```sql
-- 根据 ID 删除
DELETE FROM students WHERE id = 267;

-- 根据学号删除
DELETE FROM students WHERE student_id = '202412345';

-- 删除多个学生
DELETE FROM students WHERE id IN (267, 268, 269);
```

⚠️ **警告**：删除操作不可恢复，请谨慎操作！

## 🔍 查询学生

### 常用查询示例

```sql
-- 查询所有学生
SELECT * FROM students;

-- 查询特定班级的学生
SELECT * FROM students WHERE class = '25计科1班';

-- 查询男生
SELECT * FROM students WHERE gender = '男';

-- 查询姓名包含"张"的学生
SELECT * FROM students WHERE name LIKE '%张%';

-- 统计各班级人数
SELECT class, COUNT(*) as 人数 
FROM students 
GROUP BY class;

-- 统计性别分布
SELECT gender, COUNT(*) as 人数 
FROM students 
GROUP BY gender;

-- 查询最新添加的 10 个学生
SELECT * FROM students 
ORDER BY id DESC 
LIMIT 10;
```

## 📤 导出数据

### 导出为 Excel
1. 右键点击 students 表
2. 选择"导出向导"
3. 选择"Excel 文件"
4. 选择保存位置
5. 点击"开始"

### 导出为 CSV
1. 右键点击 students 表
2. 选择"导出向导"
3. 选择"CSV 文件"
4. 配置分隔符（建议使用逗号）
5. 点击"开始"

### 导出为 SQL
1. 右键点击 students 表
2. 选择"转储 SQL 文件"
3. 选择"结构和数据"
4. 选择保存位置

## 📥 导入数据

### 从 Excel 导入
1. 右键点击 students 表
2. 选择"导入向导"
3. 选择"Excel 文件"
4. 选择要导入的文件
5. 映射字段（确保字段对应正确）
6. 点击"开始"

### 从 CSV 导入
1. 右键点击 students 表
2. 选择"导入向导"
3. 选择"CSV 文件"
4. 配置分隔符
5. 映射字段
6. 点击"开始"

## 🔄 数据备份

### 备份整个数据库
1. 右键点击连接名
2. 选择"备份数据库"
3. 选择保存位置
4. 输入备份文件名（如：students_backup_20251206.db）
5. 点击"开始"

### 快速备份（推荐）
直接复制 `students.db` 文件：
```
复制：Kiro-ver\students.db
粘贴到：Kiro-ver\backup\students_20251206.db
```

## 🔧 实用技巧

### 1. 快速筛选
- 点击列标题右侧的筛选图标
- 输入筛选条件
- 支持多列同时筛选

### 2. 排序
- 点击列标题进行排序
- 再次点击切换升序/降序

### 3. 查找替换
- Ctrl + F：查找
- Ctrl + H：替换
- 支持正则表达式

### 4. 批量编辑
- 选中多行
- 右键选择"编辑"
- 可以批量修改相同字段

### 5. 数据验证
```sql
-- 检查是否有重复学号
SELECT student_id, COUNT(*) as 数量
FROM students
GROUP BY student_id
HAVING COUNT(*) > 1;

-- 检查是否有空姓名
SELECT * FROM students WHERE name IS NULL OR name = '';

-- 检查性别是否规范
SELECT DISTINCT gender FROM students;
```

## ⚠️ 注意事项

### 1. 数据库文件位置
确保操作的是正确的数据库文件：
```
Kiro-ver\students.db  ← 系统使用的数据库
```

### 2. 修改后刷新
修改数据后，如果系统正在运行：
- 后端会自动读取最新数据
- 前端刷新页面即可看到更新

### 3. 备份建议
- 修改前先备份数据库
- 定期备份（每周或每月）
- 保留多个版本的备份

### 4. ID 字段
- `id` 字段是主键，自动递增
- 添加记录时不要手动填写 id
- 删除记录后，id 不会重新分配

### 5. 数据类型
- `student_id`、`name` 等是文本类型，需要用引号
- `serial_number`、`id` 是数字类型，不需要引号

## 📝 常见操作示例

### 示例 1：添加新班级的学生
```sql
INSERT INTO students (serial_number, student_id, name, gender, major, class)
VALUES 
(267, '202512001', '赵六', '男', '数据科学', '25数科1班'),
(268, '202512002', '孙七', '女', '数据科学', '25数科1班'),
(269, '202512003', '周八', '男', '数据科学', '25数科1班');
```

### 示例 2：批量修改班级名称
```sql
UPDATE students 
SET class = '2025级计科1班' 
WHERE class = '25计科1班';
```

### 示例 3：删除某个班级的所有学生
```sql
-- 先查看要删除的记录
SELECT * FROM students WHERE class = '25计科3班';

-- 确认无误后删除
DELETE FROM students WHERE class = '25计科3班';
```

### 示例 4：更正学生信息
```sql
-- 更正学号
UPDATE students 
SET student_id = '202412999' 
WHERE name = '张三' AND class = '25计科1班';
```

## 🎯 最佳实践

1. **修改前备份**
   - 每次大批量修改前先备份数据库

2. **使用事务**（高级）
   ```sql
   BEGIN TRANSACTION;
   -- 执行多个操作
   UPDATE students SET class = '新班级' WHERE id = 1;
   DELETE FROM students WHERE id = 2;
   -- 确认无误后提交
   COMMIT;
   -- 如果有问题可以回滚
   -- ROLLBACK;
   ```

3. **验证数据**
   - 修改后检查总数是否正确
   - 检查是否有异常数据

4. **保持一致性**
   - 班级名称格式统一
   - 性别只用"男"或"女"
   - 专业名称规范

## 📞 获取帮助

如果遇到问题：
1. 检查数据库文件路径是否正确
2. 确保 Navicat 有读写权限
3. 查看 Navicat 的错误提示
4. 恢复备份文件

---

**提示**：修改数据库后，系统会自动读取最新数据，无需重启后端！
