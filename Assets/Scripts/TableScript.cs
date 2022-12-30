using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TableScript : MonoBehaviour
{
    [SerializeField] Transform itemContainer;
    [SerializeField] Transform itemTemplate;

    private List<Transform> items = new List<Transform>();

    int numberOfPlayers = GameState.numberOfPlayers;

    bool isCreated = false;

    public void RefreshTable(ref GameObject[] players)
    {
        if(!isCreated)
        {
            CreateTable();
            isCreated = true;
        }

        float itemHeight = itemTemplate.GetComponent<RectTransform>().rect.height;

        for (int i = 0; i < numberOfPlayers; i++)
        {
            int place = players[i].GetComponent<PlayerScript>().place;
            float yPos = ((-itemHeight * place) - 30);

            items[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, yPos);

            items[i].Find("Place").GetComponent<TextMeshProUGUI>().SetText((place + 1).ToString());
            items[i].Find("PlayerName").GetComponent<TextMeshProUGUI>().SetText(players[i].GetComponent<PlayerScript>().playerName);
            items[i].Find("Turns").GetComponent<TextMeshProUGUI>().SetText(players[i].GetComponent<PlayerScript>().numberOfTurns.ToString());
            items[i].Find("Buffs").GetComponent<TextMeshProUGUI>().SetText(players[i].GetComponent<PlayerScript>().numberOfBuffs.ToString());
            items[i].Find("Fails").GetComponent<TextMeshProUGUI>().SetText(players[i].GetComponent<PlayerScript>().numberOfFails.ToString());
        }
    }

    void CreateTable()
    {
        itemTemplate.gameObject.SetActive(false);

        for (int i = 0; i < numberOfPlayers; i++)
        {
            Transform item = Instantiate(itemTemplate, itemContainer);
            item.gameObject.SetActive(true);

            items.Add(item);
        }
    }
}
