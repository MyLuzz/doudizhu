using System;
using System.Collections;
using System.Collections.Generic;


[Serializable]
public class TReadyBackData
{
	public int action { get; set; }
    public int state { get; set; }
	public List<bool> ready_list { get; set; }
    public int turn { get; set; }
    public List<byte> card_list { get; set; }
	public int prev_rest_cards{ get; set;}
	public int next_rest_cards{ get; set;}
    public List<byte> external_card_list { get; set; }
}

