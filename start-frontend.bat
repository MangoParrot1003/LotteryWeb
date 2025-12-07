@echo off
chcp 65001 >nul
echo ========================================
echo 启动抽签系统前端
echo ========================================
echo.
echo 启动后可通过以下地址访问：
echo - 本机: http://localhost:8501
echo - 局域网: http://你的IP:8501
echo.
echo 按 Ctrl+C 停止服务
echo.

cd lottery-frontend
pnpm run dev
