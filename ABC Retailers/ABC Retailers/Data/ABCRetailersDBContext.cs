using ABC_Retailers.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ABC_Retailers.Data
{
    //https://learn.microsoft.com/en-us/aspnet/web-forms/overview/deployment/visual-studio-web-deployment/deploying-a-database-update
    //https://learn.microsoft.com/en-us/azure/app-service/app-service-web-tutorial-dotnet-sqldatabase
    //DB Context class acts as a link between the SQL Database and the application
    public class ABCRetailersDBContext : IdentityDbContext<IdentityUser>
    {
        public ABCRetailersDBContext(DbContextOptions<ABCRetailersDBContext> options) : base(options)
        {

        }
        //references the customer table
        public DbSet<CustomerTable> CustomersTable { get; set; }
        //references the order table
        public DbSet<OrderModel> OrdersTable { get; set; }
        //references the product table
        public DbSet<ProductTable> ProductsTable { get; set; }

    }
}
