using System;
using System.Collections;


[Serializable]
public class CreateData
{
    public int action { get; set; }
    public string id { get; set; }
    public int game_type { get; set; }
    public int inning { get; set; }
    public int payment { get; set; }
    public bool excard_visible { get; set; }
}

