using System;
using System.Collections.Generic;
using rpg_v2;

namespace game.GameEngine.GameStates;

public class InGameMenuGameState : MenuBaseGameState
{
    protected override List<(Action Action, Func<string> Name)> Actions =>
        new()
        {
            (() => MainGame.CurrentGameState = new SaveMenuState(),() => "Save game"),
            (() => MainGame.CurrentGameState = new LoadMenuState(this),() => "Load game"),
            (() => MainGame.CurrentGameState = new SettingsMenu(this),() => "Settings"),
            (() => MainGame.CurrentGameState = new MapGameState(),() => "Go back"),
            (() => Program.Game.Exit(),() => "Exit to desktop")
        };

    protected override void OnEscape() => MainGame.CurrentGameState = new MapGameState();
}