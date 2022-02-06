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
        private Dictionary<Keys, KeyState> _previousKeyStates;
        private List<Keys> _keysToRepeat;

        private double _timeForRepeatingKeys;
        
        public InputManager()
        {
            _trackedKeysList = new List<Keys>();
            _actionsForKeys = new Dictionary<Keys, Action>();
            _previousKeyStates = new Dictionary<Keys, KeyState>();
            _keysToRepeat = new List<Keys>();
            _timeForRepeatingKeys = 0;
        }

        public void StartTrackingKey(Keys key, Action action, bool repeating = false)
        {
            //TODO: FIX MATCHING IF KEYS SHOULD REPEAT
            _trackedKeysList.Add(key);
            _actionsForKeys.Add(key, action);
            _previousKeyStates.Add(key,KeyState.Down);
            if (repeating)
            {
                _keysToRepeat.Add(key);
            }
        }

        public void CheckInputAndAct(GameTime gameTime)
        {
            for (var index = 0; index < _trackedKeysList.Count; index++)
            {
                var key = _trackedKeysList[index];
                if (_previousKeyStates[key] == KeyState.Up && Keyboard.GetState().IsKeyDown(key))
                {
                    _timeForRepeatingKeys = gameTime.TotalGameTime.TotalMilliseconds;
                    _actionsForKeys[key].Invoke();
                    return;
                }
                
                if (_keysToRepeat.Contains(key)
                    && _previousKeyStates[key] == KeyState.Down 
                    && Keyboard.GetState().IsKeyDown(key)
                    && gameTime.TotalGameTime.TotalMilliseconds - _timeForRepeatingKeys > 250)
                {
                    _timeForRepeatingKeys = gameTime.TotalGameTime.TotalMilliseconds;
                    _actionsForKeys[key].Invoke();
                    return;
                }
            }
        }

        public void UpdateStates(GameTime gameTime)
        {
            for (var index = 0; index < _trackedKeysList.Count; index++)
            {
                var key = _trackedKeysList[index];
                _previousKeyStates[key] = Keyboard.GetState()[key];
            }
        }
    }
}