using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMediator : EventMediator
{
    [Inject]
    public GameOverView GameOverView { get; set; }
	
    [Inject]
	public RoundModel RoundModel{ get; set;}

    public byte[] reready_message;

    private byte[] leave_message;

    public byte[] message;
    public byte[] message1;

    public byte[] allscore_message;
    void Update()
    {
        if (NetManager.change_score)
        {
            GameOverView.Player.characterUI.RoundScore(NetManager.player_score);
            GameOverView.Left.characterUI.RoundScore(NetManager.left_score);
            GameOverView.Right.characterUI.RoundScore(NetManager.right_score);
            NetManager.change_score = false;
        }
        if (NetManager.change_allscore)
        {
            wait_allscore();
            NetManager.change_allscore = false;
        }
    }
	public override void OnRegister ()
	{

		//dispatcher.AddListener (ViewEvent.SHOW_ROUNDSCORE,onGetScore);
		GameOverView.Restart.onClick.AddListener (OnRestartClick);
        GameOverView.back_main.onClick.AddListener(Back_Main);
		//dispatcher.AddListener (ViewEvent.GAME_END,GameEnd);
		//GameOverView.btn_Exit.onClick.AddListener (OnExitClick);
		//GameOverView.btn_RestartToFirst.onClick.AddListener (OnRestartToFirst);
	}

	public override void OnRemove ()
	{
		//dispatcher.RemoveListener (ViewEvent.SHOW_ROUNDSCORE,onGetScore);
		GameOverView.Restart.onClick.RemoveListener (OnRestartClick);
        GameOverView.back_main.onClick.RemoveListener(Back_Main);
		//dispatcher.RemoveListener (ViewEvent.GAME_END,GameEnd);
		//GameOverView.btn_Exit.onClick.RemoveListener (OnExitClick);
		//GameOverView.btn_RestartToFirst.onClick.RemoveListener (OnRestartToFirst);
	}

    ////显示单局最终得分
    //private void onGetScore()
    //{
    //    string left;
    //    string right;
    //    string player;
    //    if (RoundModel.LeftGetScore > 0) {
    //        left = "+" + RoundModel.LeftGetScore;
    //    }
    //    else
    //    {
    //        left = RoundModel.LeftGetScore.ToString();
    //    }
    //    if (RoundModel.RightGetScore > 0) {
    //        right = "+" + RoundModel.RightGetScore;
    //    }
    //    else
    //    {
    //        right = RoundModel.RightGetScore.ToString();
    //    }
    //    if (RoundModel.PlayerGetScore > 0) {
    //        player = "+" + RoundModel.PlayerGetScore;
    //    }
    //    else
    //    {
    //        player = RoundModel.PlayerGetScore.ToString();
    //    }
    //    GameOverView.LeftRoundScore.text = "左:" + left;
    //    GameOverView.RightRoundScore.text = "右:" + right;
    //    GameOverView.PlayerRoundScore.text = "玩家:" + player;
    //}

    //public void GameEnd()
    //{
    //    GameOverView.TotalPanel.SetActive (true);
    //    string left;
    //    string right;
    //    string player;
    //    if (RoundModel.LeftTotalScore > 0) {
    //        left = "+" + RoundModel.LeftTotalScore;
    //    }
    //    else
    //    {
    //        left = RoundModel.LeftTotalScore.ToString();
    //    }
    //    if (RoundModel.RightTotalScore > 0) {
    //        right = "+" + RoundModel.RightTotalScore;
    //    }
    //    else
    //    {
    //        right = RoundModel.RightTotalScore.ToString();
    //    }
    //    if (RoundModel.PlayerTotalScore > 0) {
    //        player = "+" + RoundModel.PlayerTotalScore;
    //    }
    //    else
    //    {
    //        player = RoundModel.PlayerTotalScore.ToString();
    //    }
    //    GameOverView.LeftTotalScore.text = "左:" + left;
    //    GameOverView.RightTotalScore.text = "右:" + right;
    //    GameOverView.PlayerTotalScore.text = "玩家:" + player;
    //}
    /// <summary>
    /// 初始化标志位等信息
    /// </summary>
    public void InitGame()
    {
        NetManager.cardlist.Clear();
        Image img = Resources.Load<Image>("Identity_Farmer");
        NetManager.setReady = true;
        //清空createpoint下的牌 并设置可以三个人准备开始游戏标志
        GameOverView.Desk.Clear();
        GameOverView.Left.Clear();
        GameOverView.Right.Clear();
        GameOverView.Player.Clear();
        NetManager.current_card_list = null;
        NetManager.create = true;
        GameOverView.player_ide.sprite = img.sprite;
        GameOverView.left_ide.sprite = img.sprite;
        GameOverView.right_ide.sprite = img.sprite;
        for (int i = 0; i < 3; i++)
        {
            NetManager.readylist[i] = false;
        }
        NetManager.canCall = false;
        NetManager.nextPlayer = -1;
        NetManager.turn = -1;
        NetManager.maxBet = 0;
        NetManager.baojing1 = true;
        NetManager.baojing2 = true;
        NetManager.first_play = true;
        NetManager.multiple = 1;
        NetManager.currPlayer = NetManager.index;
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
	/// 点击开始下一局
	/// </summary>
	public void OnRestartClick()
	{
        InitGame();

        
        if (NetManager.remian_nning > 0)
        {
            reready_message = EncodeTool.ReadyEncode(NetManager.account);
            NetManager.client.Send(reready_message);
            StartCoroutine(wait_deal());
            GameObject endPanel = GameObject.Find("GameOver").GetComponent<Transform>().GetChild(0).gameObject;
            endPanel.SetActive(false);
        }
        else
        {
            allscore_message = EncodeTool.AllScoreEncode(NetManager.account);
            NetManager.client.Send(allscore_message);
            GameObject totalPanel = GameObject.Find("GameOver").GetComponent<Transform>().GetChild(1).gameObject;
            totalPanel.SetActive(true);
          
        
        }
        
        
        //if (RoundModel.RoundTimes != 0) {
        //    dispatcher.Dispatch(ViewEvent.RESTART_GAME);
        //}
        //else
        //{
        //    dispatcher.Dispatch(ViewEvent.GAME_END);
        //}

	}
    public void  wait_allscore()
    {
        if (NetManager.index == 0)
        {
            GameOverView.img_00.sprite = GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(2).GetChild(0).gameObject.GetComponent<Image>().sprite;
            GameOverView.img_11.sprite = GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite;
            GameOverView.img_22.sprite = GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(1).GetChild(0).gameObject.GetComponent<Image>().sprite;
            //GameOverView.land0.text = NetManager.player_lan.ToString();
            //GameOverView.land1.text = NetManager.left_lan.ToString();
            //GameOverView.land2.text = NetManager.right_lan.ToString();
            //GameOverView.score0.text = NetManager.player_score.ToString();
            //GameOverView.score1.text = NetManager.left_score.ToString();
            //GameOverView.score2.text = NetManager.right_score.ToString();


        }

        else if (NetManager.index == 1)
        {
            GameOverView.img_11.sprite = GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(2).GetChild(0).gameObject.GetComponent<Image>().sprite;
            GameOverView.img_22.sprite = GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite;
            GameOverView.img_00.sprite = GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(1).GetChild(0).gameObject.GetComponent<Image>().sprite;
            //GameOverView.land1.text = NetManager.player_lan.ToString();
            //GameOverView.land2.text = NetManager.left_lan.ToString();
            //GameOverView.land0.text = NetManager.right_lan.ToString();
            //GameOverView.score1.text = NetManager.player_score.ToString();
            //GameOverView.score2.text = NetManager.left_score.ToString();
            //GameOverView.score0.text = NetManager.right_score.ToString();
        }
        else if (NetManager.index == 2)
        {
            GameOverView.img_22.sprite = GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(2).GetChild(0).gameObject.GetComponent<Image>().sprite;
            GameOverView.img_00.sprite = GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite;
            GameOverView.img_11.sprite = GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(1).GetChild(0).gameObject.GetComponent<Image>().sprite;
            //GameOverView.land1.text = NetManager.player_lan.ToString();
            //GameOverView.land0.text = NetManager.left_lan.ToString();
            //GameOverView.land2.text = NetManager.right_lan.ToString();
            //GameOverView.score2.text = NetManager.player_score.ToString();
            //GameOverView.score0.text = NetManager.left_score.ToString();
            //GameOverView.score1.text = NetManager.right_score.ToString();
        }
        GameOverView.land0.text = NetManager.landlordList[0].ToString();
        GameOverView.land1.text = NetManager.landlordList[1].ToString();
        GameOverView.land2.text = NetManager.landlordList[2].ToString();
        GameOverView.score0.text = NetManager.allscoreList[0].ToString();
        GameOverView.score1.text = NetManager.allscoreList[1].ToString();
        GameOverView.score2.text = NetManager.allscoreList[2].ToString();
        if (NetManager.allscoreList[0] > NetManager.allscoreList[1])
        {
            if (NetManager.allscoreList[0] > NetManager.allscoreList[2])
            {
                GameOverView.img_k0.gameObject.SetActive(true);
            }
            else if (NetManager.allscoreList[0] < NetManager.allscoreList[2])
            {
                 GameOverView.img_k2.gameObject.SetActive(true);
            }
        }
        else
        {
            if (NetManager.allscoreList[1] > NetManager.allscoreList[2])
            {
                GameOverView.img_k1.gameObject.SetActive(true);
            }
            else if (NetManager.allscoreList[1] < NetManager.allscoreList[2])
            {
                GameOverView.img_k2.gameObject.SetActive(true);
            } 
        }
        GameObject endPanel = GameObject.Find("GameOver").GetComponent<Transform>().GetChild(0).gameObject;
        endPanel.SetActive(false);
    }
    /// <summary>
    /// 返回大厅
    /// </summary>
    public void Back_Main()
    {
        //隐藏大赢家符号
        GameOverView.img_k0.gameObject.SetActive(false);
        GameOverView.img_k1.gameObject.SetActive(false);
        GameOverView.img_k2.gameObject.SetActive(false);
        GameObject totalPanel = GameObject.Find("GameOver").GetComponent<Transform>().GetChild(1).gameObject;
        totalPanel.SetActive(false);
        //积分信息清零
        NetManager.left_score = 0;
        NetManager.left_lan = 0;
        NetManager.right_score = 0;
        NetManager.right_lan = 0;
        NetManager.player_score = 0;
        NetManager.player_lan = 0;
        NetManager.change_score = true;
       

        GameObject.Find("Start").GetComponent<Transform>().GetChild(0).gameObject.SetActive(true);
        GameOverView.soundManager.mc_welcome.Play();
        NetManager.player_pos = 0;
        //改变房卡信息

        //AA
        if (NetManager.payment == 1)
        {
            int pay_card = NetManager.Inning * 20;
            NetManager.roomcard = NetManager.roomcard - pay_card;
            NetManager.setRoomCard = true;
        }
        //房主支付
        else if (NetManager.payment == 0)
        {
            if (NetManager.index == NetManager.host_num)
            {
                int pay_card = NetManager.Inning * 60;
                NetManager.roomcard = NetManager.roomcard - pay_card;
                NetManager.setRoomCard = true;
            }
        }
        NetManager.playerNum = 0;
        leave_message = EncodeTool.LeaveRoomEncode(NetManager.account);
        NetManager.client.Send(leave_message);
    

        //显示发牌按钮
        GameOverView.btn_deal.gameObject.SetActive(true);
    }

	/// <summary>
	/// 从头开始
	/// </summary>
    //public void OnRestartToFirst()
    //{
    //    GameOverView.TotalPanel.SetActive (false);
    //    dispatcher.Dispatch (ViewEvent.RESTART_FIRST);
    //}


	/// <summary>
	/// 退出的点击
	/// </summary>
	public void OnExitClick()
	{
		Application.Quit();
	}
}

