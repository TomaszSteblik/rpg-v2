using game.GameEngine.Components;

namespace game.GameEngine.Systems.Helpers;

public static class VisionHelpers
{
    public static bool IsPositionInFov(Vision vision, Position position)
    {
        return vision.CellsInLightOfSight[position.X][position.Y];
    }
}