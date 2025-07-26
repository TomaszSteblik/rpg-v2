namespace game.GameEngine.Components;

public class Mana : Component
{
    private float _currentMana;
    public float MaxMana { get; set; } = 10f;

    public float CurrentMana
    {
        get => _currentMana;
        set => _currentMana = value % (MaxMana + 1);
    }

}