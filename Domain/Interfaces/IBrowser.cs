using OpenQA.Selenium;

namespace PortaldeCompras.Domain.Interfaces
{
    public interface IBrowser
    {
        IWebDriver Start();
        void NavigateTo (string url);
    }
}
