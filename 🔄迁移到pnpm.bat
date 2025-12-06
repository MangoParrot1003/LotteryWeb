@echo off
echo ========================================
echo 从 npm 迁移到 pnpm
echo ========================================
echo.

cd lottery-frontend

echo [1/4] 删除旧的 node_modules...
if exist node_modules (
    rmdir /s /q node_modules
    echo 已删除 node_modules
) else (
    echo node_modules 不存在，跳过
)

echo.
echo [2/4] 删除 package-lock.json...
if exist package-lock.json (
    del package-lock.json
    echo 已删除 package-lock.json
) else (
    echo package-lock.json 不存在，跳过
)

echo.
echo [3/4] 检查 pnpm 是否已安装...
pnpm --version >nul 2>&1
if errorlevel 1 (
    echo.
    echo ❌ 错误：pnpm 未安装
    echo.
    echo 请先安装 pnpm：
    echo npm install -g pnpm
    echo.
    pause
    exit /b 1
)
echo ✅ pnpm 已安装

echo.
echo [4/4] 使用 pnpm 安装依赖...
pnpm install

echo.
echo ========================================
echo ✅ 迁移完成！
echo ========================================
echo.
echo 现在可以使用 start-frontend.bat 启动前端了
echo.
pause
