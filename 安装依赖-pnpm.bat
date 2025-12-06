@echo off
echo ========================================
echo 使用 pnpm 安装前端依赖
echo ========================================
echo.

cd lottery-frontend

echo 正在安装依赖，请稍候...
pnpm install

echo.
echo ========================================
echo 依赖安装完成！
echo ========================================
echo.
echo 现在可以运行 start-frontend.bat 启动前端了
echo.
pause
