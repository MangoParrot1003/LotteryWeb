using Microsoft.EntityFrameworkCore;
using LotteryBackend.Data;
using LotteryBackend.Services;
using LotteryBackend.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 添加服务到容器
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // 使用 camelCase 命名约定（与前端 JavaScript 一致）
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

// 配置数据库上下文
builder.Services.AddDbContext<StudentContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 注册仓储
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IDrawHistoryRepository, DrawHistoryRepository>();
builder.Services.AddScoped<IGroupingHistoryRepository, GroupingHistoryRepository>();
builder.Services.AddScoped<IPrizeDrawHistoryRepository, PrizeDrawHistoryRepository>();
builder.Services.AddScoped<IMembershipRepository, MembershipRepository>();

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
        // 允许所有来源（适用于内网或测试环境）
        // 生产环境建议配置具体的域名
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// 确保数据库和表已创建
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<StudentContext>();
    context.Database.EnsureCreated();
}

// 配置 HTTP 请求管道
// 启用 Swagger（开发和生产环境都可用）
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "学生抽签系统 API v1");
    options.RoutePrefix = "swagger"; // 设置 Swagger UI 为 /swagger 路径
});

app.UseCors("AllowVue");
app.UseAuthorization();
app.MapControllers();

app.Run();
