using System;
using System.Collections;
using System.Collections.Generic;


[Serializable]
public class VoteBackData
{
    public int action { get; set; }
    public int state { get; set; }
    public string id { get; set; }
    public int offline { get; set; }
    public bool agree { get; set; }
    public bool shut_down { get; set; }

}

