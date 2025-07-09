using OpenQA.Selenium;
using PortaldeCompras.Application.Common;
using PortaldeCompras.Domain.Interfaces;
using PortaldeCompras.Domain.Models;
using System.Data;
using System.Text.RegularExpressions;

namespace PortaldeCompras.Infrastructure
{
    public class DataExtractor : IDataExtractor<DataTable>
    {
        private readonly IElementMap _elementMap;

        public DataExtractor(IElementMap elementMap)
        {
            _elementMap = elementMap;
        }

        public DataTable GetData(IWebDriver driver)
        {
            return CreateTable(GetLicitacoes(driver));
        }

        private DataTable CreateTable(List<Licitacao> licitacoes)
        {
            var table = new DataTable();
            table.Columns.Add("Modalidade", typeof(string));
            table.Columns.Add("Numero", typeof(int));
            table.Columns.Add("Ano", typeof(int));

            foreach (var l in licitacoes)
            {
                table.Rows.Add(l.TipoLicitacao, l.NumLicitacao, l.AnoLicitacao);
            }

            return table;
        }

        public List<Licitacao> GetLicitacoes(IWebDriver driver)
        {
            var listData = new List<Licitacao>();
            var cont = new HashSet<string>();

            var tipoLicitacao = new[]
            {
                Constants.Pregao,
                Constants.Concorrencia,
                Constants.Dispensa
            };

            Utils.WaitElement(driver, "BtnPesquisarLicitacoes", _elementMap);
            driver.FindElement(_elementMap.GetSelector("BtnPesquisarLicitacoes")).Click();
            Utils.WaitElement(driver, "TabPublicados", _elementMap);
            var rows = driver.FindElements(_elementMap.GetSelector("GrupoInfoLicitacao"));

            foreach (var row in rows)
            {
                foreach (var tipo in tipoLicitacao)
                {
                    try
                    {
                        var spanTitulo = row.FindElement(By.XPath($".//*[contains(text(),'{tipo}')]"));
                        string texto = spanTitulo.Text;

                        var regex = new Regex(@"Nº\s*(\d+)\s*/\s*(\d{4})", RegexOptions.IgnoreCase);
                        var match = regex.Match(texto);

                        if (match.Success)
                        {
                            string num = match.Groups[1].Value;
                            string ano = match.Groups[2].Value;
                            string key = $"{num}/{ano}";

                            if (!cont.Contains(key))
                            {
                                cont.Add(key);

                                listData.Add(new Licitacao
                                {
                                    TipoLicitacao = tipo,
                                    NumLicitacao = Convert.ToInt32(num),
                                    AnoLicitacao = Convert.ToInt32(ano)
                                });
                            }
                        }
                    }
                    catch (NoSuchElementException)
                    {
                        continue;
                    }
                }
            }
            return listData;
        }
    }
}
