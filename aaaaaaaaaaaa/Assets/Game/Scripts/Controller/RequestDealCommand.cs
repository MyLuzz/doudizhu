using strange.extensions.command.impl;
using System;
using UnityEngine;
using System.Collections;
using strange.extensions.context.api;

/// <summary>
/// 请求发牌的命令
/// </summary>
class RequestDealCommand : EventCommand
{
    [Inject]
    public CardModel CardModel { get; set; }

    [Inject(ContextKeys.CONTEXT_VIEW)]
    public GameObject GameRoot { get; set; }

    //public override void Execute()
    //{
    //    CardModel.Shuffle();

    //    //发牌
    //    GameRoot.GetComponent<GameRoot>().StartCoroutine(DeaCard());
    //}


    //IEnumerator DeaCard()
    //{
    //    CharacterType curr = CharacterType.Player;

    //    for (int i = 0; i < 51; i++)
    //    {
    //        if (curr == CharacterType.Desk || curr == CharacterType.Library)
    //            curr = CharacterType.Player;
    //        DealTo(curr);
    //        //换人
    //        curr++;
    //        //等待0.1秒
    //        yield return new WaitForSeconds(0.01f);
    //    }

    //    for (int i = 0; i < 3; i++)
    //    {
    //        DealTo(CharacterType.Desk);
    //    }

    //    //标志着发牌结束
    //    dispatcher.Dispatch(ViewEvent.COMPLETE_DEAL);
    //}


    /// <summary>
    /// 发牌
    /// </summary>
    /// <param name="cType">给谁</param>
    //private void DealTo(CharacterType cType)
    //{
    //    Card card = CardModel.DealCard(cType);
    //    DealCardArgs e = new DealCardArgs(cType, card, false);
    //    dispatcher.Dispatch(CommandEvent.DealCard, e);
    //}

}

