using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteButton : MonoBehaviour
{
    private bool isMuted = false;
    public GameObject muteIcon;

    public void ToggleMute()
    {
        isMuted = !isMuted;
        AudioListener.pause = isMuted;

        muteIcon.SetActive(isMuted);
        
        Debug.Log("Mute status: " + isMuted);
    }
}
