using System;
using game.GameEngine.Shared.Configuration;
using Microsoft.Xna.Framework;
using rpg_v2;

namespace game.GameEngine;

public static class VideoManager
{
    private static GraphicsDeviceManager _graphicsDeviceManager;
    private static DisplaySettings _displaySettings;
    
    public static int FpsTargetCap
    {
        get => _displaySettings.FpsTargetCap;
        set
        {
            var newSettings = _displaySettings with {FpsTargetCap = value};
            _displaySettings = newSettings;
            SettingsParser.SaveSettings(_displaySettings);
        }
    }

    public static void Init(GraphicsDeviceManager graphicsDeviceManager)
    {
        _displaySettings = SettingsParser.GetSettings<DisplaySettings>();
        _graphicsDeviceManager = graphicsDeviceManager;
        
        SettingsParser.OnSettingsChange += () =>
        {
            _displaySettings = SettingsParser.GetSettings<DisplaySettings>();
            if (_displaySettings.FpsTargetCap == int.MaxValue)
            {
                Program.Game.IsFixedTimeStep = false;
            }
            else
            {
                Program.Game.IsFixedTimeStep = true;
                Program.Game.TargetElapsedTime = TimeSpan.FromSeconds(1 / (double)_displaySettings.FpsTargetCap);
            }
            _graphicsDeviceManager.ApplyChanges();
        };
    }

}