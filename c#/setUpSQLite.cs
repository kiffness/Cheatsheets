How to configure sql lite
-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Install the following nuget packages
EntityFramwork.Core
EntitiyFramwork.Core.Sqlite
EntitiyFramwork.Design

Create your entity first
-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

//AppUser.cs

public class AppUser
{
		public int Id { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
}

Then you need to set up your DataContext class
-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
//DataContext.cs

public class DataContext : DbContext
{
		public DataContext(DbContextOptions options) : base(options)
		{
		}
		
		public Dbset<AppUser> Users { get; set; }
}

Then add your connection string to your appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data source=datingapp.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}

Then you need to configure your services in program.cs
-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

//program.cs

builder.services.AddDbContext<DataContext>(options =>
{
	options.UseSqlite(config.GetConnectionString("DefaultConnection"));
}
