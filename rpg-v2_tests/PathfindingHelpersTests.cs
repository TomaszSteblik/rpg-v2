using game.GameEngine;
using game.GameEngine.Systems;

namespace rpg_v2_tests;

public class PathfindingHelpersTests
{
    [Theory]
    [InlineData(12, 12, 33, 33, 30)]
    [InlineData(12, 12, 12, 33, 21)]
    [InlineData(12, 12, 33, 12, 21)]
    [InlineData(12, 12, 3, 3, 13)]
    [InlineData(12, 12, 12, 3, 9)]
    [InlineData(12, 12, 3, 12, 9)]
    [InlineData(12, 12, 33, 3, 23)]
    [InlineData(12, 12, 3, 33, 23)]
    public void Test1(int x1, int y1, int x2, int y2, int expected)
    {
        var result = PathfindingSystem.CalculateDistance(new Node(x1,y1), new Node(x2,y2));
        Assert.Equal(expected, Math.Round(result));
    }
}