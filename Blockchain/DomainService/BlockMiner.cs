using Domain;

namespace DomainService;

public class BlockMiner
{
    private readonly HashingHandler _hashingHandler;
    private readonly Mempool _mempool;
    private readonly NonceRunner _nonceRunner;
    private readonly MiningConfig _miningConfig;
    public BlockMiner(Mempool mempool, MiningConfig miningConfig)
    {
        _mempool = mempool;
        _miningConfig = miningConfig;
        _hashingHandler = new HashingHandler();
        _nonceRunner = new NonceRunner(new HashingHandler());
    }

    public Block MineBlock()
    {
        var transactions = _mempool.GetTransactionsSortedToCreateBlock();
        var block = new Block(_miningConfig.Difficulty, transactions);
        block.Nonce = _nonceRunner.FindValidNonce(transactions, block.Difficulty);
        block.BlockHash = _hashingHandler.ComputeBlockHash(block);
        block.MerkleRoot = _hashingHandler.ComputeMerkleRoot(transactions);
        return block;
    }
}