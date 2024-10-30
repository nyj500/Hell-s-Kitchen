using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorialPopup1;
    public GameObject tutorialPopup2;

    public void OnClickedNextButton()
    {
        if (tutorialPopup2.activeSelf)
        {
            SceneManager.LoadScene("Scene1");
        }
        
        if (!tutorialPopup1.activeSelf)
        {
            tutorialPopup1.SetActive(true);
        }
        else if (!tutorialPopup2.activeSelf)
        {
            tutorialPopup1.SetActive(false);
            tutorialPopup2.SetActive(true);
        }
    }
}
