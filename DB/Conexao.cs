using Microsoft.Extensions.Configuration;

namespace DeliverIt.DB
{
    public class Conexao
    {
        private readonly IConfiguration configuration;
        public Conexao(IConfiguration config)
        {
            this.configuration = config;
        }

        public string ConnString()
        {
            string sqlConn = configuration.GetConnectionString("ConnString");

            return sqlConn;
        }

    }
}
