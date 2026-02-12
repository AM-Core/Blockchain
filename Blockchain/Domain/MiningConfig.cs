namespace Domain;

public class MiningConfig
{
    private static readonly Lazy<MiningConfig> _instance = new(() => new MiningConfig());

    public static MiningConfig Instance => _instance.Value;

    public long Difficulty { get; set; }
    public long Size { get; set; } = 1000000;

    private MiningConfig() { }

}