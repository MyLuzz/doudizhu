using System;
using System.Collections.Generic;

/// <summary>
/// 游戏的数据
/// </summary>
[Serializable]
public class GameData
{
    //玩家积分
    public int PlayerIntergration = 100;
    //电脑积分
    public int ComputerLeftIntergration = 100;
    public int ComputerRightIntergration = 100;

	public int Rounds = 0;
	public int Win = 0;
}

