using System;
using System.Collections;
using System.Collections.Generic;


[Serializable]
public class UReadyBackData
{
	public int action { get; set; }
    public int state { get; set; }
	public List<bool> ready_list{ get; set;}

}

