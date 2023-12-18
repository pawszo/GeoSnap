using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace GeoSnap.Infrastructure.Context;
public class DbContextProvider : IDbContextProvider<ApplicationDbContext>
{
    //private PwiDbContext? dbContext;
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
    private readonly IConfiguration _configuration;

    public DbContextProvider(DbContextOptions<ApplicationDbContext> dbContextOptions)
    {
        _dbContextOptions = dbContextOptions;
    }

    public ApplicationDbContext GetDbContext()
    {
        return new ApplicationDbContext(_dbContextOptions);   
    }
}
