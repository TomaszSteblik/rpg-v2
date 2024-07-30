namespace game.GameEngine.Shared.Configuration;

public record struct DisplaySettings() : ISettings
{
    public int FpsTargetCap { get; set; } = 60;

    public DisplaySettings(int fpsTargetCap = 60) : this()
    {
        FpsTargetCap = fpsTargetCap;
    }
}