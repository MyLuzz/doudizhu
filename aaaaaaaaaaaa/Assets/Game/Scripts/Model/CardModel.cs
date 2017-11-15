using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 牌库 也就是 一副牌
/// </summary>
public class CardModel
{
    private Queue<Card> cardLibrary = new Queue<Card>();
    private CharacterType cType = CharacterType.Library;

    /// <summary>
    /// 牌库剩余牌的数量
    /// </summary>
    public int CardCount
    {
        get { return cardLibrary.Count; }
    }

    /// <summary>
    /// 初始化牌库(创建54张牌)
    /// </summary>
    //public void InitCardLibrary()
    //{
    //    for (int color = 1; color < 5; color++)
    //    {
    //        for (int weight = 0; weight < 13; weight++)
    //        {
    //            Weight w = (Weight)weight;
    //            Colors c = (Colors)color;
    //            string name = c.ToString() + w.ToString();
    //            Card card = new Card(name, c, w, cType);
    //            cardLibrary.Enqueue(card);
    //        }
    //    }

    //    Card sJoker = new Card("SJoker", Colors.None, Weight.SJoker, cType);
    //    Card bJoker = new Card("LJoker", Colors.None, Weight.LJoker, cType);
    //    cardLibrary.Enqueue(sJoker);
    //    cardLibrary.Enqueue(bJoker);
    //}

    /// <summary>
    /// 发牌
    /// </summary>
    /// <returns></returns>
    //public Card DealCard(CharacterType sendTo)
    //{
    //    Card card = cardLibrary.Dequeue();
    //    card.BelongTo = sendTo;
    //    return card;
    //}

    /// <summary>
    /// 回收牌
    /// </summary>
    /// <param name="card">要回收的卡牌</param>
    //public void RecycleCard(Card card)
    //{
    //    cardLibrary.Enqueue(card);
    //    card.BelongTo = cType;
    //}


    /// <summary>
    /// 洗牌
    /// </summary>
    public void Shuffle()
    {
        List<Card> newList = new List<Card>();
        foreach (Card card in cardLibrary)
        {
            int index = Random.Range(0, newList.Count + 1);
            newList.Insert(index, card);
        }
        cardLibrary.Clear();
        foreach (Card card in newList)
        {
            cardLibrary.Enqueue(card);
        }
        newList.Clear();
    }


}
