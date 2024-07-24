using System;
using System.IO;
using System.Linq;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using rpg_v2;

namespace game.GameEngine.GameStates;

public class LoadMenuState : IGameState
{
    private readonly InputManager _inputManager;
    private int _selectPosition;
    private string[] _saveFiles;

    public void Draw(SpriteBatch spriteBatch)
    {
        var font = MainGame.FontSystem.GetFont(22);
        const int xPosition = 120;
        var yPosition = 300;
        for (var index = 0; index < _saveFiles.Length; index++)
        {
            var saveFile = _saveFiles[index];
            yPosition += 42;
            spriteBatch.DrawString(font, saveFile.Split(new[] { '/', '\\' }, StringSplitOptions.None).Last(),
                new Vector2(xPosition, yPosition), _selectPosition == index ? Color.Blue : Color.White);
        }
    }

    public void Update(GameTime gameTime)
    {
        _inputManager.CheckInputAndAct(gameTime);
        _inputManager.UpdateStates(gameTime);
    }

    public LoadMenuState(IGameState previousGameState)
    {
        _inputManager = new InputManager();
        _selectPosition = 0;

        var saveFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SourceOfMagic", "Saves");

        if (!Directory.Exists(saveFolderPath))
            Directory.CreateDirectory(saveFolderPath);

        _saveFiles = Directory.GetFiles(saveFolderPath);

        _inputManager.StartTrackingKey(Keys.Up, MoveSelectUp, true);

        _inputManager.StartTrackingKey(Keys.Down, MoveSelectDown, true);

        _inputManager.StartTrackingKey(Keys.W, MoveSelectUp, true);

        _inputManager.StartTrackingKey(Keys.S, MoveSelectDown, true);

        _inputManager.StartTrackingKey(Keys.Enter, ConfirmSelection);

        _inputManager.StartTrackingKey(Keys.Escape, () => MainGame.CurrentGameState = previousGameState);

        _inputManager.StartTrackingKey(Keys.R, DeleteSelection, false);
    }

    private void ConfirmSelection()
    {
        SaveManager.LoadGame(_saveFiles[_selectPosition]);
        MainGame.CurrentGameState = new MapGameState();
    }

    private void MoveSelectDown()
    {
        if (_selectPosition >= _saveFiles.Length - 1)
            _selectPosition = 0;
        else
            _selectPosition++;
    }

    private void MoveSelectUp()
    {
        if (_selectPosition <= 0)
            _selectPosition = _saveFiles.Length - 1;
        else
            _selectPosition--;
    }

    private void DeleteSelection()
    {
        if (_saveFiles.Length == 0)
            return;
        File.Delete(_saveFiles[_selectPosition]);
        _saveFiles = _saveFiles.Except(new[] { _saveFiles[_selectPosition] }).ToArray();
        if (_selectPosition > _saveFiles.Length)
            _selectPosition = _saveFiles.Length;
    }
}