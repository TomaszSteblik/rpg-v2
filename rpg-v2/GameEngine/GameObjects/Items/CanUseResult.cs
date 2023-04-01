namespace game.GameEngine.GameObjects.Items;

public class CanUseResult
{
    public bool CanUse { get; init; }
    public string? ErrorMessage { get; init; }

    public CanUseResult(bool canUse, string? errorMessage = null)
    {
        CanUse = canUse;
        ErrorMessage = errorMessage;
    }
}