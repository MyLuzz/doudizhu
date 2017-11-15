using UnityEngine;
using System.Collections;

public class ClickTwiceToQuitApp : MonoBehaviour {

    public UnityEngine.UI.Text tipTextBox;
    public string tip = "Click again to quit app";
    public float fadingSpeed = 1;
    private bool fading;
    private float startFadingTime;
    private Color originalColor;
    private Color transparentColor;

    public byte[] leave;

    // Use this for initialization
    void Start()
    {
        originalColor = tipTextBox.color;
        transparentColor = originalColor;
        transparentColor.a = 0;
        tipTextBox.text = tip;
        tipTextBox.color = transparentColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (startFadingTime == 0)
            {
                tipTextBox.color = originalColor;
                startFadingTime = Time.time;
                fading = true;
            }
            else
            {
                ////发送离开房间的请求
                //leave = EncodeTool.LeaveRoomEncode(NetManager.account);
                //NetManager.client.Send(leave);
                Application.Quit();
            }
        }

        if (fading)
        {
            tipTextBox.color = Color.Lerp(originalColor, transparentColor, (Time.time - startFadingTime) * fadingSpeed);

            if (tipTextBox.color.a < 2.0 / 255)
            {
                tipTextBox.color = transparentColor;
                startFadingTime = 0;
                fading = false;
            }
        }
    }
}
