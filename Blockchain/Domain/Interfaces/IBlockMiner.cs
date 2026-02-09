namespace Domain.Interfaces;

public interface IBlockMiner
{
    Block MineBlock(MiningConfig miningConfig);
    bool ValidateBlock(MiningConfig miningConfig);
    Task<Block> MineBlockAsync(MiningConfig miningConfig);
}