namespace Domain.Command;

public enum CommandType
{
    SETDIFFICULTY,
    ADDTRANSACTIONTOMEMPOOL,
    EVICTMEMPOOL,
    MINEBLOCK,
    HELP
}