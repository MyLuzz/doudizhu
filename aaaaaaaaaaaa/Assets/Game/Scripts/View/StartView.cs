using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using strange.extensions.mediation.impl;

public class StartView : EventView
{
	public  Button btn_Enter;
	public Button btn_Create;
	public Button Exit;
	public Button QQCancelLogin;
	public Button WeChatCancelLogin;

    public CanvasGroup room_full;
    public CanvasGroup room_empty;
    private byte[] message1;

	public Image imgEnter;
	public Button btn_0;
	public Button btn_1;
	public Button btn_2;
	public Button btn_3;
	public Button btn_4;
	public Button btn_5;
	public Button btn_6;
	public Button btn_7;
	public Button btn_8;
	public Button btn_9;
	public InputField InpNumber;
	public Button btn_confirm;
	public Button btn_cancel;
    public Button btn_closeEnter;
    public Image Option;
    //选择局数
    public ToggleGroup tg1;
    //选择支付方式
    public ToggleGroup tg2;
    //选择底牌显示方式
    public ToggleGroup tg3;
    public Button btn_finish;
    public Button btn_closeCreate;
    
    public Button btn_Pay;
    public Button share_wechat;
    public Button share_moments;
    public Button share_qq;
    public Button share_qzone;

    public Button btn_Share;

    public Image setting_panel;
    public Image exit_panel;
    public Button exit_yes;
    public Button exit_no;

    public GameObject Tip;
    public bool open_tips = false;
    public Button btn_tip;

    public Button main_close;
    public Button main_setting;

    public SoundManager soundManager;

    public PlayerControl player;
    public ComputerControl left;
    public ComputerControl right;
    public DeskControl desk;

    //轮盘
    public GameObject turn_index;
    public GameObject show_turn;

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

    public Text txt_multiple;
    public Text Remain_Inning;

    public Button btn_deal;
    public Image img_ready;
    /// <summary>
    /// 初始化获取组件等
    /// </summary>
    public void Init()
    {
		btn_Enter = transform.Find("btn_Enter").GetComponent<Button>();
        btn_Create = transform.Find("btn_Create").GetComponent<Button>();
        //注册点击事件
        btn_Create.onClick.AddListener(onCreateClick);
        btn_finish.onClick.AddListener(room_Create);
        btn_closeCreate.onClick.AddListener(closeCreate);
        btn_tip.onClick.AddListener(OpenTips);

        main_setting.onClick.AddListener(OpenMainSetting);
        main_close.onClick.AddListener(CloseMainSetting);
    }

    /// <summary>
    /// 移除点击事件
    /// </summary>
    public void ViewDestroy()
    {
        btn_Create.onClick.RemoveListener(onCreateClick);
        btn_finish.onClick.RemoveListener(room_Create);
        btn_closeCreate.onClick.RemoveListener(closeCreate);
        btn_tip.onClick.RemoveListener(OpenTips);

        main_setting.onClick.RemoveListener(OpenMainSetting);
        main_close.onClick.RemoveListener(CloseMainSetting);
    }
    public void CloseMainSetting()
    {
        soundManager.mc_btn_click.Play();
        setting_panel.gameObject.SetActive(false);
    }
    public void OpenMainSetting()
    {
        soundManager.mc_btn_click.Play();
        setting_panel.gameObject.SetActive(true);
    }
    public void OpenTips()
    {
        soundManager.mc_btn_click.Play();
        open_tips = !open_tips;
        Tip.SetActive(open_tips);
    }
    public void room_Create()
    {
        soundManager.mc_btn_click.Play();
         int round = 0;
         string pay_way = "";
         int payway = -1;
         for (int i = 0; i < 4; i++)
         {
             if (tg1.GetComponent<Transform>().GetChild(i).gameObject.GetComponent<Toggle>().isOn)
             {
                 round = int.Parse(tg1.GetComponent<Transform>().GetChild(i).GetChild(1).gameObject.GetComponent<Text>().text);
             }
         }
         for (int i = 0; i < 2; i++)
         {
             if (tg2.GetComponent<Transform>().GetChild(i).gameObject.GetComponent<Toggle>().isOn)
             {
                 pay_way = tg2.GetComponent<Transform>().GetChild(i).GetChild(1).name;
             }
         }
         if (pay_way == "tog_host")
         {
             payway = 0;
         }
         else
             payway = 1;

         if (tg3.GetComponent<Transform>().GetChild(0).gameObject.GetComponent<Toggle>().isOn)
         {
             NetManager.show_extra = true;
         }
         else
             NetManager.show_extra = false;
         message1 = EncodeTool.CreateRoomEncode(NetManager.account, 0,round,payway,NetManager.show_extra);
         NetManager.client.Send(message1);
         GameObject.Find("StartPanel").SetActive(false);

         Option.gameObject.SetActive(false);
         soundManager.mc_welcome.Stop();
         soundManager.mc_normal.Play();
    }


    /// <summary>
    /// 双倍按钮点击
    /// </summary>
    private void onCreateClick()
    {
        ////更改Intergration的倍数为2
        //dispatcher.Dispatch(ViewEvent.CHANGE_MULTIPLE, 2);
        ////删除面板
        soundManager.mc_btn_click.Play();
        Debug.Log("创建房间");

//		Tools.CreateUIPanel(PanelType.BackgroundPanel);
//		Tools.CreateUIPanel(PanelType.CharacterPanel);
//		Tools.CreateUIPanel(PanelType.InteractionPanel);
        Option.gameObject.SetActive(true);
       

    }

    public void closeCreate()
    {
        soundManager.mc_btn_click.Play();
        Option.gameObject.SetActive(false);
    }
    IEnumerator wait_enter()
    {
        yield return new WaitForSeconds(0.5f);
        if (NetManager.enter_state == 0)
        {
            //进入房间成功 清空房间内的数据
            player_x0.gameObject.SetActive(false);
            player_x1.gameObject.SetActive(false);
            player_x2.gameObject.SetActive(false);
            left_x0.gameObject.SetActive(false);
            left_x1.gameObject.SetActive(false);
            left_x2.gameObject.SetActive(false);
            right_x0.gameObject.SetActive(false);
            right_x1.gameObject.SetActive(false);
            right_x2.gameObject.SetActive(false);
            show_turn.SetActive(false);
            turn_index.SetActive(false);
            txt_multiple.text = "";
            player.Clear();
            left.Clear();
            right.Clear();
            desk.Clear();
            Remain_Inning.text = "";

            if (NetManager.readylist[NetManager.index] == false)
            {
                btn_deal.gameObject.SetActive(true);
            }
            else
                img_ready.gameObject.SetActive(true);

            NetManager.RoomNumber = InpNumber.text;
            NetManager.change_roomnumber = true;
            GameObject.Find("StartPanel").SetActive(false);
            soundManager.mc_welcome.Stop();
            soundManager.mc_normal.Play();
            soundManager.mc_btn_click.Play();

            imgEnter.gameObject.SetActive(false);

            NetManager.setOne = true;
            NetManager.setTwo = true;
            NetManager.setThree = true;
            InpNumber.text = "";
        }
        else if (NetManager.enter_state == 262150)
        {
             StartCoroutine(IE_Room_Anim(room_full));
        }
        else
            StartCoroutine(IE_Room_Anim(room_empty));
         
    }

    public void waitLoad()
    {
        StartCoroutine(wait_enter());
    }
    /// <summary>
    /// 房间已满提示动画
    /// </summary>
    /// <returns></returns>
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

