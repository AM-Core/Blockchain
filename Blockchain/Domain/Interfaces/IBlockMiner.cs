namespace Domain.Interfaces;

public interface IBlockMiner
{
    Block MineBlock(Block block, int difficulty);
    bool ValidateBlock(Block block, int difficulty);
    Task<Block> MineBlockAsync(Block block, int difficulty);
}