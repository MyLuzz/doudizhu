using UnityEngine;
using System.Collections;

/// <summary>
/// 积分
/// </summary>
public class IntegrationModel
{
    /// <summary>
    /// 底分
    /// </summary>
    public int BasePoint;

    /// <summary>
    /// 倍数
    /// </summary>
    public int Multiples;

    /// <summary>
    /// 总分
    /// </summary>
    public int Result
    {
        get { return BasePoint * Multiples; }
    }

    /// <summary>
    /// 玩家积分
    /// </summary>
    private int playerIntergration;
    public int PlayerIntergration
    {
        set
        {
                playerIntergration = value;
        }
        get { return playerIntergration; }
    }

    private int computerLeftIntergration;
    public int ComputerLeftIntergration
    {
        set
        {
                computerLeftIntergration = value;
        }
        get { return computerLeftIntergration; }
    }
    private int computerRightIntergration;
    public int ComputerRightIntergration
    {
        set
        {
                computerRightIntergration = value;
        }
        get { return computerRightIntergration; }
    }
	private int rounds;
	public int Rounds
	{
		set
		{
			rounds = value;
		}
		get{ return rounds; }
	}


    /// <summary>
    /// 初始化积分信息
    /// </summary>
    public void InitIntergration()
    {
        PlayerIntergration = 0;
        ComputerLeftIntergration = 0;
        ComputerRightIntergration = 0;
		Rounds = 0;

        Multiples = 1;
        BasePoint = 3;
    }

}

