using System;
using System.Collections;
using System.Collections.Generic;


[Serializable]
public class PlayCardBackData
{
    public int action { get; set; }
    public int state { get; set; }
    public int prev { get; set; }
    public int turn { get; set; }
    public List<byte> card_list { get; set; }
    public bool game_over { get; set; }
    public int inning { get; set; }
}

