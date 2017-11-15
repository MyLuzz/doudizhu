using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class login_tip : MonoBehaviour {

    private int num = 1;
    private string point;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}
    public void tip_login()
    {
        StartCoroutine(wait_login_tip());
    }
    IEnumerator wait_login_tip()
    {
        while (num <= 3)
        {
            for (int i = 0; i < num; i++)
            {
                point += ".";
            }
            yield return new WaitForSeconds(0.5f);
            this.GetComponent<Text>().text = "登录中" + point;
            num++;
            if (num > 3)
            {
                num = 0;
                point = null;
                this.GetComponent<Text>().text = "登录中" + point;
            }
                
        }
    }
}
