@echo off
chcp 65001 >nul
echo ========================================
echo 🐧 打包后端为 Linux 可执行文件
echo ========================================
echo.

cd lottery-backend

echo 📦 正在清理旧的发布文件...
if exist "bin\Release\net8.0\linux-x64\publish" (
    rmdir /s /q "bin\Release\net8.0\linux-x64\publish"
)
echo ✅ 清理完成
echo.

echo 🔨 正在编译和发布...
echo 目标平台: Linux x64
echo 配置: Release
echo 模式: Self-Contained (包含运行时)
echo.

dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ========================================
    echo ✅ 打包成功！
    echo ========================================
    echo.
    echo 📁 输出目录:
    echo    lottery-backend\bin\Release\net8.0\linux-x64\publish\
    echo.
    echo 📦 可执行文件:
    echo    lottery-backend
    echo.
    echo 📋 部署步骤:
    echo    1. 将 publish 文件夹上传到 Linux 服务器
    echo    2. 复制 students.db 到同一目录
    echo    3. 给予执行权限: chmod +x lottery-backend
    echo    4. 运行: ./lottery-backend
    echo.
    echo 🌐 默认监听地址: http://0.0.0.0:5000
    echo.
) else (
    echo.
    echo ========================================
    echo ❌ 打包失败！
    echo ========================================
    echo.
    echo 请检查错误信息并重试
    echo.
)

pause
