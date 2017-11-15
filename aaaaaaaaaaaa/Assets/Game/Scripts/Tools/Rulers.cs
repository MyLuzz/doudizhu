using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// 游戏规则类
/// </summary>
public static class Rulers
{
    /// <summary>
    /// 是否是单牌
    /// </summary>
    /// <param name="cards">选择的手牌</param>
    /// <returns></returns>
    public static bool IsSingle(List<Card> cards)
    {
        return cards.Count == 1;
    }

    /// <summary>
    /// 判断是否是对儿
    /// </summary>
    /// <param name="cards">选择的手牌</param>
    /// <returns></returns>
    public static bool IsDouble(List<Card> cards)
    {
        if (cards.Count == 2)
        {
            if (cards[0].CardWeight == cards[1].CardWeight)
            {
                return true;
            }
        }

        return false;

    }

    /// <summary>
    /// 是否是顺子
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static bool IsStraight(List<Card> cards)
    {
        if (cards.Count < 5 || cards.Count > 12)
            return false;

        for (int i = 0; i < cards.Count - 1; i++)
        {
            Weight tempWeight = cards[i].CardWeight;
            if (cards[i + 1].CardWeight - tempWeight != 1)
                return false;
            //不能超过A
            if (tempWeight > Weight.One || cards[i + 1].CardWeight > Weight.One)
                return false;
        }

        return true;
    }

    /// <summary>
    /// 是否是双顺
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static bool IsDoubleStraight(List<Card> cards)
    {
        if (cards.Count < 6 || cards.Count % 2 != 0)
            return false;

        for (int i = 0; i < cards.Count - 2; i += 2)
        {
            if (cards[i].CardWeight != cards[i + 1].CardWeight)
                return false;
            if (cards[i + 2].CardWeight - cards[i].CardWeight != 1)
                return false;
            //不能超过A
            if (cards[i].CardWeight > Weight.One || cards[i + 2].CardWeight > Weight.One)
                return false;
        }

        return true;
    }

    /// <summary>
    /// 是否是飞机
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static bool IsTripleStraight(List<Card> cards)
    {
        if (cards.Count < 6 || cards.Count % 3 != 0)
            return false;

        for (int i = 0; i < cards.Count - 3; i += 3)
        {
            if (cards[i].CardWeight != cards[i + 1].CardWeight)
                return false;
            if (cards[i + 2].CardWeight != cards[i + 1].CardWeight)
                return false;
            if (cards[i].CardWeight != cards[i + 2].CardWeight)
                return false;

            if (cards[i + 3].CardWeight - cards[i].CardWeight != 1)
                return false;
            //不能超过A
            if (cards[i].CardWeight > Weight.One || cards[i + 3].CardWeight > Weight.One)
                return false;
        }

        return true;
    }

    /// <summary>
    /// 是否是三不带
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static bool IsThree(List<Card> cards)
    {
        if (cards.Count != 3)
            return false;
        if (cards[0].CardWeight != cards[1].CardWeight)
            return false;
        if (cards[2].CardWeight != cards[1].CardWeight)
            return false;
        if (cards[0].CardWeight != cards[2].CardWeight)
            return false;

        return true;
    }

    /// <summary>
    /// 是否是三带一
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static bool IsThreeAndOne(List<Card> cards)
    {
        if (cards.Count != 4)
            return false;

        if (cards[0].CardWeight == cards[1].CardWeight && cards[1].CardWeight == cards[2].CardWeight)
            return true;
        else if (cards[1].CardWeight == cards[2].CardWeight && cards[2].CardWeight == cards[3].CardWeight)
            return true;

        return false;
    }

    /// <summary>
    /// 是否是三带二
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static bool IsThreeAndTwo(List<Card> cards)
    {
        if (cards.Count != 5)
            return false;

        if (cards[0].CardWeight == cards[1].CardWeight && cards[1].CardWeight == cards[2].CardWeight)
        {
            if (cards[3].CardWeight == cards[4].CardWeight)
                return true;
        }
        else if (cards[2].CardWeight == cards[3].CardWeight && cards[3].CardWeight == cards[4].CardWeight)
        {
            if (cards[0].CardWeight == cards[1].CardWeight)
                return true;
        }

        return false;
    }

    /// <summary>
    /// 判断是否是炸弹
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static bool IsBoom(List<Card> cards)
    {
        if (cards.Count != 4)
            return false;
        if (cards[0].CardWeight != cards[1].CardWeight)
            return false;
        if (cards[1].CardWeight != cards[2].CardWeight)
            return false;
        if (cards[2].CardWeight != cards[3].CardWeight)
            return false;

        return true;
    }

    /// <summary>
    /// 判断是不是王炸
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public static bool IsJokerBoom(List<Card> cards)
    {
        if (cards.Count != 2)
            return false;

        if (cards[0].CardWeight == Weight.SJoker && cards[1].CardWeight == Weight.LJoker)
            return true;
        else if (cards[0].CardWeight == Weight.LJoker && cards[1].CardWeight == Weight.SJoker)
            return true;

        return false;
    }

    /// <summary>
    /// 判断能否出牌
    /// </summary>
    /// <param name="cards">要出的牌</param>
    /// <param name="type">出牌类型</param>
    /// <returns>能否出牌</returns>
    public static bool CanPop(List<Card> cards, out CardType type)
    {
        type = CardType.None;
        bool can = false;

        switch (cards.Count)
        {
            case 1:
                if (IsSingle(cards))
                {
                    type = CardType.Single;
                    can = true;
                }
                break;
            case 2:
                if (IsDouble(cards))
                {
                    type = CardType.Double;
                    can = true;
                }
                else if (IsJokerBoom(cards))
                {
                    type = CardType.JokerBoom;
                    can = true;
                }
                break;
            case 3:
                if (IsThree(cards))
                {
                    type = CardType.Three;
                    can = true;
                }
                break;
            case 4:
                if (IsBoom(cards))
                {
                    type = CardType.Boom;
                    can = true;
                }
                else if (IsThreeAndOne(cards))
                {
                    type = CardType.ThreeAndOne;
                    can = true;
                }
                break;
            case 5:
                if (IsStraight(cards))
                {
                    type = CardType.Straight;
                    can = true;
                }
                else if (IsThreeAndTwo(cards))
                {
                    type = CardType.ThreeAndTwo;
                    can = true;
                }
                break;
            case 6:
                if (IsStraight(cards))
                {
                    type = CardType.Straight;
                    can = true;
                }
                else if (IsDoubleStraight(cards))
                {
                    type = CardType.DoubleStraight;
                    can = true;
                }
                else if (IsTripleStraight(cards))
                {
                    type = CardType.TripleStraight;
                    can = true;
                }
                break;
            case 7:
                if (IsStraight(cards))
                {
                    type = CardType.Straight;
                    can = true;
                }
                break;
            case 8:
                if (IsStraight(cards))
                {
                    type = CardType.Straight;
                    can = true;
                }
                else if (IsDoubleStraight(cards))
                {
                    type = CardType.DoubleStraight;
                    can = true;
                }
                break;
            case 9:
                if (IsStraight(cards))
                {
                    type = CardType.Straight;
                    can = true;
                }//777 888 999 
                else if (IsTripleStraight(cards))
                {
                    type = CardType.TripleStraight;
                    can = true;
                }
                break;
            case 10:
                if (IsStraight(cards))
                {
                    type = CardType.Straight;
                    can = true;
                }
                else if (IsDoubleStraight(cards))
                {
                    type = CardType.DoubleStraight;
                    can = true;
                }
                break;
            case 11:
                if (IsStraight(cards))
                {
                    type = CardType.Straight;
                    can = true;
                }
                break;
            case 12:
                if (IsStraight(cards))
                {
                    type = CardType.Straight;
                    can = true;
                }
                else if (IsDoubleStraight(cards))
                {
                    type = CardType.DoubleStraight;
                    can = true;
                }// 444 555 666 777
                else if (IsTripleStraight(cards))
                {
                    type = CardType.TripleStraight;
                    can = true;
                }
                break;
            case 13:
                break;
            case 14:
                if (IsDoubleStraight(cards))
                {
                    type = CardType.DoubleStraight;
                    can = true;
                }
                break;
            case 15:
                if (IsTripleStraight(cards))
                {
                    type = CardType.TripleStraight;
                    can = true;
                }
                break;
            case 16:
                if (IsDoubleStraight(cards))
                {
                    type = CardType.DoubleStraight;
                    can = true;
                }
                break;
            case 17:
                break;
            case 18:
                if (IsDoubleStraight(cards))
                {
                    type = CardType.DoubleStraight;
                    can = true;
                }// 444 555 666 777 888 999 
                else if (IsTripleStraight(cards))
                {
                    type = CardType.TripleStraight;
                    can = true;
                }
                break;
            case 19:
                break;
            case 20:
                if (IsDoubleStraight(cards))
                {
                    type = CardType.DoubleStraight;
                    can = true;
                }
                break;
            default:
                break;
        }
        return can;
    }
}

