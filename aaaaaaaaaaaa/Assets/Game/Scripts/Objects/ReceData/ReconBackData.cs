using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class ReconBackData
{
    public int action { get; set; }
    public int state { get; set; }
    public int recover { get; set; }
    public string room_id { get; set; }
    public int room_card { get; set; }
    public int score { get; set; }
    public List<int> score_list { get; set; }
    public List<player> player_list { get; set; }
    public int host { get; set; }
    public int game_type { get; set; }
    public int bet { get; set; }
    public int turn { get; set; }
    public int prev { get; set; }
    public int inning { get; set; }
    public int max_inning { get; set; }
    public int payment { get; set; }
    public bool excard_visible { get; set; }
    public List<byte> card_list { get; set; }
    public List<byte> external_card_list { get; set; }
    public int prev_rest_cards { get; set; }
    public int next_rest_cards { get; set; }
}
