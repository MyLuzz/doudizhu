using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// 网络模块
/// </summary>
public class NetManager: MonoBehaviour
{
    public Demo demo;

    public static NetManager Instance = null;

    public static ClientPeer client;
    // 192.168.191.1  61.164.248.190
    public static bool CreateNewSocket = false;
    public static bool sign_CreateNewSocket = true;

    public static int login_state = 0;
    public static bool setOne = true;
    public static bool setTwo = true;
    public static bool setThree = true;
    public static bool setName = true;

    public static byte[] recon;//重连
    public static byte[] uncon;//断线
    public static byte[] login;//登录

    public static string token;

    //三人准备执行一次标志
    public static bool create = true;

    //创建房间信息
	public static string RoomNumber;

    //代表登录次数
    public static int pos = 0;

    public static PlayerBackData pbd;
    //初始化手牌的数组
    public static List<Card> cardlist;
    //重连后得到的玩家手牌数组
    public static List<Card> recon_cardlist;
    public static List<bool> readylist =  new List<bool>{false,false,false};

    public static string account;
    public static string accName;
    public static string avator;
    public static string sex;
    //叫分玩家座位号
    public static int turn = -2;
    //加入玩家座位号
    public static int index = -1;
    public static int playerNum = 0;
   // public static bool setIndex = true;
    public static bool setHost = false;
    public static int host_num = 0;

    public static List<byte> external_card_list;
    public static bool game_begin = false;//开始游戏标记
    public static bool game_over = true;
    public static bool over = false;
    public static int landlord = -1;
    //可以叫分
    public static bool canCall = false;
    public static int multiple = 1;
    ////显示抢地主 不抢的按钮UI一次标志
    //public static bool clickDisgrab = true;
    public static int lastPlayer = -2;

    public static int currPlayer = -1;

    public static int nextPlayer = -1;
    //显示卡牌UI用的数组
    public static List<byte> current_card_list = null;

    //存储在房间内的玩家信息
    public static player[] player_array = new player[3];
    //总局数
    public static int Inning;
    //剩余局数
    public static int remian_nning;
    //保存支付方式
    public static int payment = -1;
    //保存底牌显示方式
    public static bool show_extra = true;
    //设置一次玩家准备信息
    public static bool setReady = false;
    //房卡数
    public static int roomcard = 0;
    //更新房卡状态一次
    public static bool setRoomCard = true;
    public static string playerName;
    //每次接收到叫分消息 更新一次UI显示
    public static bool sign_setscore = false;
    public static int lastBet = -1;
    public static int maxBet = -1;

    public static List<int> scoreList = new List<int> { 0, 0, 0 };
    public static List<int> allscoreList = new List<int> { 0, 0, 0 };
    public static List<int> landlordList = new List<int> { 0, 0, 0 };
    public static List<int> reconscorelist = new List<int> { 0, 0, 0 };
    //改变一次UI上的积分
    public static bool sign_reconscore = false;
    public static bool StartListen = false;

    public static int left_score = 0;
    public static int right_score = 0;
    public static int player_score = 0;
    public static int left_lan = 0;
    public static int right_lan = 0;
    public static int player_lan = 0;
    //改变一次
    public static bool change_score = false;
    public static bool change_allscore = false;
    public static bool change_roomnumber = false;
    //游戏结束只访问一次
    public static bool sign_over = true;
    //有人没出牌 播放不出音效
    public static bool someone_pass = false;

    public static bool baojing1 = true;
    public static bool baojing2 = true;

    //玩家位置 0 大厅 1 游戏中(未开始) 2 游戏开始
    public static int player_pos = -1;

    public static bool bgm_open = true;

    //回合指示器向量
    public static Vector3 rot = new Vector3(0, 0, -90);
    //改变一次回合指示器标志
    public static bool sign_turn_index = false;
    //第一个人出牌时候的倒计时显示
    public static bool first_play = true;
    public static int enter_state = 0;
    public static int playcard_state = 0;
    public static int passcard_state = 0;

    public static bool sign_uncon = false;

    //判断是否需要刷新场景
    public static bool refresh_0 = false;
    public static bool refresh_1 = false;
    public static bool refresh_2 = false;
    public static bool refresh_3 = false;

    //上下家手牌数
    public static int lastcardnum;
    public static int nextcardnum;
    //登录提示
    public GameObject login_tip;
    //登录是否成功
    public static bool login_success = false;
    public SoundManager soundManager;

    //判断当前的网络状态
    public static bool set_NotReachable = true;
    public static bool reachable = true;

    public static byte[] reach_message;

    //离线的id
    public static string offline_id;
    //离线玩家的座位号
    public static int offline_pos = -1;
    //刷新一次房间信息
    public static bool sign_offline = false;
    //离线之后的重连
    public static bool sign_off_recon = false;
    //关闭房间的标志位
    public static bool sign_closeroom = false;
    public static byte[] leave_message;
    //改变一次局数
    public static bool sign_setRound = false;
    //重连隐藏出牌不出按钮
    public static bool recon_deact = false;

    void Update()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable && set_NotReachable)
        {
            reachable = false;
            set_NotReachable = false;
        }
        if (reachable == false)
        {
            if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                reachable = true;
                //重连
                client.socket.Close();
                client = new ClientPeer("61.164.248.190", 4396);
                client.Connect();
                reach_message = EncodeTool.ReconEncode(101, NetManager.account, 0, NetManager.token);
                NetManager.client.Send(reach_message);
                set_NotReachable = true;
            }
            else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
            {
                reachable = true;
                //重连
                client.socket.Close();
                client = new ClientPeer("61.164.248.190", 4396);
                client.Connect();
                reach_message = EncodeTool.ReconEncode(101, NetManager.account, 0, NetManager.token);
                NetManager.client.Send(reach_message);
                set_NotReachable = true;
            }
        }
      
            if (login_success)
            {
                login_tip.SetActive(false);
                login_success = false;
                GameObject.Find("LoginPanel").SetActive(false);
                soundManager.mc_welcome.Play();
            }
            if (CreateNewSocket && sign_CreateNewSocket)
            {
               

                Debug.Log("state错误");
                //uncon = EncodeTool.ReconEncode(100, NetManager.account, 0, NetManager.token);
                //NetManager.client.Send(uncon);
                //sign_CreateNewSocket = false;

                StartCoroutine(waitForRecon());

                CreateNewSocket = false;
            }
            if (sign_uncon)
            {
                //client.socket.Close();
                //client = new ClientPeer("61.164.248.190", 4396);
                //client.Connect();
                //login = EncodeTool.UserEncode(account, accName, sex, avator, 0, token);
                //client.Send(login);
                sign_uncon = false;

                
            }
            setPlayer();
            if (sign_offline)
            {
                setOffPlayer();
            }
            
            
            if (setReady)
            {
                setReadyImg();
            }

            if (setHost)
            {
                setHostText();
            }

            //if (client == null)
            //    return;

            //while (client.SocketMsgQueue.Count > 0)
            //{
            //    SocketMsg msg = client.SocketMsgQueue.Dequeue();
            //    //处理消息
            //   // processSocketMsg(msg);
            //}
        }
    IEnumerator waitForRecon()
    {
        yield return new WaitForSeconds(3.0f);
        client.socket.Close();
        client = new ClientPeer("61.164.248.190", 4396);
        client.Connect();
        recon = EncodeTool.ReconEncode(101, account, 0, token);
        client.Send(recon);
        login_success = true;
    }
    public void setReadyImg()
    {
        if (index == 0)
        {
            if(readylist[0] == true)
                GameObject.Find("Player").GetComponent<Transform>().GetChild(2).gameObject.SetActive(true);
            if (readylist[1] == true)
                GameObject.Find("ComputerLeft").GetComponent<Transform>().GetChild(2).gameObject.SetActive(true);
            if (readylist[2] == true)
                GameObject.Find("ComputerRight").GetComponent<Transform>().GetChild(2).gameObject.SetActive(true);
        }
        else if (index == 1)
        {
            if (readylist[1] == true)
                GameObject.Find("Player").GetComponent<Transform>().GetChild(2).gameObject.SetActive(true);
            if (readylist[0] == true)
                GameObject.Find("ComputerRight").GetComponent<Transform>().GetChild(2).gameObject.SetActive(true);
            if (readylist[2] == true)
                GameObject.Find("ComputerLeft").GetComponent<Transform>().GetChild(2).gameObject.SetActive(true);
        }
        else if (index == 2)
        {
            if (readylist[2] == true)
                GameObject.Find("Player").GetComponent<Transform>().GetChild(2).gameObject.SetActive(true);
            if (readylist[1] == true)
                GameObject.Find("ComputerRight").GetComponent<Transform>().GetChild(2).gameObject.SetActive(true);
            if (readylist[0] == true)
                GameObject.Find("ComputerLeft").GetComponent<Transform>().GetChild(2).gameObject.SetActive(true);
        }
        setReady = false;
    }
  

    public void setHostText()
    {
            if (index == 0)
            {
                if (playerNum == 2)
                {
                    if (NetManager.player_array[1] == null)
                    {
                        GameObject.Find("ComputerRight").GetComponent<Transform>().GetChild(1).gameObject.SetActive(false);
                    }
                    else
                        GameObject.Find("ComputerLeft").GetComponent<Transform>().GetChild(1).gameObject.SetActive(false);
                }
                if (playerNum == 3)
                {
                    GameObject.Find("ComputerRight").GetComponent<Transform>().GetChild(1).gameObject.SetActive(false);
                    GameObject.Find("ComputerLeft").GetComponent<Transform>().GetChild(1).gameObject.SetActive(false);
                }
                
                if (host_num == 0)
                {
                    GameObject.Find("Player").GetComponent<Transform>().GetChild(1).gameObject.SetActive(true);
                }
                else if(host_num == 1)
                {
                    GameObject.Find("ComputerLeft").GetComponent<Transform>().GetChild(1).gameObject.SetActive(true);
                }
                else if (host_num == 2)
                {
                    GameObject.Find("ComputerRight").GetComponent<Transform>().GetChild(1).gameObject.SetActive(true);
                }
                   
            }
            else if (index == 1)
            {
                if (playerNum == 2)
                {
                    if (NetManager.player_array[2] == null)
                    {
                        GameObject.Find("ComputerRight").GetComponent<Transform>().GetChild(1).gameObject.SetActive(false);
                    }
                    else
                        GameObject.Find("ComputerLeft").GetComponent<Transform>().GetChild(1).gameObject.SetActive(false);
                }
                if (playerNum == 3)
                {
                    GameObject.Find("ComputerRight").GetComponent<Transform>().GetChild(1).gameObject.SetActive(false);
                    GameObject.Find("ComputerLeft").GetComponent<Transform>().GetChild(1).gameObject.SetActive(false);
                }
                
                if (host_num == 0)
                {
                    GameObject.Find("ComputerRight").GetComponent<Transform>().GetChild(1).gameObject.SetActive(true);
                }
                else if (host_num == 1)
                {
                    GameObject.Find("Player").GetComponent<Transform>().GetChild(1).gameObject.SetActive(true);
                }
                else if (host_num == 2)
                {
                    GameObject.Find("ComputerLeft").GetComponent<Transform>().GetChild(1).gameObject.SetActive(true);
                }

            }
            else if (index == 2)
            {
                if (playerNum == 2)
                {
                    if (NetManager.player_array[0] == null)
                    {
                        GameObject.Find("ComputerRight").GetComponent<Transform>().GetChild(1).gameObject.SetActive(false);
                    }
                    else
                        GameObject.Find("ComputerLeft").GetComponent<Transform>().GetChild(1).gameObject.SetActive(false);
                }
                if (playerNum == 3)
                {
                    GameObject.Find("ComputerRight").GetComponent<Transform>().GetChild(1).gameObject.SetActive(false);
                    GameObject.Find("ComputerLeft").GetComponent<Transform>().GetChild(1).gameObject.SetActive(false);
                }
                
                if (host_num == 0)
                {
                    GameObject.Find("ComputerLeft").GetComponent<Transform>().GetChild(1).gameObject.SetActive(true);
                }
                else if (host_num == 1)
                {
                    GameObject.Find("ComputerRight").GetComponent<Transform>().GetChild(1).gameObject.SetActive(true);
                }
                else if (host_num == 2)
                {
                    GameObject.Find("Player").GetComponent<Transform>().GetChild(1).gameObject.SetActive(true);
                }

            }
            setHost = false;
    }
    /// <summary>
    /// 设置房间内的玩家信息
    /// </summary>
    public void setPlayer()
    {
        if (setName)
        {
            //显示玩家的昵称
            GameObject.Find("Player_Name").GetComponent<Text>().text = NetManager.accName;
            setName = false;
        }
       
        if (playerNum == 1 && setOne)
        {
            GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(0).gameObject.SetActive(false);
            GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(1).gameObject.SetActive(false);
            setOne = false;
        }
        if (playerNum == 2 && setTwo)
        {
            if(index == 0)
            {
                if (player_array[1] == null)
                {
                    GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(1).gameObject.SetActive(true);
                    GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(0).gameObject.SetActive(false);
                    GameObject.Find("Right_Name").GetComponent<Text>().text = player_array[2].nickname;
                    AsyncImageDownload.Instance.Init();
                    AsyncImageDownload.Instance.SetAsyncImage(player_array[2].avator, GameObject.Find("Right_Head").GetComponent<Image>());
                }
                else
                {
                    GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(0).gameObject.SetActive(true);
                    GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(1).gameObject.SetActive(false);
                    GameObject.Find("Left_Name").GetComponent<Text>().text = player_array[1].nickname;
                    AsyncImageDownload.Instance.Init();
                    AsyncImageDownload.Instance.SetAsyncImage(player_array[1].avator, GameObject.Find("Left_Head").GetComponent<Image>());
                }
            }
            if (index == 1)
            {
                if (player_array[2] == null)
                {
                    GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(1).gameObject.SetActive(true);
                    GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(0).gameObject.SetActive(false);
                    GameObject.Find("Right_Name").GetComponent<Text>().text = player_array[0].nickname;
                    AsyncImageDownload.Instance.Init();
                    AsyncImageDownload.Instance.SetAsyncImage(player_array[0].avator, GameObject.Find("Right_Head").GetComponent<Image>());
                }
                else
                {
                    GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(0).gameObject.SetActive(true);
                    GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(1).gameObject.SetActive(false);
                    GameObject.Find("Left_Name").GetComponent<Text>().text = player_array[2].nickname;
                    AsyncImageDownload.Instance.Init();
                    AsyncImageDownload.Instance.SetAsyncImage(player_array[2].avator, GameObject.Find("Left_Head").GetComponent<Image>());
                }
            }
            if (index == 2)
            {
                if (player_array[0] == null)
                {
                    GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(1).gameObject.SetActive(true);
                    GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(0).gameObject.SetActive(false);
                    GameObject.Find("Right_Name").GetComponent<Text>().text = player_array[1].nickname;
                    AsyncImageDownload.Instance.Init();
                    AsyncImageDownload.Instance.SetAsyncImage(player_array[1].avator, GameObject.Find("Right_Head").GetComponent<Image>());
                }
                else
                {
                    GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(0).gameObject.SetActive(true);
                    GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(1).gameObject.SetActive(false);
                    GameObject.Find("Left_Name").GetComponent<Text>().text = player_array[0].nickname;
                    AsyncImageDownload.Instance.Init();
                    AsyncImageDownload.Instance.SetAsyncImage(player_array[0].avator, GameObject.Find("Left_Head").GetComponent<Image>());
                }
            }
            setTwo = false;
        }
        if (playerNum == 3 && setThree)
        {
            GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(0).gameObject.SetActive(true);
            GameObject.Find("CharacterPanel").GetComponent<Transform>().GetChild(1).gameObject.SetActive(true);
            if (index == 0)
            {
                GameObject.Find("Left_Name").GetComponent<Text>().text = player_array[1].nickname;
                GameObject.Find("Right_Name").GetComponent<Text>().text = player_array[2].nickname;
                AsyncImageDownload.Instance.Init();
                AsyncImageDownload.Instance.SetAsyncImage(player_array[1].avator, GameObject.Find("Left_Head").GetComponent<Image>());
                AsyncImageDownload.Instance.Init();
                AsyncImageDownload.Instance.SetAsyncImage(player_array[2].avator, GameObject.Find("Right_Head").GetComponent<Image>());
            }
            if (index == 1)
            {
                GameObject.Find("Left_Name").GetComponent<Text>().text = player_array[2].nickname;
                GameObject.Find("Right_Name").GetComponent<Text>().text = player_array[0].nickname;
                AsyncImageDownload.Instance.Init();
                AsyncImageDownload.Instance.SetAsyncImage(player_array[2].avator, GameObject.Find("Left_Head").GetComponent<Image>());
                AsyncImageDownload.Instance.Init();
                AsyncImageDownload.Instance.SetAsyncImage(player_array[0].avator, GameObject.Find("Right_Head").GetComponent<Image>());
            }
            if (index == 2)
            {
                GameObject.Find("Left_Name").GetComponent<Text>().text = player_array[0].nickname;
                GameObject.Find("Right_Name").GetComponent<Text>().text = player_array[1].nickname;
                AsyncImageDownload.Instance.Init();
                AsyncImageDownload.Instance.SetAsyncImage(player_array[0].avator, GameObject.Find("Left_Head").GetComponent<Image>());
                AsyncImageDownload.Instance.Init();
                AsyncImageDownload.Instance.SetAsyncImage(player_array[1].avator, GameObject.Find("Right_Head").GetComponent<Image>());
            }
            setThree = false;
        }
    }
   /// <summary>
   /// 玩家离线
   /// </summary>
    public void setOffPlayer()
    {
        Sprite spr_offline = Resources.Load<Sprite>("off_line");
        for (int i = 0; i < 3; i++)
        {
            if (player_array[i] != null)
            {
                if (player_array[i].id == offline_id)
                {
                    NetManager.offline_pos = i;
                    if (index == 0)
                    {
                        if (i == 1)
                        {
                            GameObject.Find("ComputerLeft").GetComponent<Transform>().GetChild(0).gameObject.GetComponent<Image>().sprite = spr_offline;
                        }
                        else if (i == 2)
                        {
                            GameObject.Find("ComputerRight").GetComponent<Transform>().GetChild(0).gameObject.GetComponent<Image>().sprite = spr_offline;
                        }
                    }
                    else if (index == 1)
                    {
                        if (i == 2)
                        {
                            GameObject.Find("ComputerLeft").GetComponent<Transform>().GetChild(0).gameObject.GetComponent<Image>().sprite = spr_offline;
                        }
                        else if (i == 0)
                        {
                            GameObject.Find("ComputerRight").GetComponent<Transform>().GetChild(0).gameObject.GetComponent<Image>().sprite = spr_offline;
                        }
                    }
                    if (index == 2)
                    {
                        if (i == 0)
                        {
                            GameObject.Find("ComputerLeft").GetComponent<Transform>().GetChild(0).gameObject.GetComponent<Image>().sprite = spr_offline;
                        }
                        else if (i == 1)
                        {
                            GameObject.Find("ComputerRight").GetComponent<Transform>().GetChild(0).gameObject.GetComponent<Image>().sprite = spr_offline;
                        }
                    }
                }
            }
        }
    }
    #region 处理接收到的服务器发来的消息

//    HandlerBase accountHandler = new AccoutHandler();
//
//    /// <summary>
//    /// 接受网络的消息
//    /// </summary>
//    private void processSocketMsg(SocketMsg msg)
//    {
//        switch (msg.OpCode)
//        {
//            case OpCode.ACCOUNT:
//                accountHandler.OnReceive(msg.SubCode, msg.Value);
//                break;
//            default:
//                break;
//        }
//    }

    #endregion


    #region 处理客户端内部 给服务器发消息的 事件

//    private void Awake()
//    {
//        Instance = this;
//
//        Add(0, this);
//    }
//
//    public override void Execute(int eventCode, object message)
//    {
//        switch (eventCode)
//        {
//            case 0:
//                client.Send(message as SocketMsg);
//                break;
//            default:
//                break;
//        }
//    }

    #endregion

}

