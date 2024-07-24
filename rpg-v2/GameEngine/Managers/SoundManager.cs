using game.GameEngine.Shared.Configuration;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace game.GameEngine
{
    public static class SoundManager
    {
        private static SoundEffectInstance _backgroundMusic;
        private static ContentManager _contentManager;

        private static SoundsSettings _soundsSettings;
        
        public static uint BackgroundMusicVolume
        {
            get => _soundsSettings.BackgroundMusicVolume;
            set
            {
                var newSettings = _soundsSettings with {BackgroundMusicVolume = value};
                _soundsSettings = newSettings;
                SettingsParser.SaveSettings(_soundsSettings);
            }
        }

        public static void Init(ContentManager content)
        {
            _soundsSettings = SettingsParser.GetSettings<SoundsSettings>();
            _contentManager = content;
            SettingsParser.OnSettingsChange += () =>
            {
                _soundsSettings = SettingsParser.GetSettings<SoundsSettings>();
                _backgroundMusic.Volume = _soundsSettings.BackgroundMusicVolume/10f;
            };
        }
        
        public static void PlayBackgroundMusic()
        {
            _backgroundMusic = _contentManager.Load<SoundEffect>("Audio/dark_ambient").CreateInstance();
            _backgroundMusic.IsLooped = true;
            _backgroundMusic.Volume = _soundsSettings.BackgroundMusicVolume/10f;
            _backgroundMusic.Play();
        }

    }
}