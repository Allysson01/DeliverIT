using DeliverIt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DeliverIt.DB
{
    public class DtaBaseContext : DbContext
    {
        private readonly IConfiguration configuration;

        public DtaBaseContext(IConfiguration config)
        {
            this.configuration = config;
        }

        public DbSet<Contas> Contas { get; set; }
        public DbSet<CalcJuros> CalculoJuros { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Conexao oConex = new Conexao(configuration);
            optionsBuilder.UseSqlServer(oConex.ConnString());
        }
    }
}
