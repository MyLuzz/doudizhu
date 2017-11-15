using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomNumber : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (NetManager.change_roomnumber)
        {
            this.GetComponent<Text> ().text = NetManager.RoomNumber;
            NetManager.change_roomnumber = false;
        }
		
	}
}
