namespace game.GameEngine.Components;

public class Action : Component
{
    public ActionType ActionType { get; set; }
    public int DelayStep { get; set; }
    public int Delay { get; set; }
}

public enum ActionType
{
    ZombieAction
}