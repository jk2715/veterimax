using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Entity;

namespace Veterimax.Models
{
    public class VeterimaxDbContext:DbContext
    {
        public DbSet<Suplidor> Suplidores { get; set; }
    }
}
