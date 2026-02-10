using Domain;

namespace DomainService;

public class BlockMiner
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

    public Block MineBlock(MiningConfig miningConfig)
    {
        var transactions = _mempool.GetTransactionsSortedToCreateBlock();
        var block = new Block(miningConfig.Difficulty, transactions);
        block.Nonce = _nonceRunner.FindValidNonce(block);
        block.BlockHash = _hashingHandler.ComputeBlockHash(block);
        block.MerkleRoot = _hashingHandler.ComputeMerkleRoot(transactions);
        return block;
    }

    public bool ValidateBlock(Block block)
    {
        throw new NotImplementedException();
    }
}