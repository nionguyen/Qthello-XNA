using System;

namespace WindowsPhoneGame2
{
    public enum GameMode
    {
        PvP = 0,
        AI = 1
    }
    public enum GameTurn
    {
        Player_Pink = -1,
        Player_Blue = 1
    };
    public enum GameState
    {
        GameOn,
        GamePause,
        GameEnd
    };
}