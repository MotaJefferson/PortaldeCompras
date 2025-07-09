using PortaldeCompras.Domain.Interfaces;
using System.Data;

namespace PortaldeCompras.Application.Services
{
    public class AutomationService
    {
        private readonly IBrowser _browser;
        private readonly IDataExtractor<DataTable> _dataExtractor;
        private readonly IDataSaver _dataSaver;

        public AutomationService(IBrowser browser, IDataExtractor<DataTable> dataExtractor, IDataSaver dataSaver)
        {
            _browser = browser;
            _dataExtractor = dataExtractor;
            _dataSaver = dataSaver;
        }

        public int Run(string url, string path)
        {
            var driver = _browser.Start();
            _browser.NavigateTo(url);

            var data = _dataExtractor.GetData(driver);
            var savedCount = _dataSaver.Save(data, path);
            driver.Quit();
            return savedCount;
        }
    }
}
