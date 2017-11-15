using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using System;
using System.Collections.Generic;
using UnityEngine;
using cn.sharesdk.unity3d;

public class StartMediator : EventMediator
{
    [Inject]
    public StartView StartView { get; set; }

	public Demo demo;
    public string img_share = "http://a2.qpic.cn/psb?/V14dQG3B3xiE6U/b71zrVvzoY.n1AaLw2r4pp8N1oPSfqZqhwvgBvTqqcQ!/m/dD0BAAAAAAAAnull&bo=jACMAAAAAAADByI!&rf=photolist&t=5";

	private byte[] enterMessage;
    public byte[] uncon;
    public bool sign_pay = true;
    public bool sign_share = true;
    /// <summary>
    /// 注册函数
    /// </summary>
    public override void OnRegister()
    {
        StartView.Init();
        demo = GameObject.Find ("Main Camera").GetComponent<Demo> ();
        //StartView.QQCancelLogin.onClick.AddListener (QQCancelLogin);
        //StartView.WeChatCancelLogin.onClick.AddListener (WeChatCancelLogin);
		StartView.imgEnter.gameObject.SetActive(false);
		StartView.btn_Enter.onClick.AddListener (showEnterClick);

		StartView.btn_cancel.onClick.AddListener (cancelClick);
		StartView.btn_0.onClick.AddListener (btn0Click);
		StartView.btn_1.onClick.AddListener (btn1Click);
		StartView.btn_2.onClick.AddListener (btn2Click);
		StartView.btn_3.onClick.AddListener (btn3Click);
		StartView.btn_4.onClick.AddListener (btn4Click);
		StartView.btn_5.onClick.AddListener (btn5Click);
		StartView.btn_6.onClick.AddListener (btn6Click);
		StartView.btn_7.onClick.AddListener (btn7Click);
		StartView.btn_8.onClick.AddListener (btn8Click);
		StartView.btn_9.onClick.AddListener (btn9Click);
		StartView.btn_confirm.onClick.AddListener (enterClick);
        StartView.btn_closeEnter.onClick.AddListener(closeEnter);

        StartView.exit_yes.onClick.AddListener(Exit_Yes);
        StartView.exit_no.onClick.AddListener(Exit_No);
		StartView.Exit.onClick.AddListener (ClickToExit);

        StartView.btn_Pay.onClick.AddListener(OpenPayClick);
        StartView.btn_Share.onClick.AddListener(OpenShareClick);
        StartView.share_wechat.onClick.AddListener(Wechat_Share);
        StartView.share_moments.onClick.AddListener(WeChatMoments_Share);
        StartView.share_qq.onClick.AddListener(QQ_Share);
        StartView.share_qzone.onClick.AddListener(QZone_Share);

    }

    /// <summary>
    /// 移除函数
    /// </summary>
    public override void OnRemove()
    {
        StartView.ViewDestroy();
        //StartView.QQCancelLogin.onClick.RemoveListener (QQCancelLogin);
        //StartView.WeChatCancelLogin.onClick.RemoveListener (WeChatCancelLogin);

		StartView.btn_Enter.onClick.RemoveListener (showEnterClick);

		StartView.btn_cancel.onClick.RemoveListener (cancelClick);
		StartView.btn_0.onClick.RemoveListener (btn0Click);
		StartView.btn_1.onClick.RemoveListener (btn1Click);
		StartView.btn_2.onClick.RemoveListener (btn2Click);
		StartView.btn_3.onClick.RemoveListener (btn3Click);
		StartView.btn_4.onClick.RemoveListener (btn4Click);
		StartView.btn_5.onClick.RemoveListener (btn5Click);
		StartView.btn_6.onClick.RemoveListener (btn6Click);
		StartView.btn_7.onClick.RemoveListener (btn7Click);
		StartView.btn_8.onClick.RemoveListener (btn8Click);
		StartView.btn_9.onClick.RemoveListener (btn9Click);
		StartView.btn_confirm.onClick.RemoveListener (enterClick);
        StartView.btn_closeEnter.onClick.RemoveListener(closeEnter);


        StartView.exit_yes.onClick.RemoveListener(Exit_Yes);
        StartView.exit_no.onClick.RemoveListener(Exit_No);
		StartView.Exit.onClick.RemoveListener (ClickToExit);
        //StartView.dispatcher.RemoveListener(ViewEvent.CHANGE_MULTIPLE, onViewClick);
        StartView.btn_Pay.onClick.RemoveListener(OpenPayClick);
        StartView.btn_Share.onClick.RemoveListener(OpenShareClick);
        StartView.share_wechat.onClick.RemoveListener(Wechat_Share);
        StartView.share_moments.onClick.RemoveListener(WeChatMoments_Share);
        StartView.share_qq.onClick.RemoveListener(QQ_Share);
        StartView.share_qzone.onClick.RemoveListener(QZone_Share);
    }
    #region 分享
    public void OpenShareClick()
    {
        StartView.soundManager.mc_btn_click.Play();
        StartView.btn_Share.gameObject.GetComponent<Transform>().GetChild(0).gameObject.SetActive(sign_share);
        sign_share = !sign_share;
    }

    public void Wechat_Share()
    {
        ShareContent content = new ShareContent();
        content.SetShareType(ContentType.Image);
        content.SetTitle("姗姗斗地主");
        content.SetImageUrl(img_share);
        demo.ssdk.ShowShareContentEditor(PlatformType.WeChat, content);
    }

    public void WeChatMoments_Share()
    {
        ShareContent content = new ShareContent();
        content.SetShareType(ContentType.Image);
        content.SetTitle("姗姗斗地主");
        content.SetImageUrl(img_share);
        demo.ssdk.ShowShareContentEditor(PlatformType.WeChatMoments, content);
    }

    public void QQ_Share()
    {
        ShareContent content = new ShareContent();
        content.SetShareType(ContentType.Image);
        content.SetTitle("姗姗斗地主");
        content.SetImageUrl(img_share);
        demo.ssdk.ShowShareContentEditor(PlatformType.QQ, content);
    }

    public void QZone_Share()
    {
        ShareContent content = new ShareContent();
        content.SetShareType(ContentType.Image);
        content.SetTitle("姗姗斗地主");
        content.SetImageUrl(img_share);
        demo.ssdk.ShowShareContentEditor(PlatformType.QZone, content);
    }
    #endregion
    #region 支付
    public void OpenPayClick()
    {
        StartView.soundManager.mc_btn_click.Play();
        StartView.btn_Pay.gameObject.GetComponent<Transform>().GetChild(0).gameObject.SetActive(sign_pay);
        sign_pay = !sign_pay;
    }
    #endregion

    #region 房号输入
    private void enterClick()
	{
        enterMessage = EncodeTool.EnterRoomEncode(NetManager.account, StartView.InpNumber.text);
        NetManager.client.Send(enterMessage);
        StartView.waitLoad();    
	}
	private void cancelClick()
	{
        StartView.soundManager.mc_number.Play();
        if (StartView.InpNumber.text.Length > 0)
        {
            StartView.InpNumber.text = StartView.InpNumber.text.Substring (0, StartView.InpNumber.text.Length - 1);
        }
		
	}

	private void btn0Click()
	{
        StartView.soundManager.mc_number.Play();
		StartView.InpNumber.text += "0";
	}
	private void btn1Click()
	{
        StartView.soundManager.mc_number.Play();
		StartView.InpNumber.text += "1";
	}
	private void btn2Click()
	{
        StartView.soundManager.mc_number.Play();
		StartView.InpNumber.text += "2";
	}
	private void btn3Click()
	{
        StartView.soundManager.mc_number.Play();
		StartView.InpNumber.text += "3";
	}
	private void btn4Click()
	{
        StartView.soundManager.mc_number.Play();
		StartView.InpNumber.text += "4";
	}
	private void btn5Click()
	{
        StartView.soundManager.mc_number.Play();
		StartView.InpNumber.text += "5";
	}
	private void btn6Click()
	{
        StartView.soundManager.mc_number.Play();
		StartView.InpNumber.text += "6";
	}
	private void btn7Click()
	{
        StartView.soundManager.mc_number.Play();
		StartView.InpNumber.text += "7";
	}
	private void btn8Click()
	{
        StartView.soundManager.mc_number.Play();
		StartView.InpNumber.text += "8";
	}
	private void btn9Click()
	{
        StartView.soundManager.mc_number.Play();
		StartView.InpNumber.text += "9";
	}
	#endregion

	private void showEnterClick()
	{
        StartView.soundManager.mc_btn_click.Play();
		StartView.imgEnter.gameObject.SetActive (true);
	}
    /// <summary>
    /// View被点击时候的调用
    /// </summary>
    /// <param name="evt"></param>
    private void onViewClick(IEvent evt)
    {
    //    int multiple = (int)evt.data;
    //    //发送出去
    //    dispatcher.Dispatch(CommandEvent.ChangeMultiple, multiple);
    }

	public void ClickToExit()
	{
        StartView.soundManager.mc_btn_click.Play();
        StartView.exit_panel.gameObject.SetActive(true);
	}
    public void Exit_Yes()
    {
        uncon = EncodeTool.ReconEncode(100, NetManager.account, 0, NetManager.token);
        NetManager.client.Send(uncon);
        Application.Quit();
    }
    public void Exit_No()
    {
        StartView.exit_panel.gameObject.SetActive(false);
    }
    public void closeEnter()
    {
        StartView.soundManager.mc_btn_click.Play();
        GameObject.Find("imgEnter").SetActive(false);
    }
}
