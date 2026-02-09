namespace Domain.Command;

public enum CommandType
{
    SET_DIFFICULTY,
    ADD_TRANSACTION_TO_MEMPOOL,
    EVICT_MEMPOOL,
    MINE_BLOCK
}