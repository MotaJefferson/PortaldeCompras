using Microsoft.Extensions.DependencyInjection;
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
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IBrowser, BrowserSelenium>()
            .AddSingleton<IElementMap, ElementMap>()
            .AddSingleton<IDataExtractor<DataTable>, DataExtractor>()
            .AddSingleton<IDataSaver, SpreadsheetStorage>()
            .AddTransient<AutomationService>()
            .BuildServiceProvider();

        string url = "https://smaram.smarapd.com.br/demonstracao/pregao/";
        string outputPath = "C:\\Temp\\Planilha.xlsx";

        var request = new RequestDto
        {
            Url = url,
            OutputPath = outputPath
        };

        var result = RequestValidator.Validate(request);
        if (!result.IsValid)
        {
            foreach (var erro in result.Errors)
            {
                Console.WriteLine($"Erro: {erro}");
            }
            return; 
        }

        try
        {
            var service = serviceProvider.GetService<AutomationService>();
            if (service != null)
            {
                var insertedCount = service.Run(request.Url, request.OutputPath);
                Console.WriteLine($"{insertedCount} novos registros foram inseridos na planilha.");
            }
            else
            {
                Console.WriteLine("Erro: Não foi possível iniciar o serviço de automação.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro inesperado: {ex.Message}");
        }
    }
}
