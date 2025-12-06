using Microsoft.EntityFrameworkCore;
using LotteryBackend.Data;
using LotteryBackend.Services;
using LotteryBackend.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 添加服务到容器
builder.Services.AddControllers();

// 配置数据库上下文
builder.Services.AddDbContext<StudentContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 注册仓储
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IDrawHistoryRepository, DrawHistoryRepository>();

// 注册服务
builder.Services.AddScoped<IStudentService, StudentService>();

// 配置 Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "学生抽签系统 API",
        Version = "v1",
        Description = "学生抽签系统的后端 API 接口文档"
    });
    
    // 包含 XML 注释
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// 配置 CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVue", policy =>
    {
        // 开发环境：允许所有来源（包括局域网）
        if (builder.Environment.IsDevelopment())
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
        else
        {
            // 生产环境：只允许指定域名
            policy.WithOrigins("https://your-domain.com")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
    });
});

var app = builder.Build();

// 配置 HTTP 请求管道
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "学生抽签系统 API v1");
        options.RoutePrefix = string.Empty; // 设置 Swagger UI 为根路径
    });
}

app.UseCors("AllowVue");
app.UseAuthorization();
app.MapControllers();

app.Run();
