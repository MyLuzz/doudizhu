using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using UnityEngine.UI;

public class GameOverCommand : EventCommand
{
    [Inject]
    public IntegrationModel IntegrationModel { get; set; }

    [Inject]
    public RoundModel RoundModel { get; set; }

    [Inject]
    public CardModel CardModel { get; set; }

	public GameObject GameEnding;

    public override void Execute()
    {
        
        setScore();
        #region emm
        //GameObject count =  GameObject.Find ("Count").GetComponent<Transform> ().GetChild (0).gameObject;
        //count.SetActive (false);
        //int result = IntegrationModel.Result;
        //IntegrationModel.Multiples = 1;
        //GameOverArgs e = evt.data as GameOverArgs;
        //Debug.Log (e.characterType.ToString ());

        //GameData data = new GameData();
        //data = Tools.GeyDataWithOutBom ();
        //IntegrationModel.PlayerIntergration = data.PlayerIntergration;
        //IntegrationModel.ComputerLeftIntergration = data.ComputerLeftIntergration;
        //IntegrationModel.ComputerRightIntergration  = data.ComputerRightIntergration;
        //int TempLeftScore = data.ComputerLeftIntergration;
        //int TempRightScore = data.ComputerRightIntergration;
        //int TempPlayerScore = data.PlayerIntergration;

        //if (e.PlayerWin){
        //    data.Win++;
        //    GameEnding = Resources.Load<GameObject>("game_win");

        //    if(e.characterType == CharacterType.Player)
        //    {
        //        IntegrationModel.PlayerIntergration += result*2;
        //    }
        //    else
        //        IntegrationModel.PlayerIntergration += result;
        //}
        //else {
        //    GameEnding = Resources.Load<GameObject>("game_lost");
        //    if(e.characterType == CharacterType.Player)
        //    {
        //        IntegrationModel.PlayerIntergration -= result*2;
        //    }
        //    else
        //        IntegrationModel.PlayerIntergration -= result;
        //}


        //if (e.ComputerLeftWin){
        //    if(e.characterType == CharacterType.ComputerLeft)
        //    {
        //        IntegrationModel.ComputerLeftIntergration += result*2;
        //    }
        //    else
        //        IntegrationModel.ComputerLeftIntergration += result;
        //}
        //else {
        //    if(e.characterType == CharacterType.ComputerLeft)
        //    {
        //        IntegrationModel.ComputerLeftIntergration -= result*2;
        //    }
        //    else
        //        IntegrationModel.ComputerLeftIntergration -= result;
        //}


        //if (e.ComputerRightWin){
        //    if(e.characterType == CharacterType.ComputerRight)
        //    {
        //        IntegrationModel.ComputerRightIntergration += result*2;
        //    }
        //    else
        //        IntegrationModel.ComputerRightIntergration += result;
        //}
        //else {
        //    if(e.characterType == CharacterType.ComputerRight)
        //    {
        //        IntegrationModel.ComputerRightIntergration -= result*2;
        //    }
        //    else
        //        IntegrationModel.ComputerRightIntergration -= result;
        //}
			
        //data.PlayerIntergration = IntegrationModel.PlayerIntergration;
        //data.ComputerLeftIntergration = IntegrationModel.ComputerLeftIntergration;
        //data.ComputerRightIntergration = IntegrationModel.ComputerRightIntergration;
        //data.Rounds++;
        //Tools.SaveData(data);

        ////计算单局的得分 前后相减
        //RoundModel.LeftGetScore = data.ComputerLeftIntergration - TempLeftScore;
        //RoundModel.RightGetScore = data.ComputerRightIntergration - TempRightScore;
        //RoundModel.PlayerGetScore = data.PlayerIntergration - TempPlayerScore;
        ////更新积分UI
        //dispatcher.Dispatch(ViewEvent.UPDATE_INTEGRATION, data);

        ////CardModel.InitCardLibrary();
        //RoundModel.InitRound();
        ////Tools.CreateUIPanel(PanelType.GameOverPanel); 
        //RoundModel.RoundTimes--;
        ////计算总得分
        //RoundModel.LeftTotalScore += RoundModel.LeftGetScore;
        //RoundModel.RightTotalScore += RoundModel.RightGetScore;
        //RoundModel.PlayerTotalScore += RoundModel.PlayerGetScore;

        ////设置每次游戏的人物UI积分
        //RoundScoreData rsd = new RoundScoreData ();
        //rsd.RoundScoreLeft = RoundModel.LeftTotalScore;
        //rsd.RoundScoreRight = RoundModel.RightTotalScore;
        //rsd.RoundScorePlayer = RoundModel.PlayerTotalScore;
        //dispatcher.Dispatch (ViewEvent.SET_ROUNDSCORE, rsd);

        //GameObject endPanel = GameObject.Find ("GameOver").GetComponent<Transform>().GetChild(0).gameObject;

        //GameObject.Find("GameOver").transform.SetSiblingIndex (7);

        //endPanel.GetComponent<Image> ().sprite = GameEnding.GetComponent<Image> ().sprite;
        //dispatcher.Dispatch (ViewEvent.SHOW_ROUNDSCORE);
        //endPanel.SetActive (true); 
        #endregion
    }
    public void setScore()
    {
        //yield return new WaitForSeconds(0.5f);
        GameObject end = GameObject.Find("GameOver").GetComponent<Transform>().GetChild(0).gameObject;
        end.SetActive(true);
        //将查看总分文字改回 下一局
        GameObject.Find("btn_Restart").GetComponent<Transform>().GetChild(0).gameObject.GetComponent<Text>().text = "下一局";
        if (NetManager.remian_nning <= 0)//remian_nning：剩余局数
        {
            GameObject.Find("btn_Restart").GetComponent<Transform>().GetChild(0).gameObject.GetComponent<Text>().text = "查看总分";
        }
        Image img = Resources.Load<Image>("game_lost");
        Image img1 = Resources.Load<Image>("game_win");
        if (NetManager.scoreList[NetManager.index] < 0)//得分为负表示输掉此局比赛，设置图片为输
        {
            end.GetComponent<Image>().sprite = img.sprite;
        }
        else
            end.GetComponent<Image>().sprite = img1.sprite;//得分为正表示赢得此局比赛，设置得分为赢

        GameObject.Find("Score0").GetComponent<Text>().text = getText(NetManager.scoreList[0]);
        GameObject.Find("Score1").GetComponent<Text>().text = getText(NetManager.scoreList[1]);
        GameObject.Find("Score2").GetComponent<Text>().text = getText(NetManager.scoreList[2]);
        if (NetManager.index == 0)
        {
            GameObject.Find("img_0").GetComponent<Image>().sprite = GameObject.Find("Player_Head").GetComponent<Image>().sprite;
            GameObject.Find("img_1").GetComponent<Image>().sprite = GameObject.Find("Left_Head").GetComponent<Image>().sprite;
            GameObject.Find("img_2").GetComponent<Image>().sprite = GameObject.Find("Right_Head").GetComponent<Image>().sprite;
            
            NetManager.left_score += NetManager.scoreList[1];
            
            NetManager.right_score += NetManager.scoreList[2];
        }

        else if (NetManager.index == 1)
        {
            GameObject.Find("img_1").GetComponent<Image>().sprite = GameObject.Find("Player_Head").GetComponent<Image>().sprite;
            GameObject.Find("img_2").GetComponent<Image>().sprite = GameObject.Find("Left_Head").GetComponent<Image>().sprite;
            GameObject.Find("img_0").GetComponent<Image>().sprite = GameObject.Find("Right_Head").GetComponent<Image>().sprite;

            NetManager.left_score += NetManager.scoreList[2];
       
            NetManager.right_score += NetManager.scoreList[0];
        }
        else if (NetManager.index == 2)
        {
            GameObject.Find("img_2").GetComponent<Image>().sprite = GameObject.Find("Player_Head").GetComponent<Image>().sprite;
            GameObject.Find("img_0").GetComponent<Image>().sprite = GameObject.Find("Left_Head").GetComponent<Image>().sprite;
            GameObject.Find("img_1").GetComponent<Image>().sprite = GameObject.Find("Right_Head").GetComponent<Image>().sprite;

            NetManager.left_score += NetManager.scoreList[0];

            NetManager.right_score += NetManager.scoreList[1];
        }
  
       
        NetManager.player_score += NetManager.scoreList[NetManager.index];
    
        //改变UI积分显示
        NetManager.change_score = true;
    }
    /// <summary>
    /// 添加+符号
    /// </summary>
    /// <param name="score"></param>
    /// <returns></returns>
    public string getText(int score)
    {
        if (score > 0)
        {
            return "+" + score.ToString();
        }
        else
            return score.ToString();
    }
}
