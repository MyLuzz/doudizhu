using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class ScoreBackData
{
    public int action { get; set; }
    public int state{ get; set; }
    public List<int> score_list { get; set; }
}
