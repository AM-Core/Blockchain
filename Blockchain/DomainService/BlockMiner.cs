using Domain;

namespace DomainService;

public class BlockMiner
{
    private readonly HashingHandler _hashingHandler;
    private readonly Mempool _mempool;
    private readonly NonceRunner _nonceRunner;
    private readonly MiningConfig _miningConfig;
    public BlockMiner(Mempool mempool, MiningConfig miningConfig, HashingHandler hashingHandler,
        NonceRunner nonceRunner)
    {
        _mempool = mempool;
        _miningConfig = miningConfig;
        _hashingHandler = hashingHandler;
        _nonceRunner = nonceRunner;
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