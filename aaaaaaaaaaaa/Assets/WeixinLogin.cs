using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeixinLogin : MonoBehaviour {

	// Use this for initialization
    private byte[] message;
    public Image main_head;
    public Text main_name;
    public Button btn_login;
    public GameObject login_tip;

    public GameObject doudizhu;

    public SoundManager soundManager;
	void Start () {
        btn_login.onClick.AddListener(gameStart);
	}
	
	// Update is called once per frame
	void Update () {
        //GameObject.Find("LoginPanel").transform.SetSiblingIndex(8);
	}

    public void gameStart()
    {
        //Tools.CreateUIPanel(PanelType.StartPanel);
        NetManager.client = new ClientPeer("61.164.248.190", 4396);
        NetManager.client.Connect();
        if (NetManager.client.socket.Connected)
        {
            Debug.Log("已经连接");
            NetManager.StartListen = true;
            PauseandFocusTest.list.Clear();
        }
        Debug.Log("游戏开始");
        string head = "";
        if (NetManager.pos == 0)
        {
            string str = Tools.randString();
            NetManager.account = str;
            NetManager.accName = "bbb";
            GameObject.Find("Player_Name").GetComponent<Text>().text = NetManager.accName;
            head = "http://wx.qlogo.cn/mmopen/mz3Mk129TGC0GicfYaEAo3bFiaiacFkYhAQeLkwA60am5PbE7KDTibD9ahN7cX13ALQkIdswFekGFAEFQQopq5BVwH6QiaRW0sHuT/0";
            AsyncImageDownload.Instance.Init();
            AsyncImageDownload.Instance.SetAsyncImage(head, GameObject.Find("Player_Head").GetComponent<Image>());
            message = EncodeTool.TestEncode(str, "bbbb", "女", head);
            NetManager.client.Send(message);
            NetManager.pos++;
            NetManager.avator = head;
            login_tip.gameObject.SetActive(true);
            login_tip.GetComponent<login_tip>().tip_login();
        }

        AsyncImageDownload.Instance.Init();
        AsyncImageDownload.Instance.SetAsyncImage(NetManager.avator, main_head);
        main_name.text = NetManager.accName;

       
    }


}
