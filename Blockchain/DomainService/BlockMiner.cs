using Domain;
using Domain.Interfaces;

namespace DomainService;

public class BlockMiner : IBlockMiner
{
    private readonly HashingHandler _hashingHandler;
    private readonly NonceRunner _nonceRunner;
    private readonly MiningConfig _miningConfig;

    public BlockMiner(NonceRunner nonceRunner, HashingHandler hashingHandler, 
        MiningConfig miningConfig)
    {
        _nonceRunner = nonceRunner;
        _hashingHandler = hashingHandler;
        _miningConfig = miningConfig;
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