using DeliverIt.DB;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DeliverIt.Models
{
    [Table("contas")]
    public class Contas
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal ValorOriginal { get; set; }
        public decimal ValorCorrigido { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime DataPagamento { get; set; }
        public int QuantDiaAtraso { get; set; }

        private readonly IConfiguration configuration;

        public Contas(IConfiguration config)
        {
            this.configuration = config;
        }

        public Contas() { }

        public bool Post(Contas contas)
        {
            bool sucesso = false;
            try
            {
                using (var db = new DtaBaseContext(configuration))
                {
                    db.Contas.Add(contas);
                    db.SaveChanges();
                }
                sucesso = true;
            }
            catch (SqlException ex)
            {
                throw new Exception("Ocorreu o erro: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu o erro: " + ex.Message);
            }
            return sucesso;
        }

        public List<object> Get(string filtro)
        {
            List<object> lstContas = new List<object>();
            try
            {
                if (string.IsNullOrWhiteSpace(filtro))
                {
                    #region sem filtro
                    using var db = new DtaBaseContext(configuration);
                    IOrderedQueryable list = db.Contas.Select(s => new
                    {
                        id = s.Id,
                        nome = s.Nome,
                        valorOriginal = Math.Round(s.ValorOriginal, 2),
                        valorCorrigido = Math.Round(s.ValorCorrigido, 2),
                        dataVencimento = Convert.ToDateTime(s.DataVencimento).ToString("dd/MM/yyyy"),
                        dataPagamento = Convert.ToDateTime(s.DataPagamento).ToString("dd/MM/yyyy"),
                        quantDiaAtraso = s.QuantDiaAtraso

                    }).OrderBy(o => o.nome);

                    foreach (var item in list)
                    {
                        lstContas.Add(item);
                    }
                    #endregion
                }
                else
                {
                    #region com filtro
                    using var db = new DtaBaseContext(configuration);
                    IOrderedQueryable list = db.Contas.Where(x => x.Nome.ToLower().Contains(filtro.ToLower())
                                                             || x.Id.ToString().Contains(filtro))
                        .Select(s => new
                        {
                            id = s.Id,
                            nome = s.Nome,
                            valorOriginal = Math.Round(s.ValorOriginal, 2),
                            valorCorrigido = Math.Round(s.ValorCorrigido, 2),
                            dataVencimento = Convert.ToDateTime(s.DataVencimento).ToString("dd/MM/yyyy"),
                            dataPagamento = Convert.ToDateTime(s.DataPagamento).ToString("dd/MM/yyyy"),
                            quantDiaAtraso = s.QuantDiaAtraso

                        }).OrderBy(o => o.nome);

                    foreach (var item in list)
                    {
                        lstContas.Add(item);
                    }
                    #endregion
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Ocorreu o erro: " + ex.Message);
            }
            catch (Exception ex)
            {

                throw new Exception("Ocorreu o erro: " + ex.Message);
            }


            return lstContas;
        }
    }
}
