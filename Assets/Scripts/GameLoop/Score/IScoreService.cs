public interface IScoreService
{
    void ResetScore();
    void AddScore(int amount);
    int GetScore();
}
