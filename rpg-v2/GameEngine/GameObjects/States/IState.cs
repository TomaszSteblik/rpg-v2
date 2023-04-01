namespace game.GameEngine.GameObjects.States;

public interface IState
{
    void OnCreate(StateData data, Entity entity);
    void OnDestruct(StateData data, Entity entity);
    void Act(StateData data, Entity entity);
}