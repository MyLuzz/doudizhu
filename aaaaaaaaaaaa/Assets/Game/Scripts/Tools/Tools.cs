using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using System;

public static class Tools
{
    private static Transform uiParent;
    /// <summary>
    /// UI的父物体
    /// </summary>
    public static Transform UIParent
    {
        get
        {
            if (uiParent == null)
                uiParent = GameObject.Find("GameRoot").transform;
            return uiParent;
        }
    }
   
    public static Colors getColor(int x)
    {
        switch (x)
        {
            case 0:
                return Colors.Square;//方块
            case 1:
                return Colors.Club;//梅花
            case 2:
                return Colors.Heart;//红桃
            case 3:
                return Colors.Spade;//黑桃
            case 4:
                return Colors.None;
            default:
                return Colors.None;
        }
    }

    public static int get_Color(Colors color)
    {
        switch (color)
        {
            case Colors.None:
                return 4;
            case Colors.Club://梅花
                return 1;
            case Colors.Heart://红桃
                return 2;
            case Colors.Spade://黑桃
                return 3;
            case Colors.Square://方块
                return 0;
            default:
                return -1;
        }
    }
    public static Weight getThreeWeight(int x)//卡牌权值(JQK,大小王等)
    {
        switch (x)
        {
            case 0:
                return Weight.Three;
            case 3:
                return Weight.Four;
            case 6:
                return Weight.Five;
            case 9:
                return Weight.Six;
            case 12:
                return Weight.Seven;
            case 15:
                return Weight.Eight;
            case 18:
                return Weight.Nine;
            case 21:
                return Weight.Ten;
            case 24:
                return Weight.Jack;
            case 27:
                return Weight.Queen;
            case 30:
                return Weight.King;
            case 33:
                return Weight.One;
            case 36:
                return Weight.Two;
            default:
                return Weight.Three;

        }
    }
    public static Weight getDoubleWeight(int x)
    {
        switch (x)
        {
            case 0:
                return Weight.Three;
            case 2:
                return Weight.Four;
            case 4:
                return Weight.Five;
            case 6:
                return Weight.Six;
            case 8:
                return Weight.Seven;
            case 10:
                return Weight.Eight;
            case 12:
                return Weight.Nine;
            case 14:
                return Weight.Ten;
            case 16:
                return Weight.Jack;
            case 18:
                return Weight.Queen;
            case 20:
                return Weight.King;
            case 22:
                return Weight.One;
            case 24:
                return Weight.Two;
            default:
                return Weight.Three;

        }
    }

    public static Weight getWeight(int x)
    {
        switch (x)
        {
            case 0:
                return Weight.Three;
            case 1:
                return Weight.Four;
            case 2:
                return Weight.Five;
            case 3:
                return Weight.Six;
            case 4:
                return Weight.Seven;
            case 5:
                return Weight.Eight;
            case 6:
                return Weight.Nine;
            case 7:
                return Weight.Ten;
            case 8:
                return Weight.Jack;
            case 9:
                return Weight.Queen;
            case 10:
                return Weight.King;
            case 11:
                return Weight.One;
            case 12:
                return Weight.Two;
            case 13:
                return Weight.SJoker;
            case 14:
                return Weight.LJoker;
            default:
                return Weight.Three;
        
        }
    }
    public static int get_Weight(Weight weight)
    {
        switch (weight)
        {
            case Weight.Three:
                return 0;
            case Weight.Four:
                return 1;
            case Weight.Five:
                return 2;
            case Weight.Six:
                return 3;
            case Weight.Seven:
                return 4;
            case Weight.Eight:
                return 5;
            case Weight.Nine:
                return 6;
            case Weight.Ten:
                return 7;
            case Weight.Jack:
                return 8;
            case Weight.Queen:
                return 9;
            case Weight.King:
                return 10;
            case Weight.One:
                return 11;
            case Weight.Two:
                return 12;
            case Weight.SJoker:
                return 13;
            case Weight.LJoker:
                return 14;
            default:
                return -1;
        }
    }
    public static  List<Card> RandomSortList(List<Card> ListT)
    {
        System.Random random = new System.Random();
        List<Card> newList = new List<Card>();
        foreach (Card item in ListT)
        {
            //random.Next(MaxValue)----------返回一个小于所指定最大值的非负随机数
            //在newlist的随机位置插入这张牌
            newList.Insert(random.Next(newList.Count + 1), item);
        }
        return newList;
    }
    public static int NextIndex(int x)
    {
        if (x == 0)
            return 1;
        else if (x == 1)
            return 2;
        else
            return 0;
    }

    public static int LastIndex(int x)
    {
        if (x == 1)
            return 0;
        else if (x == 2)
            return 1;
        else
            return 2;
    }
    /// <summary>
    /// 得到牌组（手牌）
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static List<Card>  getCardList(List<byte> list)
    {
        List<Card> cardlist = new List<Card>();
        for (int i = 0; i < list.Count; i++)
        {
            
            string strTemp = "";
            strTemp = System.Convert.ToString(list[i], 2);//转换成2进制编码
            string c = strTemp.PadLeft(8,'0');//在字符串左边用0来填充，直到达到指定的位数
            int a = Convert.ToInt32(c.Substring(4),2);
            int b = Convert.ToInt32(c.Substring(0,4),2);
            Card card;
            if(getColor(b) == Colors.None)
                 card = new Card(getWeight(a).ToString(), getColor(b), getWeight(a), CharacterType.Player);
            else
                 card = new Card(getColor(b).ToString() + getWeight(a).ToString(), getColor(b), getWeight(a), CharacterType.Player);

            //将牌添加到牌组中
            cardlist.Add(card);
        }
        return cardlist;
    }


    public static string randString()
    {
        string str = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";//75个字符
        System.Random r = new System.Random();
        string result = string.Empty;
        //生成一个8位长的随机字符，具体长度可以自己更改
        for (int i = 0; i < 8; i++)
        {
            int m = r.Next(0, 60);//这里下界是0，随机数可以取到，上界应该是75，因为随机数取不到上界，也就是最大74，符合我们的题意
            string s = str.Substring(m, 1);
            result += s;
        }
        return result;
    }
    /// <summary>
    /// 创建UI面板
    /// </summary>
    /// <param name="panelType">面板类型</param>
    /// <returns>创建面板的实例</returns>
    public static GameObject CreateUIPanel(PanelType panelType)
    {
        GameObject prefab = Resources.Load<GameObject>(panelType.ToString());
        if (prefab == null)
        {
            Debug.LogWarning("这个 " + panelType.ToString() + " 面板不存在");
            return null;
        }
        else
        {
            GameObject panel = UnityEngine.Object.Instantiate<GameObject>(prefab);
            panel.name = panelType.ToString();
            panel.transform.SetParent(UIParent, false);
            return panel;
        }
    }



    /// <summary>
    /// 用UTF8保存数据，保存玩家和电脑的信息（积分，回合数，胜利局数）
    /// </summary>
    public static void SaveData(GameData data)
    {
//        string fileName = Consts.DataPath;
//
//		Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Write);
//		StreamWriter sw = new StreamWriter(stream, Encoding.UTF8);
//        XmlSerializer xmlSerializer = new XmlSerializer(data.GetType());
//        xmlSerializer.Serialize(sw, data);
//        sw.Close();
//        stream.Close();
		XmlDocument doc = new XmlDocument();
		doc.Load(Consts.DataPath);
		XmlNode root = doc.SelectSingleNode("GameData");
		XmlNodeList nodeList = root.ChildNodes;


        //PlayerIntergration表示玩家积分
        //ComputerLeftIntergration和ComputerRightIntergration表示电脑积分
        foreach (XmlNode node in nodeList)
		{
			if(node.Name == "PlayerIntergration")
			{
				node.InnerText = data.PlayerIntergration.ToString();
			}
			else if(node.Name == "ComputerLeftIntergration")
			{
				node.InnerText = data.ComputerLeftIntergration.ToString();
			}
			else if(node.Name == "ComputerRightIntergration")
			{
				node.InnerText = data.ComputerRightIntergration.ToString();
			}
			else if(node.Name == "Rounds")
			{
				node.InnerText = data.Rounds.ToString ();
			}
			else if (node.Name == "Win") {
				node.InnerText = data.Win.ToString ();
			}
			doc.Save(Consts.DataPath);
		}
    }


    /// <summary>
    /// 获取数据（通过反序列化得到游戏数据）
    /// </summary>
    /// <returns></returns>
    public static GameData GeyDataWithOutBom()
    {
        GameData data = new GameData();
        Stream stream = new FileStream(Consts.DataPath, FileMode.Open, FileAccess.Read, FileShare.None);
        //忽略标记--true
		StreamReader sr = new StreamReader(stream,false);
        XmlSerializer xmlSerializer = new XmlSerializer(data.GetType());

        data = xmlSerializer.Deserialize(sr) as GameData;//反序列化得到游戏数据
        stream.Close();
        sr.Close();

        return data;
    }

    /// <summary>
    /// 卡牌排序
    /// </summary>
    /// <param name="cards">选择的牌</param>
    /// <param name="asc">是否升序</param>
    public static void Sort(List<Card> cards, bool asc)//按照升序或者降序来排列手牌
    {
     
       
        cards.Sort(
            
            (Card a, Card b) =>
            {
                if (asc)
                    return a.CardWeight.CompareTo(b.CardWeight);
                else
                    return -a.CardWeight.CompareTo(b.CardWeight);
            }
            );
    }

    /// <summary>
    /// 获取卡牌的权值
    /// </summary>
    /// <param name="cards">选中的卡牌</param>
    /// <param name="cardType">卡牌类型</param>
    /// <returns>权值</returns>
    public static int GetWeight(List<Card> cards, CardType cardType)
    {
        int totalWeight = 0;
        //过滤三带一、二//3332 2333
        if (cardType == CardType.ThreeAndOne || cardType == CardType.ThreeAndTwo)//三带一或者三带二
        {
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i].CardWeight == cards[i + 1].CardWeight && cards[i].CardWeight == cards[i + 2].CardWeight)
                {
                    totalWeight += (int)cards[i].CardWeight;
                    totalWeight *= 3;
                    break;
                }
            }
        }
        else if (cardType == CardType.Straight || cardType == CardType.DoubleStraight)//顺子或者连对
        {
            totalWeight = (int)cards[0].CardWeight;
        }
        //其余类型 全算权值
        else
        {
            for (int i = 0; i < cards.Count; i++)
            {
                totalWeight += (int)cards[i].CardWeight;
            }
        }
        return totalWeight;
    }


	/// <summary>
	/// 根据人物的积分返回段位
	/// </summary>
	/// <returns>The ranking.</returns>
	/// <param name="intergration">Intergration.</param>
	public static string GetRanking(int intergration)
	{
		if (intergration <= 10)
		{
			return "懵懂出茅庐";
		} 
		else if (intergration <= 100) 
		{
			return "剑气露锋芒";
		}
		else if (intergration <= 500) 
		{
			return "豪情闯天下";
		}
		else
		{
			return "潇洒任我行";
		}
	}



}


