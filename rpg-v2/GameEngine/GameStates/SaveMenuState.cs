using System;
using System.IO;
using System.Linq;
using FontStashSharp;
using rpg_v2;

namespace game.GameEngine.GameStates;

public class SaveMenuState : IGameState
{
    private readonly InputManager _inputManager;
    private int _selectPosition;
    private readonly string[] _saveFiles;
    private bool _isTyping;
    private string _typedName;
        
    public void Draw(SpriteBatch spriteBatch)
    {
        var font = MainGame.FontSystem.GetFont(22);
        const int xPosition = 120;
        var yPosition = 300;
        spriteBatch.DrawString(font, _isTyping ? string.IsNullOrWhiteSpace(_typedName) ? "..." : _typedName : "New save", 
            new Vector2(xPosition, yPosition), _selectPosition == 0 ? Color.Blue : Color.White);
        for (var index = 0; index < _saveFiles.Length; index++)
        {
            var saveFile = _saveFiles[index];
            yPosition += 42;
            spriteBatch.DrawString(font, saveFile.Split('/', '\\').Last(), new Vector2(xPosition, yPosition),
                _selectPosition == index+1 ? Color.Blue : Color.White);
        }
    }

    public void Update(GameTime gameTime)
    {
        _inputManager.CheckInputAndAct(gameTime);
        _inputManager.UpdateStates(gameTime);
    }

    public SaveMenuState()
    {
        _inputManager = new InputManager();
        _selectPosition = 0;
        _isTyping = false;
        _typedName = string.Empty;

        var saveFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"SourceOfMagic","Saves");
        
        if (!Directory.Exists(saveFolderPath))
            Directory.CreateDirectory(saveFolderPath);
        
        _saveFiles = Directory.GetFiles(saveFolderPath);
        
        _inputManager.StartTrackingKey(Keys.Up, () =>
        {
            if(_isTyping)
                return;
            
            if (_selectPosition <= 0)
            {
                _selectPosition = _saveFiles.Length;
            }
            else
            {
                _selectPosition--;
            }
        },true);
        
        _inputManager.StartTrackingKey(Keys.Down, () =>
        {
            if(_isTyping)
                return;
            
            if (_selectPosition >= _saveFiles.Length)
            {
                _selectPosition = 0;
            }
            else
            {
                _selectPosition++;
            }
        },true);
        
        _inputManager.StartTrackingKey(Keys.W, () =>
        {
            if(_isTyping)
                return;
            
            if (_selectPosition <= 0)
            {
                _selectPosition = _saveFiles.Length;
            }
            else
            {
                _selectPosition--;
            }
        },true);
        
        _inputManager.StartTrackingKey(Keys.S, () =>
        {
            if(_isTyping)
                return;
            
            if (_selectPosition >= _saveFiles.Length)
            {
                _selectPosition = 0;
            }
            else
            {
                _selectPosition++;
            }
        },true);
        
        _inputManager.StartTrackingKey(Keys.Enter, () =>
        {
            switch (_selectPosition)
            {
                case 0:
                    if (_isTyping)
                    {
                        SaveManager.SaveGame(Path.Combine(saveFolderPath,_typedName));
                        MainGame.CurrentGameState = new MapGameState();
                    }
                    else
                    {
                        _isTyping = true;
                        MainGame.GameWindow.TextInput += (sender, args) =>
                        {
                            var pressedKey = args.Key;
                            if (pressedKey == Keys.Back)
                            {
                                _typedName = string.Concat(_typedName.SkipLast(1));
                                return;
                            }
                            
                            var character = args.Character;
                            _typedName += character;
                        };
                    }
                    break;
                default:
                    SaveManager.SaveGame(_saveFiles[_selectPosition-1]);
                    MainGame.CurrentGameState = new MapGameState();
                    break;
            }
        },true);

        _inputManager.StartTrackingKey(Keys.Escape, () =>
        {
            if(_isTyping)
            {
                _isTyping = false;
                _typedName = string.Empty;
            }
            else
                MainGame.CurrentGameState = new MapGameState();
        }, false);
    }
}