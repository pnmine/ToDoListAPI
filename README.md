# Start Project

`dotnet new webapi --use-controllers -o  ToDoListAPI`

# Nuget install
- Microsoft.EntityFrameworkCore 
- Microsoft.EntityFrameworkCore.Tools
- Pomelo.EntityFrameworkCore.MySql

# Create Model in Models Directory

# Create DbContext
> Create Diractory : Data
>>  New File : DataContext   (class)
```c#
namespace ToDoListAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        // เพิ่ม model ที่ต้องการ migrations
        public DbSet<TodoList> TodoLists { get; set; } // ชื่อตาราง todolists
        public DbSet<Category> Categories { get; set; } // ชื่อตาราง categories
    }
}
```
# connect database 
> เปิด mysql database และสร้าง ด้วย ,ตอนนี้ใช้ XAMPP เปิด mySQL ,Apache เพราะง่ายดี
## in appsettings.json
```c#
"ConnectionStrings": {
    "DefaultConnection": "server=localhost;database=ชื่อdataBase;user=root;password=;"
  },
```
## in Program.cs
```c#
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(option => option.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString)));
```

# Create Migrations 
`dotnet ef migrations add CreateTable`
จะได้ folder Migrations มา
>XXXXXXXXXXXXXX_CreateTableTodo.cs — จะเก็บรายละเอียด migration ที่เราเพิ่ม หรือลดจากการ migrate ในครั้งนี้

>XXXXXXXXXXXXXX_CreateTableTodo.Designer.cs — เป็น metadata migrations file ที่เราเพิ่มไป

> Snapshot.cs —คือ snapshot ของ current model ทั้งหมดของเราที่มีอยู่ เวลาที่เราเพิ่ม migrate เข้าไปมันจะไปกับ migrate ที่เราเพื่งเพิ่มมา

# Use Migration in Database
`dotnet ef database update`
---