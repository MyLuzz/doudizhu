using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 控制电脑自动出牌
/// </summary>
public class ComputerAI : MonoBehaviour
{
    /// <summary>
    /// 当前的出牌类型
    /// </summary>
    public CardType currType = CardType.None;
    /// <summary>
    /// 当前出的牌
    /// </summary>
    public List<Card> selectCards = new List<Card>();

    /// <summary>
    /// 自动出牌
    /// </summary>
    /// <param name="cards">所有卡牌</param>
    /// <param name="cardType">上一轮出牌类型</param>
    /// <param name="weight">上一轮卡牌权值</param>
    /// <param name="length">上一轮卡牌长度</param>
    /// <param name="isBiggest">是否最大权值</param>
    public void SmartSelectCards(List<Card> cards, CardType cardType, int weight, int length, bool isBiggest)
    {
        cardType = isBiggest ? CardType.None : cardType;
        currType = cardType;
        selectCards.Clear();
        switch (cardType)
        {
            case CardType.None:
                selectCards = FindSmallestCard(cards);
                //已经修改了当前的出牌类型
                break;
            case CardType.Single:
                selectCards = FindSingle(cards, weight);
                break;
            case CardType.Double:
                selectCards = FindDouble(cards, weight);
                break;
            case CardType.Straight:
                selectCards = FindStraight(cards, weight, length);
                if (selectCards.Count == 0)
                {
                    selectCards = FindBoom(cards, -1);
                    currType = CardType.Boom;
                    if (selectCards.Count == 0)
                    {
                        selectCards = FindJokerBoom(cards);
                        currType = CardType.JokerBoom;
                    }
                }
                break;
            case CardType.DoubleStraight:
                selectCards = FindDoubleStraight(cards, weight, length);
                if (selectCards.Count == 0)
                {
                    selectCards = FindBoom(cards, -1);
                    currType = CardType.Boom;
                    if (selectCards.Count == 0)
                    {
                        selectCards = FindJokerBoom(cards);
                        currType = CardType.JokerBoom;
                    }
                }
                break;
            case CardType.TripleStraight:
                selectCards = FindBoom(cards, -1);
                currType = CardType.Boom;
                if (selectCards.Count == 0)
                {
                    selectCards = FindJokerBoom(cards);
                    currType = CardType.JokerBoom;
                }
                break;
            case CardType.Three:
                selectCards = FindThree(cards, weight);
                break;
            case CardType.ThreeAndOne:
                selectCards = FindThreeAndOne(cards, weight);
                break;
            case CardType.ThreeAndTwo:
                selectCards = FindThreeAndTwo(cards, weight);
                break;
            case CardType.Boom:
                selectCards = FindBoom(cards, weight);
                if (selectCards.Count == 0)
                {
                    selectCards = FindJokerBoom(cards);
                    currType = CardType.JokerBoom;
                }
                break;
            case CardType.JokerBoom:
                //王炸是无敌的
                break;
            default:
                break;
        }
    }

    #region 帮助方法
    /// <summary>
    /// 找最小的牌
    /// </summary>
    /// <returns></returns>
    private List<Card> FindSmallestCard(List<Card> cards)
    {
        List<Card> select = new List<Card>();
        //先出顺子
        for (int i = 12; i >= 5; i--)
        {
            select = FindStraight(cards, -1, i);
            if (select.Count != 0)
            {
                currType = CardType.Straight;
                break;
            }
        }
        if (select.Count == 0)
        {
            //找三代二
            for (int i = 0; i < 36; i += 3)
            {
                select = FindThreeAndTwo(cards, i - 1);
                if (select.Count != 0)
                {
                    currType = CardType.ThreeAndTwo;
                    break;
                }
            }
        }
        if (select.Count == 0)
        {
            //找三带一
            for (int i = 0; i < 36; i += 3)
            {
                select = FindThreeAndOne(cards, i - 1);
                if (select.Count != 0)
                {
                    currType = CardType.ThreeAndOne;
                    break;
                }
            }
        }
        if (select.Count == 0)
        {
            //三不带
            for (int i = 0; i < 36; i += 3)
            {
                select = FindThree(cards, i - 1);
                if (select.Count != 0)
                {
                    currType = CardType.Three;
                    break;
                }
            }
        }
        if (select.Count == 0)
        {
            //对儿
            for (int i = 0; i < 24; i += 2)
            {
                select = FindDouble(cards, i - 1);
                if (select.Count != 0)
                {
                    currType = CardType.Double;
                    break;
                }
            }
        }
        if (select.Count == 0)
        {
            //单牌
            select = FindSingle(cards, -1);
            currType = CardType.Single;
        }
        return select;
    }

    /// <summary>
    /// 找单牌
    /// </summary>
    private List<Card> FindSingle(List<Card> cards, int weight)
    {
        List<Card> select = new List<Card>();
        for (int i = 0; i < cards.Count; i++)
        {
            if ((int)cards[i].CardWeight > weight)
            {
                select.Add(cards[i]);
                break;
            }
        }
        return select;
    }

    /// <summary>
    /// 找对儿
    /// </summary>
    private List<Card> FindDouble(List<Card> cards, int weight)
    {
        List<Card> select = new List<Card>();
        for (int i = 0; i < cards.Count - 1; i++)
        {
            if (cards[i].CardWeight == cards[i + 1].CardWeight)
            {
                int totalWeight = (int)cards[i].CardWeight + (int)cards[i + 1].CardWeight;

                if (totalWeight > weight)
                {
                    select.Add(cards[i]);
                    select.Add(cards[i + 1]);
                    break;
                }
            }
        }
        return select;
    }

    /// <summary>
    /// 找顺子
    /// </summary>
    private List<Card> FindStraight(List<Card> cards, int minWeight, int length)
    {
        List<Card> select = new List<Card>();
        int counter = 1;
        List<int> indexList = new List<int>();
        for (int i = 0; i < cards.Count - 4; i++)
        {
            int weight = (int)cards[i].CardWeight;
            //56789  678910
            if (weight > minWeight)
            {
                counter = 1;
                indexList.Clear();

                for (int j = i + 1; j < cards.Count; j++)
                {
                    if (cards[j].CardWeight > Weight.One)
                        break;

                    if ((int)cards[j].CardWeight - weight == counter)
                    {
                        counter++;
                        indexList.Add(j);
                    }

                    if (counter == length)
                        break;
                }
            }
            if (counter == length)
            {
                indexList.Insert(0, i);
                break;
            }

        }

        if (counter == length)
        {
            for (int i = 0; i < indexList.Count; i++)
            {
                select.Add(cards[indexList[i]]);
            }
        }

        return select;
    }

    /// <summary>
    /// 找双顺
    /// </summary>
    private List<Card> FindDoubleStraight(List<Card> cards, int minWeight, int length)
    {
        List<Card> select = new List<Card>();
        int counter = 0;
        List<int> indexList = new List<int>();
        for (int i = 0; i < cards.Count - 4; i++)
        {
            int weight = (int)cards[i].CardWeight;
            if (weight > minWeight)
            {
                counter = 0;
                indexList.Clear();

                int temp = 0;
                for (int j = i + 1; j < cards.Count; j++)
                {
                    if (cards[j].CardWeight > Weight.One)
                        break;

                    if ((int)cards[j].CardWeight - weight == counter)
                    {
                        temp++;
                        if (temp % 2 == 1)
                        {
                            counter++;
                        }
                        indexList.Add(j);
                    }

                    if (counter == length / 2)
                        break;
                }
            }

            if (counter == length / 2)
            {
                indexList.Insert(0, i);
                break;
            }

        }

        if (counter == length / 2)
        {
            for (int i = 0; i < indexList.Count; i++)
            {
                select.Add(cards[indexList[i]]);
            }
        }

        return select;
    }

    /// <summary>
    /// 找三不带
    /// </summary>
    private List<Card> FindThree(List<Card> cards, int weight)
    {
        List<Card> select = new List<Card>();
        for (int i = 0; i < cards.Count - 3; i++)
        {
            if (cards[i].CardWeight == cards[i + 1].CardWeight &&
                cards[i].CardWeight == cards[i + 2].CardWeight)
            {
                int totalWeight = (int)cards[i].CardWeight +
                    (int)cards[i + 1].CardWeight +
                    (int)cards[i + 2].CardWeight;

                if (totalWeight > weight)
                {
                    select.Add(cards[i]);
                    select.Add(cards[i + 1]);
                    select.Add(cards[i + 2]);
                    break;
                }
            }
        }
        return select;
    }

    /// <summary>
    /// 找三代二
    /// </summary>
    private List<Card> FindThreeAndTwo(List<Card> cards, int weight)
    {
        List<Card> select = new List<Card>();
        List<Card> three = FindThree(cards, weight);

        if (three.Count != 0)
        {
            foreach (Card card in three)
                cards.Remove(card);

            List<Card> two = FindDouble(cards, -1);
            if (two.Count != 0)
            {
                select.AddRange(three);
                select.AddRange(two);
            }
        }

        return select;

    }

    /// <summary>
    /// 找三带一
    /// </summary>
    private List<Card> FindThreeAndOne(List<Card> cards, int weight)
    {
        List<Card> select = new List<Card>();
        List<Card> three = FindThree(cards, weight);
        if (three.Count != 0)
        {
            foreach (Card card in three)
                cards.Remove(card);

            List<Card> one = FindSingle(cards, -1);
            if (one.Count != 0)
            {
                select.AddRange(three);
                select.AddRange(one);
            }
        }
        return select;
    }

    /// <summary>
    /// 找炸弹
    /// </summary>
    /// <param name="weight"></param>
    /// <returns></returns>
    private List<Card> FindBoom(List<Card> cards, int weight)
    {
        List<Card> select = new List<Card>();
        for (int i = 0; i < cards.Count - 4; i++)
        {
            //先找普通炸弹
            if (cards[i].CardWeight == cards[i + 1].CardWeight &&
                cards[i].CardWeight == cards[i + 2].CardWeight &&
                cards[i].CardWeight == cards[i + 3].CardWeight)
            {
                int totalWeight = (int)cards[i].CardWeight + (int)cards[i + 1].CardWeight + (int)cards[i + 2].CardWeight
                    + (int)cards[i + 4].CardWeight;
                if (totalWeight > weight)
                {
                    select.Add(cards[i]);
                    select.Add(cards[i + 1]);
                    select.Add(cards[i + 2]);
                    select.Add(cards[i + 3]);
                    break;
                }
            }
        }
        return select;
    }

    /// <summary>
    /// 找王炸
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    private List<Card> FindJokerBoom(List<Card> cards)
    {
        List<Card> select = new List<Card>();
        for (int i = 0; i < cards.Count - 1; i++)
        {
            if (cards[i].CardWeight == Weight.SJoker &&
                cards[i + 1].CardWeight == Weight.LJoker)
            {
                select.Add(cards[i]);
                select.Add(cards[i + 1]);
                break;
            }
        }
        return select;
    }
    #endregion
}
