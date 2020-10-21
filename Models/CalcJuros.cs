using DeliverIt.DB;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DeliverIt.Models
{
    [Table("calcJuros")]
    public class CalcJuros
    {
        public int Id { get; set; }
        public int DiasAtraso { get; set; }
        public int Multa { get; set; }
        public double JurosDia { get; set; }

        private readonly IConfiguration configuration;

        public CalcJuros(IConfiguration config)
        {
            this.configuration = config;
        }

        public CalcJuros() { }

        public Contas CalculaJuros(Contas contas, int dias)
        {
            int auxDias = 0;
            try
            {
                //switch para pesquisar tabela de juros
                auxDias = dias switch
                {
                    int n when (n > 0 && n <= 3) => 3,
                    int n when (n < 3 && n <= 5) => 4,
                    _ => 6,
                };

                using var db = new DtaBaseContext(configuration);
                var calc = db.CalculoJuros.Where(x => x.DiasAtraso == auxDias).Select(s => new
                {
                    diasAtraso = s.DiasAtraso,
                    multa = s.Multa,
                    juros = Convert.ToDouble(s.JurosDia)
                }).FirstOrDefault();

                decimal valorMulta = Math.Round((contas.ValorOriginal / 100) * calc.multa, 2);

                contas.ValorCorrigido = (contas.ValorOriginal + valorMulta);

                for (int i = 0; i < dias; i++)
                {
                    decimal valorJuros = Math.Round((contas.ValorCorrigido / 100) * (decimal)calc.juros, 2);
                    contas.ValorCorrigido += valorJuros;
                }

                contas.QuantDiaAtraso = dias;
            }
            catch (SqlException ex)
            {
                throw new Exception("Ocorreu o erro: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu o erro: " + ex.Message);
            }
            return contas;
        }
    }
}
