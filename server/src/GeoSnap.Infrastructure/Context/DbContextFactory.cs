using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GeoSnap.Infrastructure.Context;
public static class DbContextFactory
{
    private static readonly IDbContextProvider<ApplicationDbContext> _dbContextProvider = new DbContextProvider<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());

    public static IDbContextProvider<ApplicationDbContext> GetDbContextProvider()
    {
        return _dbContextProvider;
    }
}
