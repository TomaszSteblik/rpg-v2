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
            switch (SelectPosition)
            {
                case 0 when SoundManager.BackgroundMusicVolume != 0:
                    SoundManager.BackgroundMusicVolume -= 1;
                    break;
                case 1 when VideoManager.FpsTargetCap == 30:
                    VideoManager.FpsTargetCap = int.MaxValue;
                    break;
                case 1 when VideoManager.FpsTargetCap == 60:
                    VideoManager.FpsTargetCap = 30;
                    break;
                case 1 when VideoManager.FpsTargetCap == 120:
                    VideoManager.FpsTargetCap = 60;
                    break;
                case 1 when VideoManager.FpsTargetCap == int.MaxValue:
                    VideoManager.FpsTargetCap = 120;
                    break;
                default:
                    return;
            }
        }, true);
        
        InputManager.StartTrackingKey(Keys.Right, () =>
        {
            switch (SelectPosition)
            {
                case 0 when SoundManager.BackgroundMusicVolume < 10:
                    SoundManager.BackgroundMusicVolume += 1;
                    break;
                case 1 when VideoManager.FpsTargetCap == 30:
                    VideoManager.FpsTargetCap = 60;
                    break;
                case 1 when VideoManager.FpsTargetCap == 60:
                    VideoManager.FpsTargetCap = 120;
                    break;
                case 1 when VideoManager.FpsTargetCap == 120:
                    VideoManager.FpsTargetCap = int.MaxValue;
                    break;
                case 1 when VideoManager.FpsTargetCap == int.MaxValue:
                    VideoManager.FpsTargetCap = 30;
                    break;
                default:
                    return;
            }
        }, true);
    }

    private static string GetBackgroundMusicVolumeText()
    {
        var volume = SoundManager.BackgroundMusicVolume;
        var builder = new StringBuilder();
        builder.Append($"Background music volume: {volume}/10");
        return builder.ToString();
    }
    
    public static string GetFpsTargetCapText() => $"FPS: {(VideoManager.FpsTargetCap == int.MaxValue ? "UNLIMITED" : VideoManager.FpsTargetCap)}";

    protected override List<(Action Action, Func<string> Name)> Actions => new()
    {
        (() => { }, GetBackgroundMusicVolumeText),
        (() => { }, GetFpsTargetCapText)
    };
    
    protected override void OnEscape() => MainGame.CurrentGameState = _previousGameState;
}