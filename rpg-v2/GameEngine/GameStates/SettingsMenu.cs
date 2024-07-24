using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using rpg_v2;

namespace game.GameEngine.GameStates;

public class SettingsMenu : MenuBaseGameState
{
    private readonly IGameState _previousGameState;

    public SettingsMenu(IGameState previousGameState)
    {
        _previousGameState = previousGameState;
        InputManager.StartTrackingKey(Keys.Left, () =>
        {
            if(SelectPosition != 0 || SoundManager.BackgroundMusicVolume <= 0)
                return;
            
            SoundManager.BackgroundMusicVolume -= 1;
            
        }, true);
        
        InputManager.StartTrackingKey(Keys.Right, () =>
        {
            if(SelectPosition != 0 || SoundManager.BackgroundMusicVolume >= 10)
                return;

            SoundManager.BackgroundMusicVolume += 1;

        }, true);
    }

    private static string GetBackgroundMusicVolumeText()
    {
        var volume = SoundManager.BackgroundMusicVolume;
        var builder = new StringBuilder();
        builder.Append($"Background music volume: {volume}/10");
        return builder.ToString();
    }

    protected override List<(Action Action, Func<string> Name)> Actions => new()
    {
        (() => { }, GetBackgroundMusicVolumeText)
    };
    
    protected override void OnEscape() => MainGame.CurrentGameState = _previousGameState;
}