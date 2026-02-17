using Application.MiningApplication.Abstractions;
using Application.QueryHandler.Command;
using Domain.Contracts;
using Domain.Interfaces;
using DomainService;

namespace Application.MiningApplication.Commands;

public sealed class BlockCommand : ICommand
{
    private readonly BlockMiner _blockMiner;
    private readonly IResultWriter _resultWriter;

    public BlockCommand(BlockMiner blockMiner, IResultWriter resultWriter)
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