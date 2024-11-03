using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public Button button3;
    public Sprite originSprite;
    public Sprite newSprite;

    void Start()
    {
        GameManager.instance.currentPlayer = GameManager.PlayerType.player1;
    }
   
    public void SetPlayerToPlayer1()
    {
        GameManager.instance.currentPlayer = GameManager.PlayerType.player1;
        button1.image.sprite = newSprite;
        button2.image.sprite = originSprite;
        button3.image.sprite = originSprite;
    }

    public void SetPlayerToPlayer2()
    {
        GameManager.instance.currentPlayer = GameManager.PlayerType.player2;
        button1.image.sprite = originSprite;
        button2.image.sprite = newSprite;
        button3.image.sprite = originSprite;
    }

    public void SetPlayerToPlayer3()
    {
        GameManager.instance.currentPlayer = GameManager.PlayerType.player3;
        button1.image.sprite = originSprite;
        button2.image.sprite = originSprite;
        button3.image.sprite = newSprite;
    }
}
