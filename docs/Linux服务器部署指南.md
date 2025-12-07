# Linux 服务器部署指南

## 📦 第一步：打包文件

### 在 Windows 上打包

1. **运行打包脚本**
   ```bash
   双击：build-all-linux.bat
   ```

2. **等待打包完成**
   - 后端编译（约 30 秒）
   - 前端构建（约 10 秒）
   - 生成部署文件

3. **检查输出**
   ```
   deploy-linux/
   ├── backend/
   │   ├── lottery-backend      # 可执行文件
   │   ├── students.db          # 数据库
   │   └── start.sh             # 启动脚本
   ├── frontend/
   │   ├── index.html
   │   └── assets/
   ├── nginx.conf
   └── README.txt
   ```

---

## 🚀 第二步：上传到服务器

### 方法一：使用 SCP（推荐）

```bash
# 压缩文件夹
cd Kiro-ver
tar -czf deploy-linux.tar.gz deploy-linux/

# 上传到服务器
scp deploy-linux.tar.gz user@服务器IP:/home/user/

# 或者直接上传文件夹
scp -r deploy-linux user@服务器IP:/home/user/
```

### 方法二：使用 FTP 工具

使用 FileZilla、WinSCP 等工具：
1. 连接到服务器
2. 上传 `deploy-linux` 文件夹
3. 确保所有文件完整上传

### 方法三：使用 Git

```bash
# 在服务器上
git clone 你的仓库地址
cd 项目目录/Kiro-ver
# 运行打包脚本或直接使用已打包的文件
```

---

## 🔧 第三步：在服务器上部署

### 1. 连接到服务器

```bash
ssh user@服务器IP
```

### 2. 解压文件（如果压缩了）

```bash
cd /home/user
tar -xzf deploy-linux.tar.gz
cd deploy-linux
```

### 3. 给予执行权限

```bash
chmod +x backend/lottery-backend
chmod +x backend/start.sh
```

### 4. 测试运行后端

```bash
cd backend
./lottery-backend
```

**看到以下输出表示成功**：
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://0.0.0.0:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

按 `Ctrl+C` 停止测试。

---

## 🌐 第四步：配置 Nginx

### 1. 安装 Nginx（如果未安装）

```bash
# Ubuntu/Debian
sudo apt update
sudo apt install nginx

# CentOS/RHEL
sudo yum install nginx
```

### 2. 复制前端文件

```bash
sudo mkdir -p /var/www/lottery
sudo cp -r /home/user/deploy-linux/frontend /var/www/lottery/
sudo chown -R www-data:www-data /var/www/lottery
```

### 3. 配置 Nginx

```bash
# 复制配置文件
sudo cp /home/user/deploy-linux/nginx.conf /etc/nginx/sites-available/lottery

# 创建软链接
sudo ln -s /etc/nginx/sites-available/lottery /etc/nginx/sites-enabled/

# 测试配置
sudo nginx -t

# 重新加载 Nginx
sudo systemctl reload nginx
```

---

## 🔄 第五步：配置 Systemd 服务（开机自启）

### 1. 创建服务文件

```bash
sudo nano /etc/systemd/system/lottery-backend.service
```

### 2. 添加以下内容

```ini
[Unit]
Description=Lottery Backend Service
After=network.target

[Service]
Type=simple
User=www-data
WorkingDirectory=/home/user/deploy-linux/backend
ExecStart=/home/user/deploy-linux/backend/lottery-backend
Restart=always
RestartSec=10
Environment="ASPNETCORE_ENVIRONMENT=Production"

[Install]
WantedBy=multi-user.target
```

### 3. 启动服务

```bash
# 重新加载 systemd
sudo systemctl daemon-reload

# 启动服务
sudo systemctl start lottery-backend

# 查看状态
sudo systemctl status lottery-backend

# 设置开机自启
sudo systemctl enable lottery-backend
```

---

## 🔥 第六步：配置防火墙

### Ubuntu/Debian (UFW)

```bash
# 允许端口
sudo ufw allow 8501/tcp
sudo ufw allow 8502/tcp

# 重新加载
sudo ufw reload

# 查看状态
sudo ufw status
```

### CentOS/RHEL (Firewalld)

```bash
# 允许端口
sudo firewall-cmd --permanent --add-port=8501/tcp
sudo firewall-cmd --permanent --add-port=8502/tcp

# 重新加载
sudo firewall-cmd --reload

# 查看状态
sudo firewall-cmd --list-all
```

---

## ✅ 第七步：验证部署

### 1. 检查后端状态

```bash
# 查看服务状态
sudo systemctl status lottery-backend

# 查看日志
sudo journalctl -u lottery-backend -f

# 检查端口
sudo netstat -tlnp | grep 5000
```

### 2. 检查 Nginx 状态

```bash
# 查看 Nginx 状态
sudo systemctl status nginx

# 查看日志
sudo tail -f /var/log/nginx/error.log
sudo tail -f /var/log/nginx/access.log
```

### 3. 测试访问

```bash
# 测试后端 API
curl http://localhost:5000/api/lottery/stats

# 测试前端（在浏览器中）
http://服务器IP:8501

# 测试后端（在浏览器中）
http://服务器IP:8502
```

---

## 🎯 访问地址

部署成功后，可以通过以下地址访问：

| 服务 | 地址 | 说明 |
|------|------|------|
| 前端页面 | http://服务器IP:8501 | 抽签系统界面 |
| 后端 API | http://服务器IP:8502 | API 接口 |
| Swagger 文档 | http://服务器IP:8502 | API 文档 |

---

## 🔧 常用管理命令

### 服务管理

```bash
# 启动服务
sudo systemctl start lottery-backend

# 停止服务
sudo systemctl stop lottery-backend

# 重启服务
sudo systemctl restart lottery-backend

# 查看状态
sudo systemctl status lottery-backend

# 查看日志
sudo journalctl -u lottery-backend -f
```

### Nginx 管理

```bash
# 启动 Nginx
sudo systemctl start nginx

# 停止 Nginx
sudo systemctl stop nginx

# 重启 Nginx
sudo systemctl restart nginx

# 重新加载配置
sudo systemctl reload nginx

# 测试配置
sudo nginx -t
```

---

## 🔄 更新部署

### 1. 停止服务

```bash
sudo systemctl stop lottery-backend
```

### 2. 备份数据库

```bash
cp /home/user/deploy-linux/backend/students.db /home/user/students.db.backup
```

### 3. 上传新文件

```bash
# 在本地打包新版本
# 上传到服务器
scp -r deploy-linux user@服务器IP:/home/user/deploy-linux-new

# 在服务器上替换
cd /home/user
rm -rf deploy-linux-old
mv deploy-linux deploy-linux-old
mv deploy-linux-new deploy-linux

# 恢复数据库
cp students.db.backup deploy-linux/backend/students.db
```

### 4. 重新启动

```bash
sudo systemctl start lottery-backend
sudo systemctl reload nginx
```

---

## 🐛 故障排查

### 问题 1：缺少依赖文件（FileNotFoundException）

**错误信息**：
```
Could not load file or assembly 'Microsoft.AspNetCore.OpenApi'
```

**原因**：
打包时使用了 `-p:PublishTrimmed=true` 导致必要的依赖被裁剪。

**解决方案**：
1. 重新打包（已修复的脚本不会出现此问题）
2. 或者使用非裁剪模式：
```bash
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true
```

### 问题 2：后端无法启动

**检查日志**：
```bash
sudo journalctl -u lottery-backend -n 50
```

**常见原因**：
- 端口被占用：`sudo netstat -tlnp | grep 5000`
- 权限问题：`sudo chown -R www-data:www-data /home/user/deploy-linux`
- 数据库文件缺失：检查 `students.db` 是否存在

### 问题 2：前端无法访问

**检查 Nginx**：
```bash
sudo nginx -t
sudo systemctl status nginx
sudo tail -f /var/log/nginx/error.log
```

**常见原因**：
- Nginx 配置错误
- 前端文件路径不正确
- 防火墙阻止端口

### 问题 3：API 调用失败

**检查 CORS**：
- 确保后端 CORS 配置正确
- 检查前端 API 地址是否正确

**测试 API**：
```bash
curl http://localhost:5000/api/lottery/stats
```

### 问题 4：数据库错误

**检查权限**：
```bash
ls -la /home/user/deploy-linux/backend/students.db
sudo chown www-data:www-data /home/user/deploy-linux/backend/students.db
sudo chmod 644 /home/user/deploy-linux/backend/students.db
```

---

## 📝 注意事项

1. **安全性**
   - 建议使用 HTTPS（配置 SSL 证书）
   - 配置防火墙只允许必要的端口
   - 定期更新系统和软件包

2. **性能优化**
   - 使用 Nginx 缓存静态文件
   - 配置 Gzip 压缩
   - 监控服务器资源使用

3. **数据备份**
   - 定期备份 `students.db` 数据库
   - 使用 cron 定时任务自动备份
   ```bash
   # 添加到 crontab
   0 2 * * * cp /home/user/deploy-linux/backend/students.db /home/user/backup/students-$(date +\%Y\%m\%d).db
   ```

4. **日志管理**
   - 定期清理日志文件
   - 配置日志轮转
   ```bash
   sudo nano /etc/logrotate.d/lottery
   ```

---

## 🎉 完成！

现在你的抽签系统已经成功部署到 Linux 服务器上了！

**快速访问**：
- 前端：http://你的服务器IP:8501
- 后端：http://你的服务器IP:8502

如有问题，请查看故障排查部分或查看日志文件。
