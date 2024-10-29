using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_EndScene : MonoBehaviour
{
    public TMP_Text totalMoneyText;
    // Start is called before the first frame update
    void Start()
    {
        int money = GameManager.instance.CurrentMoney;
        string moneyText = $"Total: {money}$";
        totalMoneyText.text = moneyText;
    }
}
