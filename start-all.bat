@echo off
chcp 65001 >nul
echo ========================================
echo 🚀 正在启动抽签系统 (前后端)
echo ========================================
echo.

:: 启动后端
start "Kiro抽签后端" cmd /k "call start-backend.bat"

:: 等待几秒让后端先初始化
timeout /t 3 /nobreak >nul

:: 启动前端
start "Kiro抽签前端" cmd /k "call start-frontend.bat"

echo ✅ 服务已启动!
echo.
echo - 后端运行在: http://localhost:5000 (Swagger: /swagger)
echo - 前端运行在: http://localhost:8501
echo.
echo 请不要关闭弹出的命令行窗口。
echo.
pause
