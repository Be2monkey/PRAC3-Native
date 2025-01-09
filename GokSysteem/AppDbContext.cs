using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContextDatabase
{
    internal class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                    "server=localhost;" +
                    "port=3306;" +
                    "user=root;" +
                    "password=;" +
                    "database=prac3",
                    ServerVersion.Parse("8.0.30")
                    );
        }
    }
}
