namespace Domain;

public class MiningConfig(int difficulty, int size)
{
    public int Difficulty { get; set; } = difficulty;
    public int Size { get; set; } = size;
}