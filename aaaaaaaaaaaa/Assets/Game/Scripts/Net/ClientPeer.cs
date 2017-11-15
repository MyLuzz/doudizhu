using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using LitJson;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using System.Threading;
using System.Web.Script.Serialization;
using System.IO;
using System.Text;
using Newtonsoft.Json;
/// <summary>
/// 客户端socket的封装
/// </summary>
public class ClientPeer:EventMediator
{
    public  Socket socket;

    private string ip;
    private int port;

	private Thread t;



    [Inject]
    public RoundModel RoundModel { get; set; }
    /// <summary>
    /// 构造连接对象
    /// </summary>
    /// <param name="ip">IP地址</param>
    /// <param name="port">端口号</param>
    public ClientPeer(string ip, int port)
    {
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.ip = ip;
            this.port = port;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public bool GetState()
    {
        return socket.Connected;
    }

    public void Connect()
    {
        try
        {
            socket.Connect(ip, port);
            Debug.Log("连接服务器成功！");

            startReceive();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }


    #region 接受数据

    //接受的数据缓冲区
    private byte[] receiveBuffer = new byte[1024];

    /// <summary>
    /// 一旦接收到数据 就存到缓存区里面
    /// </summary>
    private List<byte> dataCache = new List<byte>();

    private bool isProcessReceive = false;

    public static string str;

    public Queue<SocketMsg> SocketMsgQueue = new Queue<SocketMsg>();

    public Queue<string> message_queue = new Queue<string>();

    public OutLog olg = GameObject.Find("OutLog").GetComponent<OutLog>();
    /// <summary>
    /// 开始异步接受数据
    /// </summary>
    private void startReceive()
    {
        if (socket == null && socket.Connected == false)
        {
            Debug.LogError("没有连接成功，无法发送数据");
            return;
        }

        socket.BeginReceive(receiveBuffer, 0, 1024, SocketFlags.None, receiveCallBack, socket);
    }

    public static T DeserializeJsonToObject<T>(string json) where T : class
    {
        JsonSerializer serializer = new JsonSerializer();
        StringReader sr = new StringReader(json);
        object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
        T t = o as T;
        return t;
    }

    /// <summary>
    /// 收到消息的回调
    /// </summary>
    private void receiveCallBack(IAsyncResult ar)
    {
        
        try
        {
            int length = socket.EndReceive(ar);
            
            byte[] tmpByteArray = new byte[length];
            Buffer.BlockCopy(receiveBuffer, 0, tmpByteArray, 0, length);
            string str = System.Text.Encoding.UTF8.GetString(tmpByteArray);
            
            message_queue.Enqueue(str);
           
            olg.getMW().Add(str);

            if (message_queue.Count > 0)
            {
                str = message_queue.Dequeue();
                processData(str);
                Debug.Log(str);
            }
 
     
			
            //处理收到的数据
            //dataCache.AddRange(tmpByteArray);
            //if (isProcessReceive == false)
            //{
            //    processReceive();
            //}

            startReceive();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }


    /// <summary>
    /// 处理收到的数据
    /// </summary>
    private void processReceive()
    {
        isProcessReceive = true;

        
   
       
        ////解析数据包
        //byte[] data = EncodeTool.DecodePacket(ref dataCache);

        //if (data == null)
        //{
        //    isProcessReceive = false;
        //    return;
        //}

        //SocketMsg msg = EncodeTool.DecodeMsg(data);
        ////存储消息 等待处理
        //Debug.Log(msg.ToString());
        //SocketMsgQueue.Enqueue(msg);

        //尾递归
        processReceive();
    }

    public void processData(string str)
    {
        switch (str.hashtableFromJson()["action"].ToString())
        {
            case "1":
                PlayerBackData pbd = DeserializeJsonToObject<PlayerBackData>(str);
                NetManager.roomcard = pbd.room_card;

                NetManager.login_state = pbd.state;
                if (NetManager.login_state == 196611)
                {
                    NetManager.CreateNewSocket = true;
                }
                else
                    NetManager.login_success = true;
                NetManager.player_pos = 0;
                break;
            case "3":
                NetManager.RoomNumber = str.hashtableFromJson()["room_id"].ToString();
                NetManager.change_roomnumber = true;
                //NetManager.hos
                NetManager.playerNum = 1;
                NetManager.player_pos = 1;
                NetManager.setHost = true;
                break;
            case "4":
                //实例化一个能够序列化数据的类

                EnterBackData ebd1 = DeserializeJsonToObject<EnterBackData>(str);
                Debug.Log("加入房间");
                NetManager.enter_state = ebd1.state;
                NetManager.show_extra = ebd1.excard_visible;
                if (ebd1.state == 0)
                {
                    player[] player_array = ebd1.player_list.ToArray();
                    NetManager.player_array = player_array;
                    List<bool> ready = new List<bool> { false, false, false };
                    int playerNum = 0;
                    for (int i = 0; i < player_array.Length; i++)
                    {
                        if (player_array[i] != null)
                        {
                            playerNum++;
                            ready[i] = player_array[i].ready;
                        }
                    }
                    NetManager.playerNum = playerNum;
                    NetManager.readylist = ready;
                    for (int i = 0; i < player_array.Length; i++)
                    {
                        if (player_array[i] != null)
                        {
                            if (player_array[i].id == NetManager.account)
                            {
                                NetManager.index = i;
                            }
                        }
                    }
                    NetManager.Inning = ebd1.inning;
                    NetManager.payment = ebd1.payment;
                    NetManager.remian_nning = NetManager.Inning;

                    NetManager.player_pos = 1;

                    NetManager.host_num = ebd1.host;
                    NetManager.setOne = true;
                    NetManager.setTwo = true;
                    NetManager.setThree = true;
                    NetManager.setHost = true;

                    NetManager.setReady = true;
                }



                break;
            case "5":

                if (str.hashtableFromJson().ContainsKey("host"))
                {
                    LeaveBackData lbd = DeserializeJsonToObject<LeaveBackData>(str);
                    player[] rest_array = lbd.player_list.ToArray();
                    List<bool> ready = new List<bool> { false, false, false };
                    int playerNum1 = 0;
                    for (int i = 0; i < rest_array.Length; i++)
                    {
                        if (rest_array[i] != null)
                        {
                            playerNum1++;
                            ready[i] = rest_array[i].ready;
                        }
                    }
                    NetManager.readylist = ready;
                    NetManager.playerNum = playerNum1;
                    NetManager.player_array = rest_array;
                    NetManager.host_num = lbd.host;
                    NetManager.setOne = true;
                    NetManager.setTwo = true;
                    NetManager.setThree = true;
                    NetManager.setHost = true;
                }

                break;
            case "6":
                if (str.hashtableFromJson().Count == 3 || str.hashtableFromJson().Count == 2)
                {
                    Debug.Log("游戏人数不足三人");
                    UReadyBackData ubd = DeserializeJsonToObject<UReadyBackData>(str);
                    List<bool> alist = ubd.ready_list;
                    Debug.Log("state" + ubd.state);
                    NetManager.readylist = ubd.ready_list;


                    NetManager.setReady = true;
                }
                //else if (str.hashtableFromJson().Count == 2)
                //{
                //    Debug.Log("游戏人数不足三人");
                //    UReadyBackData2 ubd = DeserializeJsonToObject<UReadyBackData2>(str);
                //    List<bool> alist = ubd.ready_list;
                //    NetManager.readylist = ubd.ready_list;  
                //}
                else
                {
                    TReadyBackData tbd = DeserializeJsonToObject<TReadyBackData>(str);

                    List<byte> clist = tbd.card_list;
                    List<byte> elist = tbd.external_card_list;
                    List<Card> cardlist = new List<Card>();
                    cardlist = Tools.RandomSortList(Tools.getCardList(clist));
                    NetManager.cardlist = cardlist;
                    NetManager.readylist = tbd.ready_list;
                    NetManager.turn = tbd.turn;
                    NetManager.external_card_list = elist;
                    NetManager.lastcardnum = tbd.prev_rest_cards;
                    NetManager.nextcardnum = tbd.next_rest_cards;
                }

                break;

            case "7":

                if (!str.hashtableFromJson().ContainsKey("landlord"))
                {

                    Debug.Log("还有玩家没有叫分");
                    UCallBackData ucbd = DeserializeJsonToObject<UCallBackData>(str);
                    NetManager.turn = ucbd.turn;
                    NetManager.maxBet = ucbd.bet;
                    NetManager.sign_setscore = true;

                    if (ucbd.bet != 0)
                        NetManager.multiple = ucbd.bet;

                    if (Tools.LastIndex(ucbd.turn) == NetManager.offline_pos)
                    {
                        NetManager.setThree = true;
                        NetManager.sign_off_recon = true;
                    }

                }
                else
                {
                    Debug.Log("接收到叫分的返回信息");
                    FCallBackData fcbd = DeserializeJsonToObject<FCallBackData>(str);
                    //NetManager.landlord = fcbd.landlord;
                  
                    Debug.Log("地主座位号" + NetManager.landlord);
                  
                    NetManager.multiple = fcbd.bet;
                    if (!NetManager.show_extra)
                        NetManager.external_card_list = fcbd.external_card_list;

                    if (Tools.LastIndex(fcbd.turn) == NetManager.offline_pos)
                    {
                        NetManager.setThree = true;
                        NetManager.sign_off_recon = true;
                    }
                    NetManager.sign_setscore = true;
                    NetManager.landlord = fcbd.landlord;
                    NetManager.game_begin = fcbd.game_begin;
                }
                break;
            case "8":

                PlayCardBackData pcdb = DeserializeJsonToObject<PlayCardBackData>(str);
                NetManager.game_over = pcdb.game_over;
                NetManager.playcard_state = pcdb.state;
                if (NetManager.game_over == true)
                {
                    NetManager.over = true;
                }
                NetManager.currPlayer = pcdb.prev;
                NetManager.nextPlayer = pcdb.turn;
                NetManager.current_card_list = pcdb.card_list;


                NetManager.sign_turn_index = true;

                if (pcdb.prev == NetManager.offline_pos)
                {
                    NetManager.setThree = true;
                    NetManager.sign_off_recon = true;
                }
                break;
            case "9":

                NetManager.someone_pass = true;

                PassBackData passbd = DeserializeJsonToObject<PassBackData>(str);
                NetManager.passcard_state = passbd.state;
                if (passbd.state == 0)
                {
                    Debug.Log("玩家过牌");
                    NetManager.nextPlayer = passbd.turn;
                }

                NetManager.sign_turn_index = true;

                if (passbd.prev == NetManager.offline_pos)
                {
                    NetManager.setThree = true;
                    NetManager.sign_off_recon = true;
                }
                break;
            case "10":
                ScoreBackData sbd = DeserializeJsonToObject<ScoreBackData>(str);
                NetManager.scoreList = sbd.score_list;
                break;
            case "11":
                AllScoreBackData asbd = DeserializeJsonToObject<AllScoreBackData>(str);
                NetManager.allscoreList = asbd.score_list;
                NetManager.landlordList = asbd.landlord_list;
                NetManager.change_allscore = true;
                break;
            case "50":
                OffLineBackData obd = DeserializeJsonToObject<OffLineBackData>(str);
                NetManager.offline_id = obd.id;
                NetManager.sign_offline = true;
                break;
            case "51":
                VoteBackData vbd = DeserializeJsonToObject<VoteBackData>(str);
                NetManager.sign_closeroom = vbd.shut_down;
                if (NetManager.sign_closeroom == true)
                {
                    NetManager.offline_pos = vbd.offline;
                }

                break;
            case "100":
                NetManager.sign_uncon = true;
                break;
            case "101":
                ReconBackData rbd = DeserializeJsonToObject<ReconBackData>(str);
                NetManager.roomcard = rbd.room_card;
                NetManager.setRoomCard = true;
                if (rbd.recover == 0)
                {
                  
                    NetManager.refresh_0 = true;
                }

                else  if (rbd.recover == 1)
                {
                    player[] player_array = rbd.player_list.ToArray();
                    NetManager.player_array = player_array;
                    List<bool> ready = new List<bool> { false, false, false };
                    int playerNum = 0;
                    for (int i = 0; i < player_array.Length; i++)
                    {
                        if (player_array[i] != null)
                        {
                            playerNum++;
                            ready[i] = player_array[i].ready;
                        }
                    }
                    NetManager.playerNum = playerNum;
                    NetManager.readylist = ready;
                    for (int i = 0; i < player_array.Length; i++)
                    {
                        if (player_array[i] != null)
                        {
                            if (player_array[i].id == NetManager.account)
                            {
                                NetManager.index = i;
                            }
                        }
                    }


                    NetManager.player_pos = 1;

                    NetManager.host_num = rbd.host;
                    NetManager.setOne = true;
                    NetManager.setTwo = true;
                    NetManager.setThree = true;
                    NetManager.setHost = true;

                    NetManager.setReady = true;
                    NetManager.Inning = rbd.max_inning;
                    NetManager.remian_nning = rbd.inning;

                    NetManager.RoomNumber = rbd.room_id;
                    NetManager.change_roomnumber = true;

                    NetManager.reconscorelist = rbd.score_list;
                    NetManager.refresh_1 = true;
                }
                else if (rbd.recover == 2)
                {
                    
                    player[] player_array = rbd.player_list.ToArray();
                    NetManager.player_array = player_array;
 
                    int playerNum = 0;
                    for (int i = 0; i < player_array.Length; i++)
                    {
                        if (player_array[i] != null)
                        {
                            playerNum++;
                        }
                    }
                    NetManager.playerNum = playerNum;
                    for (int i = 0; i < player_array.Length; i++)
                    {
                        if (player_array[i] != null)
                        {
                            if (player_array[i].id == NetManager.account)
                            {
                                NetManager.index = i;
                            }
                        }
                    }


                    NetManager.player_pos = 2;

                    NetManager.host_num = rbd.host;
                    NetManager.setOne = true;
                    NetManager.setTwo = true;
                    NetManager.setThree = true;
                    NetManager.setHost = true;
                   // NetManager.sign_setscore = true;
                    NetManager.turn = rbd.turn;
                    NetManager.maxBet = rbd.bet;
                    if (rbd.bet != 0)
                        NetManager.multiple = rbd.bet;

                    NetManager.Inning = rbd.max_inning;
                    NetManager.remian_nning = rbd.inning;
                    NetManager.sign_setRound = true;

                    NetManager.cardlist = Tools.getCardList(rbd.card_list);
                    NetManager.RoomNumber = rbd.room_id;
                    NetManager.change_roomnumber = true;

                    NetManager.reconscorelist = rbd.score_list;
                    NetManager.refresh_2 = true;
                }
                else if (rbd.recover == 3)
                {
                    player[] player_array = rbd.player_list.ToArray();
                    NetManager.player_array = player_array;

                    int playerNum = 0;
                    for (int i = 0; i < player_array.Length; i++)
                    {
                        if (player_array[i] != null)
                        {
                            playerNum++;
                        }
                    }
                    NetManager.playerNum = playerNum;
                    for (int i = 0; i < player_array.Length; i++)
                    {
                        if (player_array[i] != null)
                        {
                            if (player_array[i].id == NetManager.account)
                            {
                                NetManager.index = i;
                            }
                        }
                    }


                    NetManager.player_pos = 3;

                    NetManager.host_num = rbd.host;
                    NetManager.setOne = true;
                    NetManager.setTwo = true;
                    NetManager.setThree = true;
                    NetManager.setHost = true;
                    NetManager.maxBet = rbd.bet;
                    NetManager.cardlist = Tools.getCardList(rbd.card_list);

                    NetManager.recon_cardlist = Tools.getCardList(rbd.card_list);
                    NetManager.lastcardnum = rbd.prev_rest_cards;
                    NetManager.nextcardnum = rbd.next_rest_cards;
                    NetManager.nextPlayer = rbd.turn;
                    NetManager.lastPlayer = rbd.prev;
                    
                    NetManager.Inning = rbd.max_inning;
                    NetManager.remian_nning = rbd.inning;
                    NetManager.sign_setRound = true;

                    if (rbd.prev_rest_cards == 0 || rbd.next_rest_cards == 0)
                    {
                        NetManager.game_over = true;
                    }
                    else
                        NetManager.game_over = false;
                    if (NetManager.nextPlayer != NetManager.index)
                    {
                        NetManager.recon_deact = true;
                    }
                    NetManager.RoomNumber = rbd.room_id;
                    NetManager.change_roomnumber = true;
                    NetManager.reconscorelist = rbd.score_list;
                    NetManager.current_card_list = rbd.external_card_list;

                    NetManager.refresh_3 = true;
                }
                break;
            default:
                break;
        }
    }
    #endregion

    #region 发送数据


    public void Send(byte[] data)
    {
        try
        {
            socket.Send(data);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    #endregion

}
