using Domain;

namespace Application.MiningApplication;

public sealed class DifficultyApplication
{
    public void SetDifficulty(int difficulty, MiningConfig miningConfig)
    {
        miningConfig.Difficulty = difficulty;
    }
}