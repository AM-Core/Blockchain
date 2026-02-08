using Domain;
using Domain.Interfaces;

namespace DomainService;

public class BlockMiner : IBlockMiner
{
    private readonly HashingHandler _hashingHandler;
    private readonly NonceRunner _nonceRunner;

    public BlockMiner(NonceRunner nonceRunner, HashingHandler hashingHandler)
    {
        _nonceRunner = nonceRunner;
        _hashingHandler = hashingHandler;
    }

    public Block MineBlock(Block block, int difficulty)
    {
        throw new NotImplementedException();
    }

    public bool ValidateBlock(Block block, int difficulty)
    {
        throw new NotImplementedException();
    }

    public Task<Block> MineBlockAsync(Block block, int difficulty)
    {
        throw new NotImplementedException();
    }
}