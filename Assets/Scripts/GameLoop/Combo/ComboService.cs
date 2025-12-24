public class ComboService : IComboService
{
    private int comboCount = 0;

    public void ResetCombo()
    {
        comboCount = 0;
    }

    public void IncrementCombo()
    {
        comboCount++;
    }

    public int GetComboMultiplier()
    {
        return 1 + comboCount;
    }
}
