@echo off
chcp 65001 >nul
echo ========================================
echo 🚀 完整打包系统（Linux 部署）
echo ========================================
echo.

:: 设置变量
set OUTPUT_DIR=deploy-linux
set BACKEND_DIR=%OUTPUT_DIR%\backend
set FRONTEND_DIR=%OUTPUT_DIR%\frontend

:: 清理旧的部署文件
echo 🧹 清理旧的部署文件...
if exist "%OUTPUT_DIR%" (
    rmdir /s /q "%OUTPUT_DIR%"
)
mkdir "%OUTPUT_DIR%"
mkdir "%BACKEND_DIR%"
mkdir "%FRONTEND_DIR%"
echo ✅ 清理完成
echo.

:: 打包后端
echo ========================================
echo 📦 第 1 步: 打包后端
echo ========================================
echo.

echo 正在编译后端...
dotnet publish lottery-backend -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -o "%BACKEND_DIR%"

if %ERRORLEVEL% NEQ 0 (
    echo ❌ 后端打包失败！
    pause
    exit /b 1
)
echo ✅ 后端打包完成
echo.

:: 复制数据库
echo 📋 复制数据库文件...
copy students.db "%BACKEND_DIR%\students.db" >nul
echo 📋 复制生产环境配置...
if exist lottery-backend\appsettings.Production.json (
    copy lottery-backend\appsettings.Production.json "%BACKEND_DIR%\appsettings.Production.json" >nul
)
echo ✅ 数据库和配置文件复制完成
echo.

:: 打包前端
echo ========================================
echo 📦 第 2 步: 打包前端
echo ========================================
echo.

echo 正在构建前端...
cd lottery-frontend
call pnpm run build

if %ERRORLEVEL% NEQ 0 (
    echo ❌ 前端打包失败！
    cd ..
    pause
    exit /b 1
)

:: 复制前端构建文件
echo 复制前端文件...
cd ..
xcopy /E /I /Y lottery-frontend\dist "%FRONTEND_DIR%" >nul
echo ✅ 前端打包完成
echo.

:: 创建启动脚本
echo ========================================
echo 📝 第 3 步: 创建部署脚本
echo ========================================
echo.

:: 创建后端启动脚本
echo #!/bin/bash > "%BACKEND_DIR%\start.sh"
echo # 启动后端服务 >> "%BACKEND_DIR%\start.sh"
echo echo "启动抽签系统后端..." >> "%BACKEND_DIR%\start.sh"
echo ./lottery-backend >> "%BACKEND_DIR%\start.sh"

:: 创建 Nginx 配置示例
(
echo # Nginx 配置示例
echo # 将此配置添加到 /etc/nginx/sites-available/lottery
echo.
echo server {
echo     listen 8502;
echo     server_name _;
echo.
echo     location / {
echo         proxy_pass http://localhost:5000;
echo         proxy_http_version 1.1;
echo         proxy_set_header Upgrade $http_upgrade;
echo         proxy_set_header Connection keep-alive;
echo         proxy_set_header Host $host;
echo         proxy_cache_bypass $http_upgrade;
echo         proxy_set_header X-Real-IP $remote_addr;
echo         proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
echo         proxy_set_header X-Forwarded-Proto $scheme;
echo     }
echo }
echo.
echo server {
echo     listen 8501;
echo     server_name _;
echo     root /var/www/lottery/frontend;
echo     index index.html;
echo.
echo     location / {
echo         try_files $uri $uri/ /index.html;
echo     }
echo }
) > "%OUTPUT_DIR%\nginx.conf"

:: 创建部署说明
(
echo # 抽签系统 Linux 部署指南
echo.
echo ## 📦 部署包内容
echo.
echo - backend/          后端可执行文件
echo - frontend/         前端静态文件
echo - nginx.conf        Nginx 配置示例
echo - README.txt        本文件
echo.
echo ## 🚀 部署步骤
echo.
echo ### 1. 上传文件
echo.
echo 将整个 deploy-linux 文件夹上传到服务器：
echo ```bash
echo scp -r deploy-linux user@server:/home/user/
echo ```
echo.
echo ### 2. 部署后端
echo.
echo ```bash
echo cd /home/user/deploy-linux/backend
echo chmod +x lottery-backend
echo chmod +x start.sh
echo ./start.sh
echo ```
echo.
echo 或使用 systemd 服务：
echo.
echo 创建 /etc/systemd/system/lottery-backend.service：
echo ```ini
echo [Unit]
echo Description=Lottery Backend Service
echo After=network.target
echo.
echo [Service]
echo Type=simple
echo User=www-data
echo WorkingDirectory=/home/user/deploy-linux/backend
echo ExecStart=/home/user/deploy-linux/backend/lottery-backend
echo Restart=always
echo RestartSec=10
echo.
echo [Install]
echo WantedBy=multi-user.target
echo ```
echo.
echo 启动服务：
echo ```bash
echo sudo systemctl daemon-reload
echo sudo systemctl enable lottery-backend
echo sudo systemctl start lottery-backend
echo sudo systemctl status lottery-backend
echo ```
echo.
echo ### 3. 部署前端
echo.
echo 复制前端文件到 Nginx 目录：
echo ```bash
echo sudo mkdir -p /var/www/lottery
echo sudo cp -r /home/user/deploy-linux/frontend /var/www/lottery/
echo sudo chown -R www-data:www-data /var/www/lottery
echo ```
echo.
echo ### 4. 配置 Nginx
echo.
echo 复制配置文件：
echo ```bash
echo sudo cp /home/user/deploy-linux/nginx.conf /etc/nginx/sites-available/lottery
echo sudo ln -s /etc/nginx/sites-available/lottery /etc/nginx/sites-enabled/
echo sudo nginx -t
echo sudo systemctl reload nginx
echo ```
echo.
echo ### 5. 配置防火墙
echo.
echo ```bash
echo sudo ufw allow 8501/tcp
echo sudo ufw allow 8502/tcp
echo sudo ufw reload
echo ```
echo.
echo ## 🌐 访问地址
echo.
echo - 前端: http://服务器IP:8501
echo - 后端 API: http://服务器IP:8502
echo - Swagger 文档: http://服务器IP:8502
echo.
echo ## 🔧 故障排查
echo.
echo ### 检查后端状态
echo ```bash
echo sudo systemctl status lottery-backend
echo sudo journalctl -u lottery-backend -f
echo ```
echo.
echo ### 检查端口占用
echo ```bash
echo sudo netstat -tlnp ^| grep 5000
echo sudo netstat -tlnp ^| grep 8501
echo sudo netstat -tlnp ^| grep 8502
echo ```
echo.
echo ### 检查 Nginx 日志
echo ```bash
echo sudo tail -f /var/log/nginx/error.log
echo sudo tail -f /var/log/nginx/access.log
echo ```
echo.
echo ## 📝 注意事项
echo.
echo 1. 确保服务器已安装 .NET 8.0 运行时（self-contained 模式不需要）
echo 2. 确保 SQLite 数据库文件有正确的读写权限
echo 3. 建议使用 HTTPS（配置 SSL 证书）
echo 4. 定期备份 students.db 数据库文件
echo.
echo ## 🔄 更新部署
echo.
echo 1. 停止服务：sudo systemctl stop lottery-backend
echo 2. 备份数据库：cp students.db students.db.backup
echo 3. 上传新文件
echo 4. 启动服务：sudo systemctl start lottery-backend
echo.
) > "%OUTPUT_DIR%\README.txt"

echo ✅ 部署脚本创建完成
echo.

:: 显示摘要
echo ========================================
echo ✅ 打包完成！
echo ========================================
echo.
echo 📁 输出目录: %OUTPUT_DIR%\
echo.
echo 📦 包含内容:
echo    ├── backend\
echo    │   ├── lottery-backend      (可执行文件)
echo    │   ├── students.db          (数据库)
echo    │   ├── appsettings.Production.json (生产环境配置)
echo    │   └── start.sh             (启动脚本)
echo    ├── frontend\
echo    │   └── (静态文件)
echo    ├── nginx.conf               (Nginx 配置)
echo    └── README.txt               (部署说明)
echo.
echo 📋 下一步:
echo    1. 将 %OUTPUT_DIR% 文件夹上传到 Linux 服务器
echo    2. 阅读 README.txt 了解详细部署步骤
echo    3. 按照说明配置 Nginx 和 systemd 服务
echo.
echo 🌐 部署后访问地址:
echo    前端: http://服务器IP:8501
echo    后端: http://服务器IP:8502
echo.

pause
