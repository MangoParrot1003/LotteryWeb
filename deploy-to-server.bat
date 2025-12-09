@echo off
chcp 65001 >nul
setlocal EnableDelayedExpansion

echo ========================================
echo 🚀 自动部署到 Linux 服务器 (源目录版)
echo ========================================
echo.

:: 1. 检查是否提供了参数，否则提示输入
set SERVER_IP=
set SERVER_USER=
set REMOTE_PATH=

if not "%~1"=="" set SERVER_IP=%~1
if not "%~2"=="" set SERVER_USER=%~2
if not "%~3"=="" set REMOTE_PATH=%~3

if "%SERVER_IP%"=="" (
    set /p SERVER_IP="请输入服务器 IP 地址: "
)

if "%SERVER_USER%"=="" (
    set /p SERVER_USER="请输入用户名 (默认: root): "
    if "!SERVER_USER!"=="" set SERVER_USER=root
)

if "%REMOTE_PATH%"=="" (
    set /p REMOTE_PATH="请输入远程部署路径 (默认: /home/!SERVER_USER!/lottery): "
    if "!REMOTE_PATH!"=="" set REMOTE_PATH=/home/!SERVER_USER!/lottery
)

echo.
echo 📋 部署信息:
echo 服务器: !SERVER_USER!@!SERVER_IP!
echo 路径: !REMOTE_PATH!
echo.

:: 2. 编译打包
echo 🔨 正在执行编译打包...

:: 2.1 编译后端
echo.
echo [1/3] 编译后端...
cd lottery-backend
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true
if %ERRORLEVEL% NEQ 0 (
    echo ❌ 后端编译失败
    pause
    exit /b 1
)
cd ..

:: 2.2 编译前端
echo.
echo [2/3] 编译前端...
cd lottery-frontend
call pnpm run build
if %ERRORLEVEL% NEQ 0 (
    echo ❌ 前端编译失败
    pause
    exit /b 1
)
cd ..

:: 3. 上传文件
echo.
echo 📤 正在上传文件...
echo 提示: 如果没有配置 SSH 密钥，接下来需要输入服务器密码。
echo.

:: 创建远程目录
ssh -o StrictHostKeyChecking=no !SERVER_USER!@!SERVER_IP! "mkdir -p !REMOTE_PATH!"

:: 上传 backend (从 publish 目录直接上传)
echo 上传后端...
set BACKEND_PUBLISH_DIR=lottery-backend\bin\Release\net8.0\linux-x64\publish
scp -r -o StrictHostKeyChecking=no "%BACKEND_PUBLISH_DIR%\*" !SERVER_USER!@!SERVER_IP!:!REMOTE_PATH!/backend/

:: 手动上传 start.sh 和 appsettings.Production.json (因为它们可能不在 publish 目录里)
echo 上传后端配置脚本...
scp -o StrictHostKeyChecking=no lottery-backend\start.sh !SERVER_USER!@!SERVER_IP!:!REMOTE_PATH!/backend/
scp -o StrictHostKeyChecking=no lottery-backend\appsettings.Production.json !SERVER_USER!@!SERVER_IP!:!REMOTE_PATH!/backend/
scp -o StrictHostKeyChecking=no students.db !SERVER_USER!@!SERVER_IP!:!REMOTE_PATH!/backend/

:: 上传 frontend
echo 上传前端...
scp -r -o StrictHostKeyChecking=no lottery-frontend\dist\* !SERVER_USER!@!SERVER_IP!:!REMOTE_PATH!/frontend/

:: 上传 nginx 配置
echo 上传 Nginx 配置...
scp -o StrictHostKeyChecking=no nginx.conf !SERVER_USER!@!SERVER_IP!:!REMOTE_PATH!/

if %ERRORLEVEL% NEQ 0 (
    echo ❌ 上传失败！
    pause
    exit /b 1
)

:: 4. 设置权限
echo.
echo 🔐 设置执行权限和修复格式...
ssh -o StrictHostKeyChecking=no !SERVER_USER!@!SERVER_IP! "sed -i 's/\r$//' !REMOTE_PATH!/backend/start.sh && chmod +x !REMOTE_PATH!/backend/lottery-backend && chmod +x !REMOTE_PATH!/backend/start.sh"

echo.
echo ========================================
echo ✅ 部署文件上传完成！
echo ========================================
echo.
echo 接下来的步骤 (需在服务器执行):
echo 1. SSH 登录服务器: ssh !SERVER_USER!@!SERVER_IP!
echo 2. 进入目录: cd !REMOTE_PATH!/backend
echo 3. 运行服务: ./start.sh
echo.
pause
