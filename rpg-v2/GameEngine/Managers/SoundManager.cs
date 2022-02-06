using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace game.GameEngine
{
    public static class SoundManager
    {
        private static SoundEffectInstance _backgroundMusic;
        private static ContentManager _contentManager;

        public static void Init(ContentManager content)
        {
            _contentManager = content;
        }

        public static void Play()
        {
            _backgroundMusic = _contentManager.Load<SoundEffect>("Audio/dark_ambient").CreateInstance();
            _backgroundMusic.IsLooped = true;
            _backgroundMusic.Play();
        }
    }
}