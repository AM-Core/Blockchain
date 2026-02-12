using System.Text.Json;
using Domain;

namespace ConsoleApp.Bootstrap;

public class LoadConfiguration
{
    private readonly MiningConfig _miningConfig = MiningConfig.Instance;
    private string _configFilePath = "ConsoleApp/config.json";

    public bool LoadConfigs()
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var solutionRoot = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName;
        _configFilePath = Path.Combine(solutionRoot, _configFilePath);
        var jsonString = File.ReadAllText(_configFilePath);
        var readedConfig = JsonSerializer.Deserialize<MiningConfigDto>(jsonString);
        _miningConfig.Difficulty = readedConfig.Difficulty;
        _miningConfig.Size = readedConfig.Size;
        return true;
    }
}