using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
public class PauseandFocusTest : MonoBehaviour
{

    public Text testText;
    private int testInt = 0;
    public static List<CF_CP> list = new List<CF_CP>();
    public byte[] recon;
    public byte[] uncon;
    void OnApplicationPause(bool pauseStatus)
    {
        testText.text += DateTime.Now.ToString() + "Pause:" + testInt.ToString() + "pauseStatus" + pauseStatus.ToString() + "\n";
        testInt++;
   
        CF_CP tempPause = new CF_CP("Pause", pauseStatus);
        list.Add(tempPause);
    }
    void OnApplicationFocus(bool hasFocus)
    {
        testText.text += DateTime.Now.ToString() + "Focus:" + testInt.ToString() + "hasFocus" + hasFocus.ToString() + "\n";
        testInt++;

        CF_CP tempFocus = new CF_CP("Focus", hasFocus);
        list.Add(tempFocus);
    }
    void Update()
    {
        if (NetManager.StartListen)
        {
            if (list.Count == 2)
            {
                if (list[0].name == "Focus" && list[0].sign == false && list[1].name == "Focus" && list[1].sign == true)
                {
                        print("重新连接");
                        NetManager.client.socket.Close();
                        NetManager.client = new ClientPeer("61.164.248.190", 4396);
                        NetManager.client.Connect();
                        recon = EncodeTool.ReconEncode(101, NetManager.account,0,NetManager.token);
                        NetManager.client.Send(recon);
                        list.Clear();
                }
            }
            if (list.Count == 4)
            {
                Debug.Log("开始检测");
                if (list[0].name == "Focus" && list[0].sign == false && list[1].name == "Pause" && list[1].sign == true)
                {
                    if (list[3].name == "Pause" && list[3].sign == false && list[2].name == "Focus" && list[2].sign == true)
                    {
                        print("重新连接");
                        NetManager.client.socket.Close();
                        NetManager.client = new ClientPeer("61.164.248.190", 4396);
                        NetManager.client.Connect();
                        recon = EncodeTool.ReconEncode(101, NetManager.account,0,NetManager.token);
                        NetManager.client.Send(recon);
                    }
                }
                list.Clear();
            }
        }
    }
}