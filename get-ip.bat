@echo off
chcp 65001 >nul
echo ========================================
echo 🌐 本机网络信息
echo ========================================
echo.

echo 📍 本机 IP 地址：
ipconfig | findstr "IPv4" | findstr "192.168\|10\.\|172."
echo.

echo ========================================
echo 🚀 访问地址
echo ========================================
echo.
echo 请将下面的 "你的IP" 替换为上面显示的实际 IP 地址
echo.
echo 🔗 前端页面：
echo   - 本机访问: http://localhost:8501
echo   - 局域网访问: http://你的IP:8501
echo.
echo 🔗 后端 API：
echo   - 本机访问: http://localhost:5000
echo   - 局域网访问: http://你的IP:5000
echo.
echo 🔗 Swagger 文档：
echo   - 本机访问: http://localhost:5000
echo   - 局域网访问: http://你的IP:5000
echo.

echo ========================================
echo 💡 使用提示
echo ========================================
echo.
echo 1. 确保防火墙允许 5000 和 8501 端口
echo 2. 确保所有设备连接到同一局域网
echo 3. 使用 start-frontend.bat 启动前端
echo 4. 移动设备可直接在浏览器中访问
echo.
echo ========================================

pause
