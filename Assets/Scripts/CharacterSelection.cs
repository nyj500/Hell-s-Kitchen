using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    void Start()
    {
        GameManager.instance.currentPlayer = GameManager.PlayerType.player1;
    }
   
    public void SetPlayerToPlayer1()
    {
        GameManager.instance.currentPlayer = GameManager.PlayerType.player1;
    }

    public void SetPlayerToPlayer2()
    {
        GameManager.instance.currentPlayer = GameManager.PlayerType.player2;
    }

    public void SetPlayerToPlayer3()
    {
        GameManager.instance.currentPlayer = GameManager.PlayerType.player3;
    }
}
