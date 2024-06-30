using System;
using System.Collections.Generic;
using rpg_v2;

namespace game.GameEngine.GameStates;

public class InGameMenuGameState : MenuBaseGameState
{
    public InGameMenuGameState() : 
        base(
            new List<(Action, string)>
            {
                (() => MainGame.CurrentGameState = new SaveMenuState(),"Save game"),
                (() => MainGame.CurrentGameState = new LoadMenuState(),"Load game"),
                (() => MainGame.CurrentGameState = new MapGameState(),"Go back"),
                (() => Program.Game.Exit(),"Exit to desktop")
            }, 
            () => MainGame.CurrentGameState = new MapGameState())
    {
        
    }
}