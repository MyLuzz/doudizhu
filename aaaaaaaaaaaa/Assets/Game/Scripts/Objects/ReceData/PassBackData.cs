using System;
using System.Collections;


[Serializable]
public class PassBackData
{
    public int action { get; set; }
    public int state { get; set; }
    public int prev { get; set; }
    public int turn { get; set; }
}

