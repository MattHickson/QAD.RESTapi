
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Project.Models;
using POSAPI.Models;

namespace Project.Data
{
    public class MonsterModContext : DbContext
    {
        public MonsterModContext(DbContextOptions<MonsterModContext> options) : base(options) { }
        public DbSet<MonsterMod> MonsterMods { get; set; }

    }
    public class DivinePoolContext : DbContext
    {
        public DivinePoolContext(DbContextOptions<DivinePoolContext> options) : base(options) { }
        public DbSet<DivinePool> DivinePools { get; set; }

    }
}

