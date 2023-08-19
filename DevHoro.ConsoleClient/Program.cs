using DevHoro.ConsoleClient;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

var services = new ServiceCollection();
services.AddHttpClient();

var serviceProvider = services.BuildServiceProvider();

while (true)
{
    try
    {
        Console.Write("Language: ");
        var language = Console.ReadLine();

        if (String.IsNullOrEmpty(language))
        {
            Console.WriteLine("Language is empty");
            continue;
        }

        Console.Write("Date: ");

        var dateString = Console.ReadLine();

        if (String.IsNullOrEmpty(dateString))
        {
            Console.WriteLine("Date is empty");
            continue;
        }

        var date = DateTime.Parse(dateString);

        var httpClientFacotry = serviceProvider.GetRequiredService<IHttpClientFactory>();
        var httpDevHoroClient = new DevHoroHttpClient("http://localhost:5000/", httpClientFacotry.CreateClient());

        var horo = await httpDevHoroClient.GetHoroAsync(language, date);

        Console.WriteLine(horo.Text);
    }
    catch (ApiException ex)
    {
        var error = JsonConvert.DeserializeObject<dynamic>(ex.Response)!;
        Console.WriteLine(error.detail);
    }
    catch (FormatException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine("System error. Please try later.");
    }

    Console.WriteLine();
}