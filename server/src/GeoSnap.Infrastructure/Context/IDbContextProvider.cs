using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GeoSnap.Infrastructure.Context;
public interface IDbContextProvider<TDbContext> where TDbContext : DbContext
{
    TDbContext GetDbContext();
}
