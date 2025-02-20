using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace MonkeyFinder.Services;

public class MonkeyService
{
    HttpClient httpClient;
    public MonkeyService()
    {
        this.httpClient = new HttpClient();
    }

    List<Monkey> monkeyList = new List<Monkey>();
    public async Task<List<Monkey>> GetMonkeys()
    {
        if (monkeyList?.Count > 0)
            return monkeyList;

        // Online
        var response = await httpClient.GetAsync("https://www.montemagno.com/monkeys.json");
        if (response.IsSuccessStatusCode)
        {
            var temp = await response.Content.ReadFromJsonAsync(SourceGenerationContext.Default.ListMonkey);
            for (var i = 0; i < 100; i++)
            {
                foreach (var item in temp)
                {
                    monkeyList.Add(item);
                }
            }
            //monkeyList = await response.Content.ReadFromJsonAsync<List<Monkey>>();

        }

        // Offline
        //using var stream = await FileSystem.OpenAppPackageFileAsync("monkeydata.json");
        //using var reader = new StreamReader(stream);
        //var contents = await reader.ReadToEndAsync();
        //monkeyList = JsonSerializer.Deserialize<List<Monkey>>(contents);

        return monkeyList;
    }
}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(List<Monkey>))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}
