using UnityEngine;
using System.Collections;

/// <summary>
/// 桌面控制
/// </summary>
public class DeskControl : CharacterBase
{
	[Inject]
	public RoundModel RoundModel{ get; set;}

    /// <summary>
    /// 出牌
    /// </summary>
    /// <returns></returns>
    public override Card DealCard()
    {
        return base.DealCard();
    }

    /// <summary>
    /// 添加牌
    /// </summary>
    /// <param name="card"></param>
    /// <param name="selected"></param>
    public override void AddCard(Card card, bool selected)
    {
        base.AddCard(card, selected);
    }

    /// <summary>
    /// 排序
    /// </summary>
    /// <param name="asc"></param>
    public override void Sort(bool asc)
    {
        base.Sort(asc);
    }


    /// <summary>
    /// 清空桌面
    /// </summary>
    public void Clear()
    {
        CardList.Clear();

        CardUI[] cards = transform.Find("CreatePoint").GetComponentsInChildren<CardUI>();
        for (int i = 0; i < cards.Length; i++)
            cards[i].Destroy();
    }
}
