using System;
using System.Collections;
using System.Collections.Generic;


[Serializable]
public class EnterBackData
{
    public int action { get; set; }
    public int state { get; set; }
    public int host { get; set; }//房主座位号
    public List<player> player_list { get; set; }
    public int inning { get; set; }
    public int payment { get; set; }
    public bool excard_visible { get; set; }

}

