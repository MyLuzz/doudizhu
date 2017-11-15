using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoomCard : MonoBehaviour {

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (NetManager.roomcard != 0 && NetManager.setRoomCard)
        {
            this.GetComponent<Text>().text = NetManager.roomcard.ToString();
            NetManager.setRoomCard = false;
        }
	}
}
