namespace ConsoleApp.Bootstrap;

public class MiningConfigDto
{
    public MiningConfigDto(long difficulty, long size)
    {
        Difficulty = difficulty;
        Size = size;
    }

    public long Difficulty { get; set; }
    public long Size { get; set; } = 1000000;
}