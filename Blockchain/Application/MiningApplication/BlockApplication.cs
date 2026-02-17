using Application.QueryHandler.Command;
using Domain.Contracts;
using Domain.Interfaces;
using DomainService;

namespace Application.MiningApplication;

public sealed class BlockApplication : ICommand
{
    private readonly BlockMiner _blockMiner;
    private readonly IResultWriter _resultWriter;

    public BlockApplication(BlockMiner blockMiner, IResultWriter resultWriter)
    {
        _blockMiner = blockMiner;
        _resultWriter = resultWriter;
    }
    
    public void Execute(Command command)
    {
        var block = _blockMiner.MineBlock();
        _resultWriter.WriteBlock(new BlockDto(block));
    }
}