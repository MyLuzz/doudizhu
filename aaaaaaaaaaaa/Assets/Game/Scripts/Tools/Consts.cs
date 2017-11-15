using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Consts
{
    /// <summary>
    /// 游戏数据路径信息
    /// </summary>
	public static string content = "<GameData><PlayerIntergration>0</PlayerIntergration><ComputerLeftIntergration>0</ComputerLeftIntergration><ComputerRightIntergration>0</ComputerRightIntergration><Rounds>0</Rounds><Win>0</Win></GameData>";
	public static string dataPath = Application.persistentDataPath + "/data.xml";
	public static string DataPath
	{
		get{
			if (!File.Exists(dataPath)||dataPath=="") {
				File.WriteAllText (dataPath, content);
			}
			return dataPath;
		}
	}


}
	

/// <summary>
/// View的事件(更新积分、成功出牌、显示回合得分、打开任务状态栏、显示房间号...)
/// </summary>
public enum ViewEvent
{
    CHANGE_MULTIPLE,//改变倍数
    COMPLETE_DEAL,//完成发牌
    DEAL_THREECARD,//发底牌
    REQUEST_PLAY,//请求出牌
    SUCCESSED_PLAY,//成功出牌
    COMPLETE_PLAY,//完成出牌
    RESTART_GAME,//重新开始
    UPDATE_INTEGRATION,//更新积分
	SET_ROUNDSCORE,//将积分赋值给人物的积分UI子物体
	SHOW_ROUNDSCORE,//显示回合得分
	GAME_END,//局数打完了
	RESTART_FIRST,//从头开始
	OPEN_INFORMATION,//打开人物状态栏
    SHOW_ROOMNUMBER//显示房间号
}

/// <summary>
/// Command的事件(抢地主、出牌、不出...)
/// </summary>
public enum CommandEvent
{
    ChangeMultiple,//改变倍数
    RequestDeal,//请求发牌
    DealCard,//发牌
    GrabLandLord,//抢地主
    PlayCard,//出牌
    PassCard,//不出
    GameOver,//游戏结束
    RequestUpdate,//更新积分
}


/// <summary>
/// 面板类型(开始面板、背景、结束...)
/// </summary>
public enum PanelType
{
    StartPanel,
    BackgroundPanel,
    CharacterPanel,
    InteractionPanel,
    GameOverPanel
}

/// <summary>
/// 角色类型(牌库、玩家、电脑、桌子)
/// </summary>
public enum CharacterType
{
    Library = 0,//牌库
    Player = 1,//玩家
    ComputerRight = 2,//电脑
    ComputerLeft = 3,
    Desk//桌子
}


/// <summary>
/// 卡牌花色(梅花、方块、黑桃、红桃)
/// </summary>
public enum Colors
{
    None,
    Club,//梅花
    Heart,//红桃
    Spade,//黑桃
    Square//方片
}


/// <summary>
/// 卡牌的权值(牌的大小，JQK、大小王等)
/// </summary>
public enum Weight
{
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King,
    One,
    Two,
    SJoker,
    LJoker
}


/// <summary>
/// 出牌类型(单牌、顺子、连对、飞机...)
/// </summary>
public enum CardType
{
    None,
    Single,//单
    Double,//对儿
    Straight,//顺子
    DoubleStraight,//双顺
    TripleStraight,//飞机
    Three,//三不带
    ThreeAndOne,//三带一
    ThreeAndTwo,//三代二
    Boom,//炸弹
    JokerBoom//王炸
}


/// <summary>
/// 身份(农民、地主)
/// </summary>
public enum Identity
{
    Farmer,//农民
    Landlord//地主
}
