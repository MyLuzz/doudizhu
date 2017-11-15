using System;
using System.Collections;


[Serializable]
public class UCallBackData
{
    public int action { get; set; }
    public int state { get; set; }
    public int bet { get; set; }
    public int turn { get; set; }
    public bool game_begin { get; set; }
}

