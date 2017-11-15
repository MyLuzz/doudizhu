using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 玩家
/// </summary>
public class PlayerControl : CharacterBase
{
    /// <summary>
    /// 角色UI控制
    /// </summary>
    public CharacterUI characterUI;

    private Identity identity;


    /// <summary>
    /// 角色身份
    /// </summary>
    public Identity Identity
    {
        get { return identity; }
        set
        {
            identity = value;
            characterUI.SetIdentity(value);
        }
    }


    /// <summary>
    /// 添加卡牌
    /// </summary>
    /// <param name="card"></param>
    public override void AddCard(Card card, bool selected)
    {
        base.AddCard(card, selected);
        characterUI.SetRemain(CardCount);
    }

    /// <summary>
    /// 出牌
    /// </summary>
    /// <param name="card"></param>
    public override Card DealCard()
    {
        Card card = base.DealCard();
        characterUI.SetRemain(CardCount);
        return card;
    }


    /// <summary>
    /// 排序
    /// </summary>
    /// <param name="asc"></param>
    public override void Sort(bool asc)
    {
        base.Sort(asc);
    }

    //临时保存的卡牌和卡牌UI
    List<CardUI> tempUI = null;
    List<Card> tempCard = null;

    /// <summary>
    /// 寻找选中的手牌
    /// </summary>
    /// <returns></returns>
    public List<Card> FindSelectCard()
    {
        CardUI[] cardUIs = CreatePoint.GetComponentsInChildren<CardUI>();

        tempUI = new List<CardUI>();
        tempCard = new List<Card>();

        for (int i = 0; i < cardUIs.Length; i++)
        {
            if (cardUIs[i].Selected)
            {
                tempUI.Add(cardUIs[i]);
                tempCard.Add(cardUIs[i].Card);
            }
        }
        Tools.Sort(tempCard, true);
        return tempCard;
    }


    /// <summary>
    /// 删除出去的手牌
    /// </summary>
    public void DestroySelectCard()
    {
        if (tempUI == null || tempCard == null)
            return;
        else
        {
            for (int i = 0; i < tempCard.Count; i++)
            {
                tempUI[i].Destroy();
                CardList.Remove(tempCard[i]);
            }
        }
        this.SortCardUI(CardList);
        characterUI.SetRemain(CardCount);
    }

    public void Clear()
    {
        CardList.Clear();

        CardUI[] cards = transform.Find("CreatePoint").GetComponentsInChildren<CardUI>();
        for (int i = 0; i < cards.Length; i++)
            cards[i].Destroy();
    }


}

