using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class FCallBackData
{
    public int action { get; set; }
    public int state { get; set; }
    public int bet { get; set; }
    public int turn { get; set; }
    public int landlord { get; set; }
    public bool game_begin { get; set; }
    public List<byte> external_card_list { get; set; }
}

