using OpenQA.Selenium;
using PortaldeCompras.Domain.Interfaces;

namespace PortaldeCompras.Infrastructure
{
    public class ElementMap : IElementMap
    {
        private readonly Dictionary<string, By> _selectors = new()
        {
            {"BtnPesquisarLicitacoes", By.Id("O16B_id-innerCt") },
            {"TabPublicados", By.Id("tab-1017-btnInnerEl") },
            {"GrupoInfoLicitacao", By.XPath($"//*[starts-with(@id, 'gridview-1041-record-')]") }
        };

        public By GetSelector(string elementKey)
        {
            return _selectors[elementKey];
        }
    }
}
