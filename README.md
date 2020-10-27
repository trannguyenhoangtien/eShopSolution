# ASP.NET CORE 3.1 From Hoàng Tiến
## Technologies
- ASP.NET Core 3.1
- Entity framework Core 3.1
## Instal Packages
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.Tools
## Youtube tutorial
- https://youtube.com
## How to configure and run
- Clone code from Github: git clone https://github.com/trannguyenhoangtien/eShopSolution
- Open solution eShopSolution.sln in Visual Studio 2019
- Set startup project is eShopSolution.Data
- Change connection string in Appsetting.json in eShopSolution.Data project
- Open Tools --> Nuget Package Manager --> Package Manager Console in Visual Studio
- Run Update-database and Enter.
- After migrate database successful, set Startup Project is eShopSolution.WebApp
- Change database connection in appsettings.Development.json in eShopSolution.WebApp project.
- Choose profile to run or press F5
## How to contribute
- Fork and create your branch
- Create Pull request to us.
## Admin template: 
- Source: https://startbootstrap.com/templates/sb-admin/
## Portal template: 
- Source: https://www.free-css.com/free-css-templates/page194/bootstrap-shop