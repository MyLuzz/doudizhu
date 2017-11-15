using System;
using System.Collections;


[Serializable]
public class player
{
    public string id { get; set; }
    public string avator { get; set; }
    public string nickname { get; set; }
    public string sex { get; set; }
    public int score { get; set; }
    public bool ready { get; set; }

    public player()
    {
        id = "";
        avator = "";
        nickname = "";
        sex = "";
        score = 0;
        ready = false;
    }
}

