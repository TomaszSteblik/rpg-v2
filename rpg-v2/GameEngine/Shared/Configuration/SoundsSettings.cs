using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace game.GameEngine.Shared.Configuration;

public record struct SoundsSettings()
    : ISettings
{
    public uint BackgroundMusicVolume { get; init; } = 5;

    public SoundsSettings(uint backgroundMusicVolume = 5) : this()
    {
        BackgroundMusicVolume = backgroundMusicVolume;
    }
    
    
    
}