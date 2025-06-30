using OpenQA.Selenium;
using System.Data;

namespace PortaldeCompras.Domain.Interfaces
{
    public interface IDataExtractor<T>
    {
        T GetData(IWebDriver driver);
    }
}
