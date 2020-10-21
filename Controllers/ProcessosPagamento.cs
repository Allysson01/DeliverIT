using DeliverIt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace DeliverIt.Controllers
{
    [ApiController]
    public class ProcessosPagamento : ControllerBase
    {
        private readonly IConfiguration configuration;

        public ProcessosPagamento(IConfiguration config)
        {
            this.configuration = config;
        }

        #region ListarContas (HttpGet)
        [HttpGet]
        [Route("api/[controller]/listarcontas")]
        public List<object> ListarContas(string filtro)
        {
            List<object> lstContas;
            try
            {
                Contas oContas = new Contas(this.configuration);
                lstContas = oContas.Get(filtro);
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
        #endregion ListarContas (HttpGet)

        #region Cadastrar Contas (HttpPost)
        [HttpPost]
        [Route("api/[controller]/cadastrarcontas")]
        public bool CadastrarContas(Contas contas)
        {
            bool sucesso = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(contas.Nome) && contas.ValorOriginal > 0)
                {

                    TimeSpan tData = (contas.DataPagamento - contas.DataVencimento);
                    int dias = tData.Days;

                    if (dias > 0)
                    {
                        CalcJuros oCalc = new CalcJuros(this.configuration);

                        contas = oCalc.CalculaJuros(contas, dias);
                    }

                    Contas oContas = new Contas(this.configuration);

                    sucesso = oContas.Post(contas);
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

            return sucesso;
        }
        #endregion Cadastrar Contas (HttpPost)
    }
}
