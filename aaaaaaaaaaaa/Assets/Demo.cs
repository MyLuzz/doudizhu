using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using cn.sharesdk.unity3d;
using UnityEngine.UI;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using LitJson; 
using Net;  
using Newtonsoft.Json;
using System.Net;  
using System.Net.Sockets; 

public class Demo : MonoBehaviour {

	public GUISkin demoSkin;
	public ShareSDK ssdk;
	public Button Login;
	public String Nickname;
    public byte[] message;
    public Image main_head;
    public Text main_name;
    public GameObject login_tip;

    public SoundManager soundManager;
	public String GetName()
	{
		return Nickname;
	}
	// Use this for initialization
	void Start ()
	{	
		ssdk = gameObject.GetComponent<ShareSDK>();
		ssdk.authHandler = OnAuthResultHandler;
		ssdk.shareHandler = OnShareResultHandler;
		ssdk.showUserHandler = OnGetUserInfoResultHandler;
		ssdk.getFriendsHandler = OnGetFriendsResultHandler;
		ssdk.followFriendHandler = OnFollowFriendResultHandler;
	}


    public void Capture()
    {
        ShareContent content = new ShareContent();
       // content.SetText("斗地主");
        content.SetTitle("战绩分享");
        content.SetTitleUrl("http://www.baidu.com");
        //content.SetSite("分享site");
       // content.SetSiteUrl("http://http://www.site.com");
        //content.SetUrl("http://http://www.site.com");
        //content.SetComment("分享comment");
        content.SetShareType(ContentType.Image);

        //截屏  
        Application.CaptureScreenshot("Shot4Share.png");
        //设置图片路径  
        content.SetImagePath(Application.persistentDataPath + "/Shot4Share.png");

        ssdk.ShowPlatformList(null, content, 100, 100);
    }  
  
	public string GetMd5(String str)
	{
		string str1 = "";
		byte[] data = Encoding.GetEncoding ("utf-8").GetBytes (str);
		MD5 md5 = new MD5CryptoServiceProvider ();
		byte[] bytes = md5.ComputeHash (data);
		for (int i = 0; i < bytes.Length; i++) {
			str1 += bytes [i].ToString ("x2");
		}
		return str1;
	}

	public void QQLoginClick()
	{
        NetManager.client = new ClientPeer("61.164.248.190", 4396);
        NetManager.client.Connect();
	    ssdk.GetUserInfo(PlatformType.QQ);
	}

	public void WeChatLoginClick()
	{
        login_tip.SetActive(true);
        login_tip.GetComponent<login_tip>().tip_login();
        NetManager.client = new ClientPeer("61.164.248.190", 4396);
        NetManager.client.Connect();
        ssdk.GetUserInfo(PlatformType.WeChat);
  
	}

	public void QQUnLoginClick()
	{
		ssdk.CancelAuthorize (PlatformType.QQ);
	}

	public void WeChatUnLoginClick()
	{
		ssdk.CancelAuthorize (PlatformType.WeChat);
	}
		
	
	void OnAuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{
			print ("authorize success !" + "Platform :" + type);
            Hashtable authInfo = ssdk.GetAuthInfo(type);
            print(MiniJSON.jsonEncode(result));
           
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}
	public string GetSex(string a)
	{
		if (a == "1") {
			return "男";
		}
		else 
			return "女";
	}
	
	void OnGetUserInfoResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
     
		if (state == ResponseState.Success)
		{
            PauseandFocusTest.list.Clear();

            Hashtable authInfo = ssdk.GetAuthInfo(type);
			print ("get user info result :");
			print (MiniJSON.jsonEncode(result));
            print("AuthInfo:" + MiniJSON.jsonEncode(authInfo));
            print("Get userInfo success !Platform :" + type);

            NetManager.playerName = result["nickname"].ToString();
            NetManager.token = authInfo["token"].ToString();
            int plat;
            if (type == PlatformType.QQ)
            {
                NetManager.account = authInfo["userID"].ToString();
                NetManager.accName = result["nickname"].ToString();
                NetManager.avator = result["headimgurl"].ToString();
                plat = 1;
                message = EncodeTool.UserEncode(authInfo["userID"].ToString(), result["nickname"].ToString(), result["gender"].ToString(), result["figureurl_qq_2"].ToString(), plat, authInfo["token"].ToString());
                NetManager.client.Send(message);
            }
            else if (type == PlatformType.WeChat)
            {
                NetManager.account = result["openid"].ToString();
                plat = 0;
             
                NetManager.accName = result["nickname"].ToString();
                NetManager.avator = result["headimgurl"].ToString();
                NetManager.sex = GetSex(result["sex"].ToString());
                NetManager.avator = result["headimgurl"].ToString();
                AsyncImageDownload.Instance.Init();
                AsyncImageDownload.Instance.SetAsyncImage(NetManager.avator, GameObject.Find("Player_Head").GetComponent<Image>());
                message = EncodeTool.UserEncode(result["openid"].ToString(), result["nickname"].ToString(), GetSex(result["sex"].ToString()), result["headimgurl"].ToString(), plat, authInfo["token"].ToString());
                NetManager.client.Send(message);
               
            }
            
            NetManager.StartListen = true;
           
			Nickname = result ["nickname"].ToString();//获得登录昵称
			print ("获得头像成功！");
//			Figure = GetMd5(result["figureurl_qq_2"].ToString());
//			print (result["figureurl_qq_2"].ToString());
//			print (Figure);
//			print ("获得头像成功！");
            GameObject.Find("Player_Name").GetComponent<Text>().text = NetManager.accName;
            AsyncImageDownload.Instance.Init();
            AsyncImageDownload.Instance.SetAsyncImage(NetManager.avator, main_head);
            main_name.text = NetManager.accName;
         
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}
	
    void OnShareResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{
			print ("share successfully - share result :");
			print (MiniJSON.jsonEncode(result));
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}

	void OnGetFriendsResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{			
			print ("get friend list result :");
			print (MiniJSON.jsonEncode(result));
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}

	void OnFollowFriendResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{
			print ("Follow friend successfully !");
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			print ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			print ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			print ("cancel !");
		}
	}
}
