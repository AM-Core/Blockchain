using Domain;
using Domain.Interfaces;

namespace DomainService;

public class BlockMiner : IBlockMiner
{
    private readonly HashingHandler _hashingHandler;
    private readonly NonceRunner _nonceRunner;
    private readonly Mempool _mempool;

    public BlockMiner()
    {
        _hashingHandler = new HashingHandler();
        _nonceRunner = new NonceRunner(new HashingHandler());
        _mempool = new Mempool();
    }

    public Block MineBlock(Block block)
    {
        throw new NotImplementedException();
    }

    public bool ValidateBlock(Block block)
    {
        throw new NotImplementedException();
    }

    public Task<Block> MineBlockAsync(Block block)
    {
        throw new NotImplementedException();
    }

    public Task<Block> MineBlockAsync(MiningConfig miningConfig)
    {
        throw new NotImplementedException();
    }
}