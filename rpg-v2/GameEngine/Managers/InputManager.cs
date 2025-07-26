using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace game.GameEngine
{
    public class InputManager
    {
        private List<Keys> _trackedKeysList;
        private Dictionary<Keys, Action> _actionsForKeys;
        
        //make this static to keep track of key states across different instances (game states) because there is always
        //only one Keyboard and its state should always be reflected correctly
        private static readonly Dictionary<Keys, KeyState> PreviousKeyStates = new();
        private List<Keys> _keysToRepeat;

        private double _timeForRepeatingKeys;

        public InputManager()
        {
            _trackedKeysList = new List<Keys>();
            _actionsForKeys = new Dictionary<Keys, Action>();
            _keysToRepeat = new List<Keys>();
            _timeForRepeatingKeys = 0;
        }

        public void StartTrackingKey(Keys key, Action action, bool repeating = false)
        {
            //TODO: FIX MATCHING IF KEYS SHOULD REPEAT
            _trackedKeysList.Add(key);
            _actionsForKeys.Add(key, action);
            PreviousKeyStates.TryAdd(key, KeyState.Down);
            if (repeating)
            {
                _keysToRepeat.Add(key);
            }
        }

        public bool CheckInputAndAct(GameTime gameTime)
        {
            for (var index = 0; index < _trackedKeysList.Count; index++)
            {
                var key = _trackedKeysList[index];
                var keyboardState = Keyboard.GetState();
                if (PreviousKeyStates[key] is KeyState.Up && keyboardState.IsKeyDown(key))
                {
                    _timeForRepeatingKeys = gameTime.TotalGameTime.TotalMilliseconds;
                    _actionsForKeys[key].Invoke();
                    return true;
                }

                if (_keysToRepeat.Contains(key)
                    && PreviousKeyStates[key] == KeyState.Down
                    && Keyboard.GetState().IsKeyDown(key)
                    && gameTime.TotalGameTime.TotalMilliseconds - _timeForRepeatingKeys >
                    (keyboardState.IsKeyDown(Keys.LeftShift) ? 50 : 250))
                {
                    _timeForRepeatingKeys = gameTime.TotalGameTime.TotalMilliseconds;
                    _actionsForKeys[key].Invoke();
                    return true;
                }
            }

            return false;
        }

        public void UpdateStates(GameTime gameTime)
        {
            for (var index = 0; index < _trackedKeysList.Count; index++)
            {
                var key = _trackedKeysList[index];
                PreviousKeyStates[key] = Keyboard.GetState()[key];
            }
        }
    }
}