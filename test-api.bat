@echo off
chcp 65001 >nul
echo ========================================
echo 测试抽签系统 API
echo ========================================
echo.

echo [1] 测试统计接口...
curl http://localhost:5000/api/lottery/stats
echo.
echo.

echo [2] 测试抽签接口（无筛选）...
curl http://localhost:5000/api/lottery/draw
echo.
echo.

echo [3] 测试抽签接口（性别筛选：女）...
curl "http://localhost:5000/api/lottery/draw?gender=女"
echo.
echo.

echo [4] 测试抽签接口（班级筛选）...
curl "http://localhost:5000/api/lottery/draw?className=25计科1班"
echo.
echo.

echo ========================================
echo 测试完成！
echo ========================================
pause
