using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using strange.extensions.mediation.impl;

public class InteractionView : View
{
    public Button btn_Deal;
    public Button btn_Play;
    public Button btn_Pass;
    public Button btn_Grab1;
    public Button btn_Grab2;
    public Button btn_Grab3;

    //倍数显示
    public Text player_x0;
    public Text player_x1;
    public Text player_x2;
    public Text left_x0;
    public Text left_x1;
    public Text left_x2;
    public Text right_x0;
    public Text right_x1;
    public Text right_x2;

    public Toggle tog_bgm;
    public Toggle tog_sound;

    public Button btn_Disgrab;

    public Button game_close;
	public Button btn_Setting;
	public GameObject Setting;
	public Button btn_Exit;
	
    //public InputField RoundTimes;
    //public Button Confirm;
    //public Text Times;
    //public Button Times1;
    //public Button Times2;
    //public Button Times3;
    //public Button Times4;
    //public Button Times5;
    //public Button UpToYou;
	public GameObject Information;
	public Text CurrScore;
	public Text CurrRanking;
	public Text Rounds;
	public Text WinRate;
	public Text NextRanking;
	public bool information = false;
    public Text RoomNumber;
    public Text txt_multiple;
    public Button btn_Leave;
    public PlayerControl Player;

    public DeskControl Desk;

    public ComputerControl Left;
    public ComputerControl Right;

    public Text Remain_Inning;

    public Image round_num;
    public CanvasGroup cg_Round;

    public SoundManager soundManager;

    public Image main_exit_panel;
    public Button main_exit_yes;
    public Button main_exit_no;
    //轮盘
    public GameObject turn_index;
    public GameObject show_turn;
    //底牌显示
    public Image extra_card1;
    public Image extra_card2;
    public Image extra_card3;

    //投票解散房间
    public Button btn_vote;
    public Image img_vote;
    public Button btn_agree;
    public Button btn_disagree;
    public Button vote_close;
    //提示房间已经解散的面板
    public Image roomclose_tip_panel;
    public Button btn_backstart;
    public CanvasGroup cg_roomclose;

    public Transform standrand;
	public void ShowInformation()
	{
		information = !information;
		Information.SetActive (information);
       
        }

	public void CloseInformation()
	{
		Information.SetActive (false);
	}

    public void RoundShowAnim()
    {
        cg_Round.alpha = 0f;
        StartCoroutine(RoundAnim(NetManager.Inning + 1 - NetManager.remian_nning));
    }

    /// <summary>
    /// 渐隐动画
    /// </summary>
    /// <returns></returns>
    IEnumerator RoundAnim(int round)
    {
        string str_round = "round" + round.ToString();
        Image img_round = round_num.GetComponent<Image>();
        img_round.sprite = Resources.Load<Sprite>(str_round);
        img_round.SetNativeSize();
        float time = 1f;

        while (time >= 0f)
        {
            yield return new WaitForSeconds(0.1f);
            time -= 0.2f;
            cg_Round.alpha += 0.2f;
        }
        float time1 = 1f;
        
        while (time1 >= 0f)
        {
            yield return new WaitForSeconds(0.1f);
            time1 -= 0.25f;
            cg_Round.alpha -= 0.25f;
        }
    }

    

    ///// <summary>
    ///// 显示选择局数的按钮
    ///// </summary>
    //public void ActiveTimesBtn()
    //{
    //    Times1.gameObject.SetActive (true);
    //    Times2.gameObject.SetActive (true);
    //    Times3.gameObject.SetActive (true);
    //    Times4.gameObject.SetActive (true);
    //    Times5.gameObject.SetActive (true);
    //    UpToYou.gameObject.SetActive (true); 
    //}

    ///// <summary>
    ///// 隐藏选择局数的按钮
    ///// </summary>
    //public void DeactiveTimesBtn()
    //{
    //    Times1.gameObject.SetActive (false);
    //    Times2.gameObject.SetActive (false);	
    //    Times3.gameObject.SetActive (false);	
    //    Times4.gameObject.SetActive (false);	
    //    Times5.gameObject.SetActive (false);
    //    UpToYou.gameObject.SetActive (false); 
    //    RoundTimes.gameObject.SetActive (false);
    //    Confirm.gameObject.SetActive (false);
    //}

    /// <summary>
    /// 全部隐藏按钮
    /// </summary>
    public void DeactiveAll()
    {
        btn_Deal.gameObject.SetActive(false);
        btn_Play.gameObject.SetActive(false);
        btn_Pass.gameObject.SetActive(false);
        btn_Grab1.gameObject.SetActive(false);
        btn_Grab2.gameObject.SetActive(false);
        btn_Grab3.gameObject.SetActive(false);
        btn_Disgrab.gameObject.SetActive(false);

    }


    /// <summary>
    /// 显示发牌按钮
    /// </summary>
    public void ActiveDeal()
    {
        btn_Deal.gameObject.SetActive(true);
        btn_Play.gameObject.SetActive(false);
        btn_Pass.gameObject.SetActive(false);
        btn_Grab1.gameObject.SetActive(false);
        btn_Grab2.gameObject.SetActive(false);
        btn_Grab3.gameObject.SetActive(false);
        btn_Disgrab.gameObject.SetActive(false);
    }

    /// <summary>
    /// 显示出牌按钮
    /// </summary>
    public void ActivePlayAndPass(bool canPass = true)
    {
        btn_Deal.gameObject.SetActive(false);
        btn_Play.gameObject.SetActive(true);
        btn_Pass.gameObject.SetActive(true);
        btn_Pass.interactable = canPass;
        btn_Grab1.gameObject.SetActive(false);
        btn_Grab2.gameObject.SetActive(false);
        btn_Grab3.gameObject.SetActive(false);
        btn_Disgrab.gameObject.SetActive(false);
    }

    /// <summary>
    /// 显示抢地主按钮
    /// </summary>
    public void ActiveGrabAndDisgrab()
    {
        btn_Deal.gameObject.SetActive(false);
        btn_Play.gameObject.SetActive(false);
        btn_Pass.gameObject.SetActive(false);
        btn_Grab1.gameObject.SetActive(true);
        btn_Grab2.gameObject.SetActive(true);
        btn_Grab3.gameObject.SetActive(true);
        btn_Disgrab.gameObject.SetActive(true);
    }

    public void ShowRoomCloseAnim()
    {
        StartCoroutine(IE_Room_Anim(cg_roomclose));
    }
    IEnumerator IE_Room_Anim(CanvasGroup cg)
    {
        float time = 1f;

        while (time >= 0f)
        {
            yield return new WaitForSeconds(0.1f);
            time -= 0.2f;
            cg.alpha += 0.2f;
        }

        yield return new WaitForSeconds(1f);
        float time1 = 1f;

        while (time1 >= 0f)
        {
            yield return new WaitForSeconds(0.1f);
            time1 -= 0.25f;
            cg.alpha -= 0.25f;
        }
    }
}
