using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {
    public Slider volume;
    public Slider volume1;
    public AudioSource mc_welcome;
    public AudioSource mc_normal;
    public AudioSource mc_exciting;
    public AudioSource mc_lose;
    public AudioSource mc_win;

    public AudioSource mc_btn_click;
    public AudioSource mc_game_click;
    public AudioSource mc_number;

    public AudioSource mc_buyao1;

    public AudioSource mc_baojing2;
    public AudioSource mc_baojing1;
	void Update () {
        //if (AudioListener.volume != 0 && sound.value != 0)
        //{
        //    AudioListener.volume = sound.value;
        //}
        if (NetManager.player_pos == 0)
        {
            AudioListener.volume = volume.value;
            volume1.value = volume.value;
        }
        else if (NetManager.player_pos == 1)
        {
            AudioListener.volume = volume1.value;
            volume.value = volume1.value;
        }
       
	}
}
