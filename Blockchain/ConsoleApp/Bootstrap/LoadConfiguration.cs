using System.Text.Json;
using Domain;

namespace ConsoleApp.Bootstrap;

public class LoadConfiguration
{
    private readonly string _configFilePath = "config.json";
    public bool LoadConfigs()
    {
        
        var jsonString = File.ReadAllText(_configFilePath);
        var config = JsonSerializer.Deserialize<MiningConfig>(jsonString);
        return true;
    }
}