public class ScoreService : IScoreService
{
    private int score = 0;

    public void ResetScore()
    {
        score = 0;
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public int GetScore()
    {
        return score;
    }
}
