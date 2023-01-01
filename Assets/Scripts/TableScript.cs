using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TableScript : MonoBehaviour
{
    [SerializeField] Transform itemContainer;
    [SerializeField] Transform itemTemplate;

    private List<TableRow> items = new List<TableRow>();

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
            PlayerScript player = players[i].GetComponent<PlayerScript>();

            int place = player.place;
            float yPos = ((-itemHeight * place) - 30);

            items[i].gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, yPos);

            items[i].SetPlaceText((place + 1).ToString());
            items[i].SetPlayerNameText(player.playerName);
            items[i].SetTurnsText(player.numberOfTurns.ToString());
            items[i].SetBuffsText(player.numberOfBuffs.ToString());
            items[i].SetFailsText(player.numberOfFails.ToString());
        }
    }

    void CreateTable()
    {
        itemTemplate.gameObject.SetActive(false);

        for (int i = 0; i < numberOfPlayers; i++)
        {
            Transform item = Instantiate(itemTemplate, itemContainer);
            item.gameObject.SetActive(true);

            items.Add(item.GetComponent<TableRow>());
        }
    }
}
