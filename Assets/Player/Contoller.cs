using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static WayPointData;

public class Contoller : MonoBehaviour
{
    [SerializeField] GameObject playerTemplate;

    [SerializeField] List<Material> playersMaterials;

    GameObject[] players;

    float speed = 4.0f;

    int currentPlayer;
    int numberOfPlayers;

    bool gameEnd = false;

    int steps;

    [SerializeField] Route route;

    [SerializeField] GameObject PlayersNamedObj;
    [SerializeField] GameObject canvasGameInfo;

    [SerializeField] GameObject hintInfo;
    [SerializeField] GameObject tableStat;

    [SerializeField] GameObject screenHint;

    [SerializeField] TextMeshProUGUI diceRolledText;
    [SerializeField] TextMeshProUGUI hintText;
    [SerializeField] TextMeshProUGUI playerTurnText;

    [SerializeField] GameObject landScape;

    [SerializeField] AudioSource audioSource;
    private SFXScript audioSFX;

    public bool isPlayersNamed = false;

    [SerializeField] GameObject EscMenuObj;
    bool escMenu = false;

    private struct KeyValuePlace
    {
        public int key;
        public int value;
    }


    // Start is called before the first frame update
    void Start()
    {
        numberOfPlayers = GameState.numberOfPlayers;
        players = new GameObject[numberOfPlayers];
        // TODO: disable unusable name of players
        PlayersNamedObj.SetActive(true);

        audioSFX = audioSource.GetComponent<SFXScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayersNamed)
        {
            // getting rolled dice
            if (Input.GetKeyDown(KeyCode.Space) && !(players[currentPlayer].GetComponent<PlayerScript>().isMooving) && (!gameEnd))
            {
                steps = ThrowDice();
                StartCoroutine(MakeSteps(steps));
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SwitchEscMenu();
            }

            if(Input.GetKeyDown(KeyCode.Tab))
            {
                ShowStat();
            }

            if(Input.GetKeyDown(KeyCode.F1))
            {
                ShowHideTint();
            }
        }
        else
        {
            
        }

    }

    public void SpawnPlayers(List<PlayerNamedEntry> playersInputField)
    {
        Vector3 spawnPointPos = route.wayPointsSorted[0].GetComponent<Transform>().position;

        for (int i = 0; i < numberOfPlayers; i++)
        {
            players[i] = Instantiate(playerTemplate);
            players[i].GetComponent<MeshRenderer>().material = playersMaterials[i];

            players[i].GetComponent<PlayerScript>().playerName = playersInputField[i].GetName();
            players[i].GetComponent<PlayerScript>().pointPosition = 0;
            players[i].GetComponent<PlayerScript>().place = 1;
            players[i].GetComponent<PlayerScript>().playerNumber = i;

            players[i].GetComponent<Transform>().position = spawnPointPos;

            players[i].GetComponent<Transform>().SetParent(landScape.transform);
        }

        SelectPositionsOnWayPoint(0);

        RefreshStat();

        isPlayersNamed = true;
        currentPlayer = 0;
        playerTurnText.SetText($"Player {players[currentPlayer].GetComponent<PlayerScript>().playerName} turn");
        players[currentPlayer].GetComponent<PlayerScript>().particle.gameObject.SetActive(true);
    }

    void SelectPositionsOnWayPoint(int wayPointNumber)
    {
        int playersOnWayPoint = 0;

        for(int i = 0; i < numberOfPlayers; i++)
        {
            if (players[i].GetComponent<PlayerScript>().pointPosition == wayPointNumber)
            {
                playersOnWayPoint++;
            }
        }

        if(playersOnWayPoint < 2)
        {
            return;
        }

        float angle = 360 / playersOnWayPoint;
        float anglePos = angle / 2;
        // TODO: calculate radius dynamically
        float radius = 0.35f;

        Vector3 center = route.wayPointsSorted[wayPointNumber].GetComponent<Transform>().position;

        for (int i = 0; i < numberOfPlayers; i++)
        {
            if (players[i].GetComponent<PlayerScript>().pointPosition == wayPointNumber)
            {
                Vector3 pos;
                pos.x = center.x + radius * Mathf.Sin(anglePos * Mathf.Deg2Rad);
                pos.z = center.z + radius * Mathf.Cos(anglePos * Mathf.Deg2Rad);
                // TODO: calculate high dynamically
                pos.y = center.y + 0.3f;

                //players[i].GetComponent<Transform>().position = pos;
                StartCoroutine(MoveToTarget(players[i].transform, pos));

                anglePos += angle;
            }
        }
        
    }

    private IEnumerator MoveToTarget(Transform obj, Vector3 target)
    {
        while(obj.position != target)
        {
            obj.position = Vector3.MoveTowards(obj.position, target, Time.deltaTime * speed);
            yield return null;
        }
    }

    IEnumerator MakeSteps(int steps)
    {
        players[currentPlayer].GetComponent<PlayerScript>().isMooving = true;
        players[currentPlayer].GetComponent<PlayerScript>().particle.gameObject.SetActive(false);

        while (steps != 0)
        {
            int pointPos = 0;

            if (steps > 0)
            {
                pointPos = ++players[currentPlayer].GetComponent<PlayerScript>().pointPosition;
            }
            else
            {
                pointPos = --players[currentPlayer].GetComponent<PlayerScript>().pointPosition;
            }

            Vector3 target = route.wayPointsSorted[pointPos].transform.position;

            // TODO: calculate high dynamically
            target.y += 0.3f;

            yield return StartCoroutine(MoveToTarget(players[currentPlayer].transform, target));
            audioSFX.PlayOneOfStepSound();

            //steps--;
            steps = steps > 0 ? (steps - 1) : (steps + 1);

            yield return new WaitForSeconds(0.12f);

            if(CheckForEnd(pointPos))
            {
                players[currentPlayer].GetComponent<PlayerScript>().isEnd = true;
                steps = 0;
            }
        }

        players[currentPlayer].GetComponent<PlayerScript>().isMooving = false;

        SelectPositionsOnWayPoint(players[currentPlayer].GetComponent<PlayerScript>().pointPosition);

        TurnResult();
    }

    public int ThrowDice()
    {
        int result = 0;
        result = Random.Range(1, 7);
        steps = result;
        diceRolledText.SetText($"Dice Rolled: {result}");
        return result;
    }

    // check info about standing way point and recording stat
    public void TurnResult()
    {
        PlayerScript player = players[currentPlayer].GetComponent<PlayerScript>();
        WayPointData pointData = route.wayPointsSorted[player.pointPosition].GetComponent<WayPointData>();

        player.numberOfTurns++;

        switch (pointData.bonusType)
        {
            case WayPointBonus.Default:
                {
                    NextTurn(false);
                    break;
                }

            case WayPointBonus.Fail:
                {
                    player.numberOfFails++;
                    StartCoroutine(MakeSteps(-pointData.bonusValue));
                    break;
                }

            case WayPointBonus.Buff:
                {
                    player.numberOfBuffs++;
                    StartCoroutine(MakeSteps(pointData.bonusValue));
                    break;
                }

            case WayPointBonus.OneTurn:
                {
                    NextTurn(true);
                    break;
                }
        }


        if(gameEnd)
        {
            ShowStat();
        }
    }

    // prepare for the next turn
    public void NextTurn(bool repeat)
    {
        RefreshStat();

        if(repeat)
        {
            // we simply dont change current player
        }
        else
        {
            int iter = 0;

            do
            {
                currentPlayer++;

                if ((currentPlayer + 1) > numberOfPlayers)
                {
                    currentPlayer = 0;
                }

                if (players[currentPlayer].GetComponent<PlayerScript>().isEnd)
                {
                    iter++;
                    if (iter >= numberOfPlayers)
                    {
                        gameEnd = true;
                        break;
                    }
                }
                else
                {
                    break;
                }

            } while (true);
        }
        

        playerTurnText.SetText($"Player {players[currentPlayer].GetComponent<PlayerScript>().playerName} turn");
        diceRolledText.SetText("Dice Rolled: ...");
        players[currentPlayer].GetComponent<PlayerScript>().particle.gameObject.SetActive(true);
    }


    public void RefreshStat()
    {
        List<KeyValuePlace> places = new List<KeyValuePlace>();

        for (int i = 0; i < numberOfPlayers; i++)
        {
            KeyValuePlace item;
            item.key = players[i].GetComponent<PlayerScript>().playerNumber;
            item.value = players[i].GetComponent<PlayerScript>().pointPosition;
            places.Add(item);
        }

        places = places.OrderByDescending(x => x.value).ToList();

        for(int i = 0; i < numberOfPlayers; i++)
        {
            players[i].GetComponent<PlayerScript>().place = places[i].key;
        }
    }

    public void SwitchEscMenu()
    {
        escMenu = !escMenu;
        EscMenuObj.SetActive(escMenu);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowStat()
    {
        hintInfo.SetActive(!hintInfo.activeSelf);
        tableStat.GetComponent<TableScript>().RefreshTable(ref players);
        tableStat.SetActive(!tableStat.activeSelf);
    }

    bool CheckForEnd(int pointPos)
    {
        if (route.wayPointsSorted[pointPos].GetComponent<WayPointData>().pointType == WayPointData.WayPointEnumerator.End)
        {
            return true;
        }

        return false;
    }

    void ShowHideTint()
    {
        screenHint.SetActive(!screenHint.activeSelf);
    }
}
