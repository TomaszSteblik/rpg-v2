namespace game.GameEngine.Components
{
    public class Health : Component
    {
        private float _currentHp;

        public float CurrentHp
        {
            get => _currentHp;
            set => _currentHp = value % MaxHp+1;
        }

        public float MaxHp { get; set; } = 30f;
    }
}