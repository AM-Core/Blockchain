using System.Text.Json;
using Domain;

namespace ConsoleApp.Bootstrap;

public class LoadConfiguration
{
    
    private readonly MiningConfig _miningConfig;
    private string _configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

    public LoadConfiguration(MiningConfig miningConfig)
    {
        _miningConfig = miningConfig;
    }

    public bool LoadConfigs()
    {
        var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        var jsonString = File.ReadAllText(configPath);
        var readedConfig = JsonSerializer.Deserialize<MiningConfig>(jsonString);
        _miningConfig.Difficulty = readedConfig.Difficulty;
        _miningConfig.Size = readedConfig.Size;
        return true;
    }
}