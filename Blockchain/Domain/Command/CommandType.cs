namespace Domain.Command;

public enum CommandType
{
    SetDifficulty,
    AddTransactionToMempool,
    EvictMempool,
    MineBlock
}