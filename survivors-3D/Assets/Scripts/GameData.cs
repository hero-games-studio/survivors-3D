using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int level;
    public int rescuedNum;

    public GameData(Gamemanager GM)
    {
        level = GM.level;
        rescuedNum = GM.rescuedNum;
    }
}
