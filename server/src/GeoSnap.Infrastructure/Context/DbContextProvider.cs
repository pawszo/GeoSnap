using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GeoSnap.Infrastructure.Context;
public class DbContextProvider<TDbContext> : IDbContextProvider<TDbContext> where TDbContext : DbContext, new()
{
    private readonly DbContextOptions<TDbContext> _options;

    public DbContextProvider(DbContextOptions<TDbContext> options)
    {
        _options = options;
    }

    public DbContextProvider()
    {
        _options = new DbContextOptions<TDbContext>();
    }

    public TDbContext GetDbContext()
    {
        return new TDbContext();
    }
}
