using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNamedEntry : MonoBehaviour
{
    int playerNumber = 0;

    [SerializeField] TextMeshProUGUI numberField;
    [SerializeField] TextMeshProUGUI nameField;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void SetName(string value)
    //{
    //    playerName = value;
    //    nameField.SetText(playerName);
    //}

    public string GetName()
    {
        return nameField.text;
    }

    public void SetNumber(int value)
    {
        playerNumber = value;
        numberField.SetText(playerNumber.ToString());
    }

    public int GetNumber()
    {
        return playerNumber;
    }
}
