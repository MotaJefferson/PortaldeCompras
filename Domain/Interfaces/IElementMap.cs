using OpenQA.Selenium;

namespace PortaldeCompras.Domain.Interfaces
{
    public interface IElementMap
    {
        By GetSelector(string elementKey);
    }
}
