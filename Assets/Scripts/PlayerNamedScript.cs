using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNamedScript : MonoBehaviour
{
    [SerializeField] Transform itemContainer;
    [SerializeField] Transform itemTemplate;

    [SerializeField] GameObject warningContainer;
    [SerializeField] TextMeshProUGUI warningText;

    int maxNameLength = 10;
    int minNameLength = 2;

    int numberOfPlayers = GameState.numberOfPlayers;

    List<PlayerNamedEntry> textMeshList = new List<PlayerNamedEntry>();

    [SerializeField] Contoller contoller;

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

    private IEnumerator ShowWarning()
    {
        warningContainer.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        warningContainer.SetActive(false);
        yield return null;
    }


    public void VerifyName()
    {

        for(int i = 0; i < numberOfPlayers; i++)
        {
            if(textMeshList[i].GetComponent<PlayerNamedEntry>().GetName().Length > maxNameLength)
            {
                warningText.SetText($"{i + 1} name is to long");
                StartCoroutine(ShowWarning());
                return;
            }

            if(textMeshList[i].GetComponent<PlayerNamedEntry>().GetName().Length < minNameLength)
            {
                warningText.SetText($"enter {i + 1} name");
                StartCoroutine(ShowWarning());
                return;
            }
        }

        contoller.SpawnPlayers(textMeshList);
        itemContainer.parent.gameObject.SetActive(false);
    }
}
