using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using PortaldeCompras.Domain.Interfaces;
using SeleniumExtras.WaitHelpers;

namespace PortaldeCompras.Application.Common
{
    public class Utils
    {        
        public static void WaitElement(IWebDriver driver, string elementKey, IElementMap elementMap)
        {
            var selector = elementMap.GetSelector(elementKey);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            wait.Until(ExpectedConditions.ElementExists(selector));
        }
    }
}
