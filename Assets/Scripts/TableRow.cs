using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TableRow : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI place;
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] TextMeshProUGUI turns;
    [SerializeField] TextMeshProUGUI buffs;
    [SerializeField] TextMeshProUGUI fails;

    public void SetPlaceText(string str)
    {
        place.SetText(str);
    }

    public void SetPlayerNameText(string str)
    {
        playerName.SetText(str);
    }

    public void SetTurnsText(string str)
    {
        turns.SetText(str);
    }

    public void SetBuffsText(string str)
    {
        buffs.SetText(str);
    }

    public void SetFailsText(string str)
    {
        fails.SetText(str);
    }
}
