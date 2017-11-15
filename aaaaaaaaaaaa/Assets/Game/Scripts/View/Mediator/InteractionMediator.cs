using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class InteractionMediator : EventMediator
{
    [Inject]
    public InteractionView InteractionView { get; set; }

	[Inject]
	public RoundModel RoundModel{ get; set;}

    //public int sign = 0;
   // public int times = 0;   

	public byte[] message;

    public byte[] message1;

    public byte[] leave_message;

    public byte[] pass_message;

    public byte[] disgrab_message;

    public byte[] grab_message;

    public byte[] score_message;

    public byte[] agree_message;
    public byte[] disagree_message;

   public static bool canp = false;

    public static List<Card> currList;
    //用来将自动出的牌的选择状态变为选中
    public static List<Card> selectList;

   // public static int callNum = 0;

    public static bool canGrab = true;

    public bool over = false;

    public AudioSource bgm;

    public byte[] uncon;
    void Update()
    {
        if (NetManager.refresh_0)
        {
            GameObject.Find("Start").GetComponent<Transform>().GetChild(0).gameObject.SetActive(true);
            NetManager.refresh_0 = false;

        }
        if (NetManager.refresh_1 )
        {
            GameObject.Find("Start").GetComponent<Transform>().GetChild(0).gameObject.SetActive(false);
            NetManager.refresh_1 = false;
            if (NetManager.readylist[NetManager.index] == true)
            {
                InteractionView.btn_Deal.gameObject.SetActive(false);
            }
            NetManager.sign_reconscore = true;
           
        }
        if (NetManager.refresh_2)
        {
            GameObject.Find("Start").GetComponent<Transform>().GetChild(0).gameObject.SetActive(false);
            InteractionView.btn_Deal.gameObject.SetActive(false);
            NetManager.refresh_2 = false;
            if (InteractionView.Player.CardCount == 0)
            {
                StartCoroutine(wait());
            }
            NetManager.sign_reconscore = true;
        }

        //刷新场上的牌
        if (NetManager.refresh_3)
        {
            if (InteractionView.Player.CardCount == 0)
            {
                if (NetManager.lastPlayer == -1 || NetManager.external_card_list == null)
                {
                    canp = false;
                    InteractionView.ActivePlayAndPass(canp);
                }
                if (NetManager.landlord == NetManager.index)
                {
                    Image img = Resources.Load<Image>("Identity_Landlord");
                    Image img_Identity = GameObject.Find("Player_Identity").GetComponent<Image>();
                    img_Identity.sprite = img.sprite;
                }
                //重连返回没有倍数 默认一倍
                NetManager.multiple = NetManager.maxBet;
                GameObject.Find("Start").GetComponent<Transform>().GetChild(0).gameObject.SetActive(false);
                InteractionView.btn_Deal.gameObject.SetActive(false);
                Card lcard = new Card("ClubEight", Colors.Club, Weight.Eight, CharacterType.ComputerLeft);
                Card rcard = new Card("ClubEight", Colors.Club, Weight.Eight, CharacterType.ComputerRight);
                for (int i = 0; i < NetManager.cardlist.Count; i++)
                {
                    InteractionView.Player.AddCard(NetManager.cardlist[i], false);
                }
                for (int j = 0; j < NetManager.nextcardnum; j++)
                {
                    InteractionView.Left.AddCard(lcard, false);
                }
                for (int k = 0; k < NetManager.lastcardnum; k++)
                {
                    InteractionView.Right.AddCard(rcard, false);
                }
                InteractionView.Player.Sort(false);
            }
            ReconPlay();
            NetManager.sign_reconscore = true;
        }

        if (NetManager.sign_reconscore)
        {
            InteractionView.Player.characterUI.txt_Round.text = "积分：" + NetManager.reconscorelist[NetManager.index].ToString();
            InteractionView.Left.characterUI.txt_Round.text = "积分：" + NetManager.reconscorelist[Tools.NextIndex(NetManager.index)].ToString();
            InteractionView.Right.characterUI.txt_Round.text = "积分：" + NetManager.reconscorelist[Tools.LastIndex(NetManager.index)].ToString();
            NetManager.sign_reconscore = false;
        }
        //投票解散房间
        if (NetManager.sign_offline)
        {
            InteractionView.btn_vote.gameObject.SetActive(true);
            NetManager.sign_offline = false;
        }
        if (NetManager.sign_off_recon)
        {
            InteractionView.btn_vote.gameObject.SetActive(false);
            NetManager.sign_off_recon = false;
        }
        //解散房间 刷新返回大厅 刷新房卡
        if (NetManager.sign_closeroom)
        {
            Debug.Log("离线玩家座位号" + NetManager.offline_pos);
            Debug.Log("你的座位号" + NetManager.index);
            if (NetManager.offline_pos == NetManager.index)
            {
                InteractionView.roomclose_tip_panel.gameObject.SetActive(true);
            }
            else
            {
                DismissRoom();
                InteractionView.ShowRoomCloseAnim();
            }
            
            NetManager.sign_closeroom = false;
        }
        if (NetManager.readylist[0] == true && NetManager.readylist[1] == true && NetManager.readylist[2] == true&&NetManager.create)
        {
            //游戏开始清空
            NetManager.current_card_list = null;
            canp = false;
            ThreeReady();
            //三人都准备后即将局数减一
            NetManager.remian_nning--;
        }
        if (NetManager.canCall)
        {
            callScore();
            canGrab = true;
        }
        if (NetManager.sign_setscore)
        {
            setScore();
            setMultiple();
            setCallIndex();
            NetManager.sign_setscore = false;
        }
        //游戏开始
        if (NetManager.game_begin)
        {
            GameBegin();
         }
        //游戏进行过程中
        if (!NetManager.game_over)
        {
            //打过程中 重连非在自己出牌隐藏按钮
            if (NetManager.recon_deact)
            {
                InteractionView.DeactiveAll();
                NetManager.recon_deact = false;
            }
            //重连的时候修改一次 局数？/？ 显示
            if (NetManager.sign_setRound)
            {
                  InteractionView.Remain_Inning.text = (NetManager.Inning  - NetManager.remian_nning) + "/" + (NetManager.Inning);
                  NetManager.sign_setRound = false;
            }
            GamePlay();
        }
        //游戏结束
        if (NetManager.game_over&&NetManager.over&& NetManager.sign_over)
        {
            InteractionView.txt_multiple.text = "";
            GameOver();
        }
    }
 
    public void DismissRoom()
    {
        NetManager.cardlist.Clear();
        NetManager.playerNum = 1;
        NetManager.setOne = true;
        NetManager.current_card_list = null;
        //轮盘隐藏
        InteractionView.turn_index.SetActive(false);
        InteractionView.show_turn.SetActive(false);
        InteractionView.DeactiveAll();

        InteractionView.btn_Deal.gameObject.SetActive(true);
        NetManager.left_score = 0;
        NetManager.left_lan = 0;
        NetManager.right_score = 0;
        NetManager.right_lan = 0;
        NetManager.player_score = 0;
        NetManager.player_lan = 0;
        NetManager.change_score = true;
        NetManager.readylist = new List<bool> { false, false, false };
        NetManager.setReady = true;
        NetManager.create = true;
        Image img = Resources.Load<Image>("Identity_Farmer");
        NetManager.current_card_list = null;
        Image player = GameObject.Find("Player_Identity").GetComponent<Image>();
        player.sprite = img.sprite;
        Image left = GameObject.Find("Right_Identity").GetComponent<Image>();
        left.sprite = img.sprite;
        Image right = GameObject.Find("Left_Identity").GetComponent<Image>();
        right.sprite = img.sprite;
        InteractionView.Desk.Clear();
        InteractionView.Player.Clear();
        InteractionView.Left.Clear();
        InteractionView.Right.Clear();
        NetManager.canCall = false;
        NetManager.nextPlayer = -1;
        NetManager.turn = -1;
        NetManager.maxBet = 0;
        NetManager.baojing1 = true;
        NetManager.baojing2 = true;
        NetManager.first_play = true;
        NetManager.multiple = 1;
        NetManager.currPlayer = NetManager.index;
        GameObject.Find("Start").GetComponent<Transform>().GetChild(0).gameObject.SetActive(true);
        InteractionView.soundManager.mc_welcome.Play();
        //AA 
        if (NetManager.payment == 1)
        {
            int pay_card = (NetManager.Inning - NetManager.remian_nning) * 20;
            NetManager.roomcard = NetManager.roomcard - pay_card;
            NetManager.setRoomCard = true;
        }
        //房主支付
        else if (NetManager.payment == 0)
        {
            if (NetManager.index == NetManager.host_num)
            {
                int pay_card = (NetManager.Inning - NetManager.remian_nning) * 60;
                NetManager.roomcard = NetManager.roomcard - pay_card;
                NetManager.setRoomCard = true;
            }
        }
        InteractionView.roomclose_tip_panel.gameObject.SetActive(false);
    }
    public void DealCard()
    {
        if (NetManager.current_card_list != null)
        {
          
            //存储卡牌的名称
            string mc_name = "";
            InteractionView.Desk.Clear();
            currList = Tools.getCardList(NetManager.current_card_list);
            

            CardType cardType;
            Rulers.CanPop(currList, out cardType);
            RoundModel.Length = currList.Count;
            RoundModel.CardType = cardType;

            RoundModel.Weight = Tools.GetWeight(currList, cardType);
           ///炸弹X2 王炸X3
            if (cardType == CardType.Boom)
            {
                NetManager.multiple *= 2;
            }
            else if(cardType == CardType.JokerBoom)
            {
                NetManager.multiple *= 3;
            }
            setMultiple();
            if (cardType == CardType.Single)
            {
                mc_name = cardType.ToString() + Tools.getWeight(RoundModel.Weight).ToString();
            }
            else if (cardType == CardType.Double)
            {
                mc_name = cardType.ToString() + Tools.getDoubleWeight(RoundModel.Weight).ToString();
            }
            else if (cardType == CardType.Three)
            {
                mc_name = cardType.ToString() + Tools.getThreeWeight(RoundModel.Weight).ToString();
            }
            else
                mc_name = cardType.ToString();
            bgm =  GameObject.Find(mc_name).GetComponent<AudioSource>();
            bgm.Play();
           // InteractionView.Desk.setDesk_Pos(-NetManager.current_card_list.Count / 2);

            Debug.Log("场上的牌的数量" + NetManager.current_card_list.Count);
            InteractionView.Desk.CreatePoint.Translate(Vector3.left * 25 * NetManager.current_card_list.Count);
            NetManager.current_card_list = null;
            foreach (Card card in currList)
            {
                setCardCount();
                InteractionView.Desk.AddCard(card, false);
            }
            InteractionView.Desk.CreatePoint.position = InteractionView.standrand.position;
            //报警音效提示
            StartCoroutine(WaitForBaojing());  
        }
    }
    /// <summary>
    /// 打牌阶段的重连
    /// </summary>
    public void ReconPlay()
    {
        NetManager.refresh_3 = false;
        CardUI[] cardUIs = InteractionView.Player.CreatePoint.GetComponentsInChildren<CardUI>();
        for (int i = 0; i < cardUIs.Length; i++)
        {
            for (int j = 0; j < NetManager.recon_cardlist.Count; j++)
            {
                if (cardUIs[i].Card.CardName == NetManager.recon_cardlist[j].CardName)
                {
                    cardUIs[i].Selected = true;
                }
            }
        }
        for (int k = 0; k < cardUIs.Length; k++)
        {
            cardUIs[k].Selected = !cardUIs[k].Selected;
        }
        InteractionView.Player.FindSelectCard();
        InteractionView.Player.DestroySelectCard();
        InteractionView.Player.Sort(false);
        //更新其他两个玩家的UI
        while (InteractionView.Left.CardCount > NetManager.nextcardnum)
        {
            InteractionView.Left.DealCard();
        }
        while (InteractionView.Right.CardCount > NetManager.lastcardnum)
        {
            InteractionView.Right.DealCard();
        }
    }
    public void ThreeReady()
    {
        //开始exciting 音乐
        InteractionView.soundManager.mc_normal.Stop();
        if (NetManager.bgm_open)
            InteractionView.soundManager.mc_exciting.Play();
        NetManager.pos = 2;

        //局数显示
        InteractionView.RoundShowAnim();
        InteractionView.Remain_Inning.text = (NetManager.Inning + 1 - NetManager.remian_nning) + "/" + (NetManager.Inning);
        if (NetManager.remian_nning <= 0)
        {
            InteractionView.Remain_Inning.text = "";
        }
        StartCoroutine(wait());
        NetManager.create = false;
        GameObject.Find("Player").GetComponent<Transform>().GetChild(2).gameObject.SetActive(false);
        GameObject.Find("ComputerLeft").GetComponent<Transform>().GetChild(2).gameObject.SetActive(false);
        GameObject.Find("ComputerRight").GetComponent<Transform>().GetChild(2).gameObject.SetActive(false);

        //设置一次叫分指示器
        setCallIndex();
    }
    public void GameBegin()
    {
        RoundModel.Biggest = CharacterType.Player;
        StartCoroutine(close_extra_anim());
        if (NetManager.index == 0)
        {
            if (NetManager.landlord == 1)
            {
                NetManager.left_lan++;
            }
            else if (NetManager.landlord == 2)
            {
                NetManager.right_lan++;
            }
        }
        if (NetManager.index == 1)
        {
            if (NetManager.landlord == 2)
            {
                NetManager.left_lan++;
            }
            else if (NetManager.landlord == 0)
            {
                NetManager.right_lan++;
            }
        }
        if (NetManager.index == 2)
        {
            if (NetManager.landlord == 0)
            {
                NetManager.left_lan++;
            }
            else if (NetManager.landlord == 1)
            {
                NetManager.right_lan++;
            }
        }
        setTurnIndexOnce();
        //恢复倍数按钮禁用状态
        InteractionView.btn_Grab1.interactable = true;
        InteractionView.btn_Grab2.interactable = true;
        InteractionView.btn_Grab3.interactable = true;
        Deactivex0x1x2();
        NetManager.sign_over = true;
        setLandlord();
        InteractionView.DeactiveAll();
        Debug.Log("游戏开始");
        for (int i = 0; i < 3; i++)
        {
            setThreeCardCount();
        }
        if (NetManager.landlord == NetManager.index)
        {
            NetManager.player_lan++;
            Image img = Resources.Load<Image>("Identity_Landlord");
            Image img_Identity = GameObject.Find("Player_Identity").GetComponent<Image>();
            img_Identity.sprite = img.sprite;
           
            InteractionView.ActivePlayAndPass(false);
            canp = true;
            Debug.Log("发底牌");
            List<Card> grabList = Tools.getCardList(NetManager.external_card_list);
            for (int i = 0; i < 3; i++)
            {
                Debug.Log("加牌");
                InteractionView.Player.AddCard(grabList[i], true);
            }
            InteractionView.Player.Sort(false);
            //防止意外隐藏底牌图片
            CanvasGroup cg_card1 = InteractionView.extra_card1.GetComponent<CanvasGroup>();
            CanvasGroup cg_card2 = InteractionView.extra_card2.GetComponent<CanvasGroup>();
            CanvasGroup cg_card3 = InteractionView.extra_card3.GetComponent<CanvasGroup>();
            cg_card1.alpha = 0;
            cg_card2.alpha = 0;
            cg_card3.alpha = 0;
        }

        NetManager.game_begin = false;
        NetManager.game_over = false;
    }
    public void GamePlay()
    {
        //改变一次轮盘的指示器位置
        if (NetManager.sign_turn_index)
        {
            setTurnIndex();
            NetManager.sign_turn_index = false;
        }
        setBigget();
        setIndex();

        if (NetManager.index == NetManager.nextPlayer)
        {
            NetManager.nextPlayer = -1;

            if (RoundModel.Biggest == CharacterType.Player)
            {
                canp = false;
            }
            else
            {
                canp = true;
            }
            InteractionView.ActivePlayAndPass(canp);
        }
        DealCard();
        if (NetManager.someone_pass)
        {
            GameObject.Find("Woman_buyao1").GetComponent<AudioSource>().Play();
            NetManager.someone_pass = false;
        }    
    }
    public void GameOver()
    {
        //隐藏轮盘
        InteractionView.turn_index.SetActive(false);
        InteractionView.show_turn.SetActive(false);
        InteractionView.DeactiveAll();
        DealCard();
        score_message = EncodeTool.ScoreEncode(NetManager.account);
        NetManager.client.Send(score_message);
        NetManager.sign_over = false;
        StartCoroutine(WaitForScore());
    }
    IEnumerator WaitForBaojing()
    { 
        yield return new WaitForSeconds(1.0f);
        if (InteractionView.Player.CardCount == 2 || InteractionView.Left.CardCount == 2 || InteractionView.Right.CardCount == 2)
        {
            if (NetManager.baojing2) 
            {
                InteractionView.soundManager.mc_baojing2.Play();
            }
            NetManager.baojing2 = false;
        }
        if (InteractionView.Player.CardCount == 1 || InteractionView.Left.CardCount == 1 || InteractionView.Right.CardCount == 1)
        {

            if (NetManager.baojing1)
            {
                InteractionView.soundManager.mc_baojing1.Play();
            }
            NetManager.baojing1 = false;
        }
    }
    IEnumerator WaitForScore()
    {
        yield return new WaitForSeconds(0.5f);
        //播放失败或者胜利的音乐
        if (NetManager.scoreList[NetManager.index] < 0)
        {
            InteractionView.soundManager.mc_exciting.Stop();
            InteractionView.soundManager.mc_lose.Play();
        }
        else
        {
            InteractionView.soundManager.mc_exciting.Stop();
            InteractionView.soundManager.mc_win.Play();
        }
        dispatcher.Dispatch(CommandEvent.GameOver);
    }
    public void Deactivex0x1x2()
    {
        InteractionView.player_x0.gameObject.SetActive(false);
        InteractionView.player_x1.gameObject.SetActive(false);
        InteractionView.player_x2.gameObject.SetActive(false);
        InteractionView.left_x0.gameObject.SetActive(false);
        InteractionView.left_x1.gameObject.SetActive(false);
        InteractionView.left_x2.gameObject.SetActive(false);
        InteractionView.right_x0.gameObject.SetActive(false);
        InteractionView.right_x1.gameObject.SetActive(false);
        InteractionView.right_x2.gameObject.SetActive(false);
    }
    //地主指示器
    public void setCallIndex()
    {

        //轮盘显示
        InteractionView.turn_index.SetActive(true);
        InteractionView.show_turn.SetActive(true);
        if (NetManager.index == 0)
        {
            if (NetManager.turn == 0)
            {
                NetManager.rot.z = 90;
            }
            else if (NetManager.turn == 1)
            {
                NetManager.rot.z = 300;
            }
            else if (NetManager.turn == 2)
            {
                NetManager.rot.z = 210;
            }
        }
        if (NetManager.index == 1)
        {
            if (NetManager.turn == 1)
            {
                NetManager.rot.z = 90;
            }
            else if (NetManager.turn == 2)
            {
                NetManager.rot.z = 300;
            }
            else if (NetManager.turn == 0)
            {
                NetManager.rot.z = 210;
            }
        }
        if (NetManager.index == 2)
        {
            if (NetManager.turn == 2)
            {
                NetManager.rot.z = 90;
            }
            else if (NetManager.turn == 0)
            {
                NetManager.rot.z = 300;
            }
            else if (NetManager.turn == 1)
            {
                NetManager.rot.z = 210;
            }
        }
        InteractionView.turn_index.GetComponent<Transform>().rotation = Quaternion.Euler(NetManager.rot);
   }
    //叫分指示器
    public void setTurnIndexOnce()
    {
        if (NetManager.index == 0)
        {
            if (NetManager.landlord == 0)
            {
                NetManager.rot.z = 90;
            }
            else if (NetManager.landlord == 1)
            {
                NetManager.rot.z = 300;
            }
            else if (NetManager.landlord == 2)
            {
                NetManager.rot.z = 210;
            }
        }
        if (NetManager.index == 1)
        {
            if (NetManager.landlord == 1)
            {
                NetManager.rot.z = 90;
            }
            else if (NetManager.landlord == 2)
            {
                NetManager.rot.z = 300;
            }
            else if (NetManager.landlord == 0)
            {
                NetManager.rot.z = 210;
            }
        }
        if (NetManager.index == 2)
        {
            if (NetManager.landlord == 2)
            {
                NetManager.rot.z = 90;
            }
            else if (NetManager.landlord == 0)
            {
                NetManager.rot.z = 300;
            }
            else if (NetManager.landlord == 1)
            {
                NetManager.rot.z = 210;
            }
        }
        InteractionView.turn_index.GetComponent<Transform>().rotation = Quaternion.Euler(NetManager.rot);
    }
    public void setTurnIndex()
    {

        if (NetManager.index == 0)
        {
            if (NetManager.nextPlayer == 0)
            {
                NetManager.rot.z = 90;
            }
            else if (NetManager.nextPlayer == 1)
            {
                NetManager.rot.z = 300;
            }
            else if (NetManager.nextPlayer == 2)
            {
                NetManager.rot.z = 210;
            }
        }
        if (NetManager.index == 1)
        {
            if (NetManager.nextPlayer == 1)
            {
                NetManager.rot.z = 90;
            }
            else if (NetManager.nextPlayer == 2)
            {
                NetManager.rot.z = 300;
            }
            else if (NetManager.nextPlayer == 0)
            {
                NetManager.rot.z = 210;
            }
        }
        if (NetManager.index == 2)
        {
            if (NetManager.nextPlayer == 2)
            {
                NetManager.rot.z = 90;
            }
            else if (NetManager.nextPlayer == 0)
            {
                NetManager.rot.z = 300;
            }
            else if (NetManager.nextPlayer == 1)
            {
                NetManager.rot.z = 210;
            }
        }
        InteractionView.turn_index.GetComponent<Transform>().rotation = Quaternion.Euler(NetManager.rot);
    }
    public void setMultiple()//设置倍数
    {
        InteractionView.txt_multiple.text = NetManager.multiple.ToString() + "倍";
    }
    public void setIndex()
    {
        if (RoundModel.Biggest == CharacterType.Player)
        {
            GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(4).gameObject.SetActive(true);
            GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(5).gameObject.SetActive(false);
            GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(6).gameObject.SetActive(false);
        }
        else if (RoundModel.Biggest == CharacterType.ComputerLeft)
        {
            GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(4).gameObject.SetActive(false);
            GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(5).gameObject.SetActive(true);
            GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(6).gameObject.SetActive(false);
        }
        else if (RoundModel.Biggest == CharacterType.ComputerRight)
        {
            GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(4).gameObject.SetActive(false);
            GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(5).gameObject.SetActive(false);
            GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(6).gameObject.SetActive(true);
        }
    }
    public void setScore()
    {
        if (NetManager.maxBet != -1)
        {
            Debug.Log("上次叫分" + NetManager.lastBet);
            if (NetManager.index == 0)
            {
                if (NetManager.turn == 0)
                {
                    if (NetManager.maxBet == 0 || NetManager.maxBet == NetManager.lastBet)
                    {
                        InteractionView.right_x0.gameObject.SetActive(true);
                    }
                    else if (NetManager.maxBet == 1)
                    {
                        InteractionView.right_x1.gameObject.SetActive(true);
                    }
                    else if (NetManager.maxBet == 2)
                    {
                        InteractionView.right_x2.gameObject.SetActive(true);
                    }
                }
                if (NetManager.turn == 2)
                {
                    if (NetManager.maxBet == 0 || NetManager.maxBet == NetManager.lastBet)
                    {
                        InteractionView.left_x0.gameObject.SetActive(true);
                    }
                    else if (NetManager.maxBet == 1)
                    {
                        InteractionView.left_x1.gameObject.SetActive(true);
                    }
                    else if (NetManager.maxBet == 2)
                    {
                        InteractionView.left_x2.gameObject.SetActive(true);
                    }
                }
            }
            if (NetManager.index == 1)
            {
                if (NetManager.turn == 1)
                {
                    if (NetManager.maxBet == 0||NetManager.maxBet == NetManager.lastBet)
                    {
                        InteractionView.right_x0.gameObject.SetActive(true);
                    }
                    else if (NetManager.maxBet == 1)
                    {
                        InteractionView.right_x1.gameObject.SetActive(true);
                    }
                    else if (NetManager.maxBet == 2)
                    {
                        InteractionView.right_x2.gameObject.SetActive(true);
                    }
                }
                if (NetManager.turn == 0)
                {
                    if (NetManager.maxBet == 0 || NetManager.maxBet == NetManager.lastBet)
                    {
                        InteractionView.left_x0.gameObject.SetActive(true);
                    }
                    else if (NetManager.maxBet == 1)
                    {
                        InteractionView.left_x1.gameObject.SetActive(true);
                    }
                    else if (NetManager.maxBet == 2)
                    {
                        InteractionView.left_x2.gameObject.SetActive(true);
                    }
                }
            }
            if (NetManager.index == 2)
            {
                if (NetManager.turn == 2)
                {
                    if (NetManager.maxBet == 0||NetManager.maxBet == NetManager.lastBet)
                    {
                        InteractionView.right_x0.gameObject.SetActive(true);
                    }
                    else if (NetManager.maxBet == 1)
                    {
                        InteractionView.right_x1.gameObject.SetActive(true);
                    }
                    else if (NetManager.maxBet == 2)
                    {
                        InteractionView.right_x2.gameObject.SetActive(true);
                    }
                }
                if (NetManager.turn == 1)
                {
                    if (NetManager.maxBet == 0 || NetManager.maxBet == NetManager.lastBet)
                    {
                        InteractionView.left_x0.gameObject.SetActive(true);
                    }
                    else if (NetManager.maxBet == 1)
                    {
                        InteractionView.left_x1.gameObject.SetActive(true);
                    }
                    else if (NetManager.maxBet == 2)
                    {
                        InteractionView.left_x2.gameObject.SetActive(true);
                    }
                }
            }
            NetManager.lastBet = NetManager.maxBet;
        }
    }
    public void setBigget()
    {
        if (NetManager.index == 0)
        {
            if (NetManager.currPlayer == 0)
                RoundModel.Biggest = CharacterType.Player;
            else if (NetManager.currPlayer == 1)
                RoundModel.Biggest = CharacterType.ComputerLeft;
            else if (NetManager.currPlayer == 2)
                RoundModel.Biggest = CharacterType.ComputerRight;
        }
        else if (NetManager.index == 1)
        {
            if (NetManager.currPlayer == 1)
                RoundModel.Biggest = CharacterType.Player;
            else if (NetManager.currPlayer == 2)
                RoundModel.Biggest = CharacterType.ComputerLeft;
            else if (NetManager.currPlayer == 0)
                RoundModel.Biggest = CharacterType.ComputerRight;
        }
        else if (NetManager.index == 2)
        {
            if (NetManager.currPlayer == 2)
                RoundModel.Biggest = CharacterType.Player;
            else if (NetManager.currPlayer == 0)
                RoundModel.Biggest = CharacterType.ComputerLeft;
            else if (NetManager.currPlayer == 1)
                RoundModel.Biggest = CharacterType.ComputerRight;
        }
    }
    public void setThreeCardCount()
    {
         Card lcard = new Card("ClubEight",Colors.Club,Weight.Eight,CharacterType.ComputerLeft);
         Card rcard = new Card("ClubEight",Colors.Club,Weight.Eight,CharacterType.ComputerRight);
        if (NetManager.index == 0)
        {
            if (NetManager.landlord == 1)
            {
                Debug.Log("加牌显示");
                InteractionView.Left.AddCard(lcard,false);
            }
            if (NetManager.landlord == 2)
            {
                Debug.Log("加牌显示");
                InteractionView.Right.AddCard(rcard, false);
            }
        }
        if (NetManager.index == 1)
        {
            if (NetManager.landlord == 2)
            {
                InteractionView.Left.AddCard(lcard, false);
            }
            if (NetManager.landlord == 0)
            {
                InteractionView.Right.AddCard(rcard, false);
            }
        }
        if (NetManager.index == 2)
        {
            if (NetManager.landlord == 0)
            {
                InteractionView.Left.AddCard(lcard, false);
            }
            if (NetManager.landlord == 1)
            {
                InteractionView.Right.AddCard(rcard, false);
            }
        }
    }
    public void setCardCount()
    {
        if (NetManager.index == 0)
        {
            if (NetManager.currPlayer == 1)
            {
                InteractionView.Left.DealCard();
            }
            if (NetManager.currPlayer == 2)
            {
                InteractionView.Right.DealCard();
            }
        }
        if (NetManager.index == 1)
        {
            if (NetManager.currPlayer == 2)
            {
                InteractionView.Left.DealCard();
            }
            if (NetManager.currPlayer == 0)
            {
                InteractionView.Right.DealCard();
            }
        }
        if (NetManager.index == 2)
        {
            if (NetManager.currPlayer == 0)
            {
                InteractionView.Left.DealCard();
            }
            if (NetManager.currPlayer == 1)
            {
                InteractionView.Right.DealCard();
            }
        }
    }
    public void setLandlord()
    {
        Image img = Resources.Load<Image>("Identity_Landlord");
        if (NetManager.index == 0)
        {
            if (NetManager.landlord == 1)
            {
                Image ide = GameObject.Find("Left_Identity").GetComponent<Image>();
                ide.sprite = img.sprite;
            }
            if (NetManager.landlord == 2)
            {
                Image ide = GameObject.Find("Right_Identity").GetComponent<Image>();
                ide.sprite = img.sprite;
            }
        }
        if (NetManager.index == 1)
        {
            if (NetManager.landlord == 2)
            {
                Image ide = GameObject.Find("Left_Identity").GetComponent<Image>();
                ide.sprite = img.sprite;
            }
            if (NetManager.landlord == 0)
            {
                Image ide = GameObject.Find("Right_Identity").GetComponent<Image>();
                ide.sprite = img.sprite;
            }
        }
        if (NetManager.index == 2)
        {
            if (NetManager.landlord == 0)
            {
                Image ide = GameObject.Find("Left_Identity").GetComponent<Image>();
                ide.sprite = img.sprite;
            }
            if (NetManager.landlord == 1)
            {
                Image ide = GameObject.Find("Right_Identity").GetComponent<Image>();
                ide.sprite = img.sprite;
            }
        }
    }
    public void callScore()
    {
        if (NetManager.turn == NetManager.index && NetManager.canCall)
        {
            InteractionView.ActiveGrabAndDisgrab();
            if (NetManager.maxBet == 1)
            {
                InteractionView.btn_Grab1.interactable = false;
            }
            else if(NetManager.maxBet == 2)
            {
                InteractionView.btn_Grab1.interactable = false;

                InteractionView.btn_Grab2.interactable = false;
            }
        }
    }
    public void BgmChange(bool sign)
    {
        if (sign == true)
        {
            NetManager.bgm_open = true;
            if (NetManager.pos == 1)
                InteractionView.soundManager.mc_normal.Play();
            else if (NetManager.pos == 2)
                InteractionView.soundManager.mc_exciting.Play();
        }
        //音效 bgm 监听
        else
        {
            NetManager.bgm_open = false;
            if (NetManager.pos == 1)
                InteractionView.soundManager.mc_normal.Pause();
            else if (NetManager.pos == 2)
                InteractionView.soundManager.mc_exciting.Pause();
        }
    }
    public void SoundChange(bool sign)
    {
        if (sign == true)
        {
            InteractionView.soundManager.mc_game_click.GetComponent<AudioSource>().volume = 1;
            InteractionView.soundManager.mc_baojing1.GetComponent<AudioSource>().volume = 1;
            InteractionView.soundManager.mc_baojing2.GetComponent<AudioSource>().volume = 1;
            InteractionView.soundManager.mc_buyao1.GetComponent<AudioSource>().volume = 1;
            InteractionView.soundManager.mc_lose.GetComponent<AudioSource>().volume = 1;
            InteractionView.soundManager.mc_win.GetComponent<AudioSource>().volume = 1;
            InteractionView.soundManager.mc_btn_click.GetComponent<AudioSource>().volume = 1;
        }
        //音效 bgm 监听
        else
        {
            InteractionView.soundManager.mc_game_click.GetComponent<AudioSource>().volume = 0;
            InteractionView.soundManager.mc_baojing1.GetComponent<AudioSource>().volume = 0;
            InteractionView.soundManager.mc_baojing2.GetComponent<AudioSource>().volume = 0;
            InteractionView.soundManager.mc_buyao1.GetComponent<AudioSource>().volume = 0;
            InteractionView.soundManager.mc_lose.GetComponent<AudioSource>().volume = 0;
            InteractionView.soundManager.mc_win.GetComponent<AudioSource>().volume = 0;
            InteractionView.soundManager.mc_btn_click.GetComponent<AudioSource>().volume = 0;
        }
    }
   
    //发牌 17 17 17
    IEnumerator wait()
    {
        yield return new WaitForSeconds(1f);
        Card lcard = new Card("ClubEight",Colors.Club,Weight.Eight,CharacterType.ComputerLeft);
        Card rcard = new Card("ClubEight",Colors.Club,Weight.Eight,CharacterType.ComputerRight);
        for (int i = 0; i < NetManager.cardlist.Count; i++)
        {
            InteractionView.Player.AddCard(NetManager.cardlist[i], false);
            InteractionView.Left.AddCard(lcard, false);
            InteractionView.Right.AddCard(rcard, false);
            yield return new WaitForSeconds(0.1f);
        }
        InteractionView.Player.Sort(false);
        NetManager.canCall = true;
        //显示底牌
        if (NetManager.show_extra)
        {
            List<Card> extra_list = Tools.getCardList(NetManager.external_card_list);
            InteractionView.extra_card1.sprite = Resources.Load<Sprite>("Pokers/" + extra_list[0].CardName);
            InteractionView.extra_card2.sprite = Resources.Load<Sprite>("Pokers/" + extra_list[1].CardName);
            InteractionView.extra_card3.sprite = Resources.Load<Sprite>("Pokers/" + extra_list[2].CardName);
            StartCoroutine(show_extra_anim());
        }
        else
        {
            InteractionView.extra_card1.sprite = Resources.Load<Sprite>("Pokers/CardBack1");
            InteractionView.extra_card2.sprite = Resources.Load<Sprite>("Pokers/CardBack1");
            InteractionView.extra_card3.sprite = Resources.Load<Sprite>("Pokers/CardBack1");
            StartCoroutine(show_extra_anim());
        }
    }
    IEnumerator show_extra_anim()
    {
        CanvasGroup cg_card1 = InteractionView.extra_card1.GetComponent<CanvasGroup>();
        CanvasGroup cg_card2 = InteractionView.extra_card2.GetComponent<CanvasGroup>();
        CanvasGroup cg_card3 = InteractionView.extra_card3.GetComponent<CanvasGroup>();
        while (cg_card1.alpha < 1)
        {
            yield return new WaitForSeconds(0.1f);
            cg_card1.alpha += 0.2f;
            cg_card2.alpha = cg_card1.alpha;
            cg_card3.alpha = cg_card1.alpha;
        }

    }
    IEnumerator close_extra_anim()
    {
        CanvasGroup cg_card1 = InteractionView.extra_card1.GetComponent<CanvasGroup>();
        CanvasGroup cg_card2 = InteractionView.extra_card2.GetComponent<CanvasGroup>();
        CanvasGroup cg_card3 = InteractionView.extra_card3.GetComponent<CanvasGroup>();
        while (cg_card1.alpha > 0)
        {
            yield return new WaitForSeconds(0.1f);
            cg_card1.alpha -= 0.2f;
            cg_card2.alpha = cg_card1.alpha;
            cg_card3.alpha = cg_card1.alpha;
        }

    }
    public override void OnRegister()
    {
		InteractionView.Setting.SetActive (false);
		InteractionView.btn_Deal.onClick.AddListener(onDealClick);

        dispatcher.AddListener(ViewEvent.COMPLETE_DEAL, onCompleteDeal);
        dispatcher.AddListener(ViewEvent.COMPLETE_PLAY, onCompletePlay);
        dispatcher.AddListener(ViewEvent.RESTART_GAME, onRestartGame);
		dispatcher.AddListener (ViewEvent.RESTART_FIRST, onRestartGameToFirst);

        InteractionView.btn_Disgrab.onClick.AddListener(onDisgrabClick);
        InteractionView.btn_Grab1.onClick.AddListener(delegate(){
            onGrabClick(1);
        });
        InteractionView.btn_Grab2.onClick.AddListener(delegate()
        {
            onGrabClick(2);
        });
        InteractionView.btn_Grab3.onClick.AddListener(delegate()
        {
            onGrabClick(3);
        });
        InteractionView.tog_bgm.onValueChanged.AddListener((bool value) => BgmChange(value));
        InteractionView.tog_sound.onValueChanged.AddListener((bool value) => SoundChange(value));
        InteractionView.btn_Pass.onClick.AddListener(onPassClick);
        InteractionView.btn_Play.onClick.AddListener(onPlayClick);

        InteractionView.game_close.onClick.AddListener(CloseSetting);
		InteractionView.btn_Setting.onClick.AddListener (ClickSetting);

        InteractionView.main_exit_yes.onClick.AddListener(Main_Exit_Yes);
        InteractionView.main_exit_no.onClick.AddListener(Main_Exit_No);
		InteractionView.btn_Exit.onClick.AddListener (Exit);
		

        //InteractionView.Times1.onClick.AddListener (delegate(){
        //    SetTimes(1);
        //});
        //InteractionView.Times2.onClick.AddListener (delegate(){
        //    SetTimes(2);
        //});
        //InteractionView.Times3.onClick.AddListener (delegate(){
        //    SetTimes(3);
        //});
        //InteractionView.Times4.onClick.AddListener (delegate(){
        //    SetTimes(4);
        //});
        //InteractionView.Times5.onClick.AddListener (delegate(){
        //    SetTimes(5);
        //});
        //InteractionView.UpToYou.onClick.AddListener (ShowUpToYouBtn);
        //InteractionView.Confirm.onClick.AddListener (GetRoundTimes);

		dispatcher.AddListener (ViewEvent.OPEN_INFORMATION,OpenInformation);

        InteractionView.btn_Leave.onClick.AddListener(roomLeave);

        InteractionView.btn_vote.onClick.AddListener(OpenVotePanel);
        InteractionView.vote_close.onClick.AddListener(CloseVotePanel);
        InteractionView.btn_agree.onClick.AddListener(AgreeMessage);
        InteractionView.btn_disagree.onClick.AddListener(DisagreeMessage);

        InteractionView.btn_backstart.onClick.AddListener(DismissRoom);

        //注册静态事件
        RoundModel.PlayerHandler += ActiveButton;
    }

    public override void OnRemove()
    {
        InteractionView.btn_Deal.onClick.RemoveListener(onDealClick);


        dispatcher.RemoveListener(ViewEvent.COMPLETE_DEAL, onCompleteDeal);
        dispatcher.RemoveListener(ViewEvent.COMPLETE_PLAY, onCompletePlay);
		dispatcher.RemoveListener(ViewEvent.RESTART_GAME, onRestartGame);
		dispatcher.RemoveListener (ViewEvent.RESTART_FIRST, onRestartGameToFirst);

        InteractionView.btn_Disgrab.onClick.RemoveListener(onDisgrabClick);
        InteractionView.btn_Grab1.onClick.RemoveListener(delegate()
        {
            onGrabClick(1);
        });
        InteractionView.btn_Grab2.onClick.RemoveListener(delegate()
        {
            onGrabClick(2);
        }); ;
        InteractionView.btn_Grab3.onClick.RemoveListener(delegate()
        {
            onGrabClick(3);
        });
        InteractionView.tog_bgm.onValueChanged.RemoveListener((bool value) => BgmChange(value));
        InteractionView.tog_sound.onValueChanged.RemoveListener((bool value) => SoundChange(value));
        InteractionView.btn_Pass.onClick.RemoveListener(onPassClick);
        InteractionView.btn_Play.onClick.RemoveListener(onPlayClick);

        InteractionView.game_close.onClick.RemoveListener(CloseSetting);
		InteractionView.btn_Setting.onClick.RemoveListener (ClickSetting);

        InteractionView.main_exit_yes.onClick.RemoveListener(Main_Exit_Yes);
        InteractionView.main_exit_no.onClick.RemoveListener(Main_Exit_No);
		InteractionView.btn_Exit.onClick.RemoveListener (Exit);
		

        //InteractionView.Times1.onClick.RemoveListener (delegate(){
        //    SetTimes(1);
        //});
        //InteractionView.Times2.onClick.RemoveListener (delegate(){
        //    SetTimes(2);
        //});
        //InteractionView.Times3.onClick.RemoveListener (delegate(){
        //    SetTimes(3);
        //});
        //InteractionView.Times4.onClick.RemoveListener (delegate(){
        //    SetTimes(4);
        //});
        //InteractionView.Times5.onClick.RemoveListener (delegate(){
        //    SetTimes(5);
        //});

        //InteractionView.UpToYou.onClick.RemoveListener (ShowUpToYouBtn);
        //InteractionView.Confirm.onClick.RemoveListener (GetRoundTimes);

        InteractionView.btn_Leave.onClick.RemoveListener(roomLeave);

		dispatcher.RemoveListener (ViewEvent.OPEN_INFORMATION,OpenInformation);

        InteractionView.btn_vote.onClick.RemoveListener(OpenVotePanel);
        InteractionView.vote_close.onClick.RemoveListener(CloseVotePanel);

        InteractionView.btn_agree.onClick.RemoveListener(AgreeMessage);
        InteractionView.btn_disagree.onClick.RemoveListener(DisagreeMessage);

        InteractionView.btn_backstart.onClick.RemoveListener(DismissRoom);

        RoundModel.PlayerHandler -= ActiveButton;
    }

    public void roomLeave()
    {
        Debug.Log("请求离开房间");
        leave_message = EncodeTool.LeaveRoomEncode(NetManager.account);
        NetManager.client.Send(leave_message);
        GameObject.Find("Start").GetComponent<Transform>().GetChild(0).gameObject.SetActive(true);
        InteractionView.soundManager.mc_normal.Stop();
        InteractionView.soundManager.mc_exciting.Stop();
        InteractionView.soundManager.mc_welcome.Play();

        //清空自己的准备状态
        InteractionView.btn_Deal.gameObject.SetActive(true);
        GameObject.Find("Player").GetComponent<Transform>().GetChild(2).gameObject.SetActive(false);
        GameObject.Find("Player").GetComponent<Transform>().GetChild(1).gameObject.SetActive(false);
        NetManager.readylist[NetManager.index] = false;
        InteractionView.Player.Clear();
        //Tools.CreateUIPanel(PanelType.StartPanel);
        NetManager.readylist = new List<bool> { false, false, false };
        InteractionView.Player.Clear();
        InteractionView.Left.Clear();
        InteractionView.Right.Clear();
        InteractionView.Desk.Clear();
        InteractionView.DeactiveAll();
    }

    #region 回调事件

    public void AgreeMessage()
    {
        agree_message = EncodeTool.VoteEncode(NetManager.account, true);
        NetManager.client.Send(agree_message);
        InteractionView.img_vote.gameObject.SetActive(false);
        InteractionView.btn_vote.gameObject.SetActive(false);
    }
    public void DisagreeMessage()
    {
        disagree_message = EncodeTool.VoteEncode(NetManager.account, false);
        NetManager.client.Send(disagree_message);
        InteractionView.img_vote.gameObject.SetActive(false);
        InteractionView.btn_vote.gameObject.SetActive(false);
    }
    public void OpenVotePanel()
    {
        InteractionView.img_vote.gameObject.SetActive(true);
    }
    public void CloseVotePanel()
    {
        InteractionView.img_vote.gameObject.SetActive(false);
    }

	private void OpenInformation()
	{
		InteractionView.ShowInformation ();
		GameData playerInfo = new GameData();
		playerInfo = Tools.GeyDataWithOutBom ();
		InteractionView.CurrScore.text = "当前分数："+ playerInfo.PlayerIntergration;
		InteractionView.CurrRanking.text = "当前段位：" + Tools.GetRanking (playerInfo.PlayerIntergration);
		InteractionView.Rounds.text = "局数：" + playerInfo.Rounds;
		InteractionView.WinRate.text = "胜率：" + (100.0f*(double)playerInfo.Win/playerInfo.Rounds).ToString("#0.00") + "%";
		string nextRankScore;
		if (playerInfo.PlayerIntergration <= 10)
		{
			nextRankScore = "11";
		}
		else if (playerInfo.PlayerIntergration <= 100) {
			nextRankScore = "101";
		}
		else if (playerInfo.PlayerIntergration <= 500) {
			nextRankScore = "501";
		}
		else
			nextRankScore = "你已成神";

		InteractionView.NextRanking.text = nextRankScore;
	}

	public void Exit()
	{
        InteractionView.main_exit_panel.gameObject.SetActive(true);
	}
    public void Main_Exit_Yes()
    {
        
        uncon = EncodeTool.ReconEncode(100, NetManager.account, 0, NetManager.token);
        NetManager.client.Send(uncon);
        Application.Quit();
    }
    public void Main_Exit_No()
    {
        InteractionView.main_exit_panel.gameObject.SetActive(false);
    }

    //public void SetTimes(int i)
    //{
    //    RoundModel.RoundTimes = i;
    //    Debug.Log (RoundModel.RoundTimes);
    //    times = RoundModel.RoundTimes;
    //    sign = 0;
    //    RoundModel.LeftTotalScore = 0;
    //    RoundModel.RightTotalScore = 0;
    //    RoundModel.PlayerTotalScore = 0;
    //    InteractionView.DeactiveTimesBtn ();
    //    InteractionView.ActiveDeal();
    //}

    ///// <summary>
    ///// 自定义设置局数
    ///// </summary>
    //public void ShowUpToYouBtn()
    //{
    //    InteractionView.RoundTimes.gameObject.SetActive (true);
    //    InteractionView.Confirm.gameObject.SetActive (true);
    //}

    ///// <summary>
    ///// 获得局数
    ///// </summary>
    //private void GetRoundTimes()
    //{
    //    RoundModel.RoundTimes = int.Parse (InteractionView.RoundTimes.text);
    //    Debug.Log (RoundModel.RoundTimes);
    //    InteractionView.ActiveDeal();
    //    //将新的值赋予局数框
    //    times = RoundModel.RoundTimes;
    //    sign = 0;
    //    RoundModel.LeftTotalScore = 0;
    //    RoundModel.RightTotalScore = 0;
    //    RoundModel.PlayerTotalScore = 0;
    //    InteractionView.DeactiveTimesBtn ();
    //}

	private void onRestartGameToFirst()
	{
		InteractionView.DeactiveAll();
		//InteractionView.ActiveTimesBtn ();
	}

    /// <summary>
    /// 重新开始游戏的回调
    /// </summary>
    private void onRestartGame()
    {
        InteractionView.DeactiveAll();
        InteractionView.ActiveDeal();
    }

    IEnumerator wait_deal()
    {
        yield return new WaitForSeconds(5f);
        if (NetManager.readylist[NetManager.index] == false)
        {
            NetManager.client.socket.Close();
            NetManager.client = new ClientPeer("61.164.248.190", 4396);
            NetManager.client.Connect();
            message = EncodeTool.ReconEncode(101, NetManager.account, 0, NetManager.token);
            NetManager.client.Send(message);
        }
        yield return new WaitForSeconds(5f);
        if (NetManager.readylist[NetManager.index] == false)
        {
            message1 = EncodeTool.ReadyEncode(NetManager.account);
            NetManager.client.Send(message1);
        }
    }
    /// <summary>
    /// 发牌的回调函数
    /// </summary>
    private void onDealClick()
    {
        InteractionView.soundManager.mc_game_click.Play();
        //发送准备信息给发服务器
		message = EncodeTool.ReadyEncode (NetManager.account);
		NetManager.client.Send (message);
        GameObject.Find("Player").GetComponent<Transform>().GetChild(2).gameObject.SetActive(true);
        //dispatcher.Dispatch(CommandEvent.RequestDeal);
        InteractionView.DeactiveAll();
        StartCoroutine(wait_deal());
     




        /////发完牌显示局数百分比
        ////InteractionView.Times.gameObject.SetActive(true);
        //if (sign == 0) {
        //    times = RoundModel.RoundTimes;
        //    sign++;
        //}
        //int a = times - RoundModel.RoundTimes + 1;
        //InteractionView.Times.text = a + "/" + times; 
    }

    /// <summary>
    /// 发牌结束的回调
    /// </summary>
    /// <param name="payload"></param>
    private void onCompleteDeal()
    {
        InteractionView.ActiveGrabAndDisgrab();
    }

    /// <summary>
    /// 不抢的点击事件
    /// </summary>
    private void onDisgrabClick()
    {
        InteractionView.player_x0.gameObject.SetActive(true);

        NetManager.canCall = false;
        //GameObject.Find("Player_Disgrab").SetActive(true);
        InteractionView.DeactiveAll();
        disgrab_message = EncodeTool.CallEncode(NetManager.account, 0);
        NetManager.client.Send(disgrab_message);
        Debug.Log("叫分一次");
        //int r = UnityEngine.Random.Range(2, 4);
        //GrabLandlordArgs e = new GrabLandlordArgs()
        //{ cType = (CharacterType)r };
        //dispatcher.Dispatch(CommandEvent.GrabLandLord, e);
    }


    /// <summary>
    /// 抢地主的点击事件
    /// </summary>
    private void onGrabClick(int bet)
    {
        //显示倍数
        if (bet == 1)
        {
            InteractionView.player_x1.gameObject.SetActive(true);
        }
        else if (bet == 2)
        {
            InteractionView.player_x2.gameObject.SetActive(true);
        }



        InteractionView.DeactiveAll();
       // GameObject.Find("Player_Grab").SetActive(true);
        NetManager.canCall = false;
        grab_message = EncodeTool.CallEncode(NetManager.account, bet);
        NetManager.client.Send(grab_message);
        //GrabLandlordArgs e = new GrabLandlordArgs()
        //{ cType = CharacterType.Player };
        //dispatcher.Dispatch(CommandEvent.GrabLandLord, e);
    }

    /// <summary>
    /// 激活按钮
    /// </summary>
    private void ActiveButton(bool canPass)
    {
		InteractionView.ActivePlayAndPass(canPass);
		if (InteractionView.btn_Pass.interactable) {
			Debug.Log (InteractionView.btn_Pass.interactable);
		}

    }

    /// <summary>
    /// 出牌点击事件
    /// </summary>
    private void onPlayClick()
    {
        dispatcher.Dispatch(ViewEvent.REQUEST_PLAY);
    }

    /// <summary>
    /// 不出点击事件
    /// </summary>
    private void onPassClick()
    {
        //dispatcher.Dispatch(CommandEvent.PassCard);
        InteractionView.DeactiveAll();
        
        pass_message = EncodeTool.PassEncode(NetManager.account);
        NetManager.client.Send(pass_message);
    }

    /// <summary>
    /// 完成出牌的回调
    /// </summary>
    private void onCompletePlay()
    {
        Debug.Log("完成出牌");
        InteractionView.DeactiveAll();

    }

	public void ClickSetting()
	{
        InteractionView.soundManager.mc_game_click.Play();
		InteractionView.Setting.SetActive (true);
	}
    public void CloseSetting()
    {
        InteractionView.soundManager.mc_game_click.Play();
        InteractionView.Setting.SetActive(false);
    }


    #endregion





}
