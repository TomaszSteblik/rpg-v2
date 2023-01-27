namespace game.GameEngine.GameObjects.States;

public interface IState
{
    void OnCreate(IStateData data);
    void OnDestruct(IStateData data);
    void Act(IStateData data);
}