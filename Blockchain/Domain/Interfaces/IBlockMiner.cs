namespace Domain.Interfaces;

public interface IBlockMiner
{
    Block MineBlock(Block block);
    bool ValidateBlock(Block block);
    Task<Block> MineBlockAsync(Block block);
}