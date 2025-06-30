using PortaldeCompras.Application.Common;
using PortaldeCompras.Application.DTOs;
using PortaldeCompras.Application.Services;
using PortaldeCompras.Application.Validators;
using PortaldeCompras.Domain.Interfaces;
using PortaldeCompras.Infrastructure;
using System.Data;

namespace UI;

class Program
{
    static void Main(string[] args)
    {
        var request = new RequestDto
        {
            Url = "https://smaram.smarapd.com.br/demonstracao/pregao/",
            OutputPath = "C:\\Temp\\Planilha.xlsx"
        };

        var result = RequestValidator.Validate(request);

        try
        {
            if (!result.IsValid)
            {
                foreach (var erro in result.Errors)
                {
                    Console.WriteLine($"Erro: {erro}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }        

        IBrowser browser = new BrowserSelenium();
        IElementMap elementMap = new ElementMap();
        IDataExtractor<DataTable> dataExtractor = new DataExtractor(elementMap);
        IDataSaver dataSaver = new SpreadsheetStorage();

        var service = new AutomationService(browser, dataExtractor, dataSaver);
        service.Run(request.Url, request.OutputPath);
        
    }
}