using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class AllScoreBackData
{
    public int action { get; set; }
    public int state { get; set; }
    public List<int> score_list { get; set; }
    public List<int> landlord_list { get; set; }
}