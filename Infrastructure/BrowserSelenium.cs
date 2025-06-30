using DocumentFormat.OpenXml.Wordprocessing;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PortaldeCompras.Domain.Interfaces;

namespace PortaldeCompras.Infrastructure
{
    public class BrowserSelenium : IBrowser
    {
        private IWebDriver _driver;

        public IWebDriver Start()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless"); //Ocultar navegador
            options.AddArgument("--window-size=1280,720");
            options.AddArgument("--disable-gpu");
            options.AddExcludedArgument("enable-logging");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--disable-background-networking");
            options.AddArgument("--disable-sync");
            options.AddArgument("--disable-translate");
            options.AddArgument("--metrics-recording-only");
            options.AddArgument("--mute-audio");

            var service = ChromeDriverService.CreateDefaultService();
            service.SuppressInitialDiagnosticInformation = true;
            service.HideCommandPromptWindow = true;

            _driver = new ChromeDriver(service, options);
                     
            return _driver;
        }

        public void NavigateTo(string url) => _driver.Navigate().GoToUrl(url);
    }
}
