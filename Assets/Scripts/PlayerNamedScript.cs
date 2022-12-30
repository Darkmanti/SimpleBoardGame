using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerNamedScript : MonoBehaviour
{
    [SerializeField] Transform itemContainer;
    [SerializeField] Transform itemTemplate;

    int numberOfPlayers = GameState.numberOfPlayers;

    List<PlayerNamedEntry> textMeshList = new List<PlayerNamedEntry>();

    [SerializeField] Contoller contoller;

    // Start is called before the first frame update
    void Start()
    {
        CreateTable();
    }

    void CreateTable()
    {
        itemTemplate.gameObject.SetActive(false);

        float itemHeight = itemTemplate.GetComponent<RectTransform>().rect.height;

        for (int i = 0; i < numberOfPlayers; i++)
        {
            Transform item = Instantiate(itemTemplate, itemContainer);
            item.gameObject.SetActive(true);

            float yPos = ((-itemHeight * i) - 60);

            item.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, yPos);
            item.GetComponent<PlayerNamedEntry>().SetNumber(i + 1);

            textMeshList.Add(item.GetComponent<PlayerNamedEntry>());
        }
    }


    public void VerifyName()
    {
        // TODO: check name

        if(true)
        {
            contoller.SpawnPlayers(textMeshList);
            itemContainer.parent.gameObject.SetActive(false);
            //PlayersNamedObj.SetActive(false);
        }
        else
        {
            // notify
        }
    }
}
