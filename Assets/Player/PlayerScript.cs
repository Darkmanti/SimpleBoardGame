using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] public ParticleSystem particle;

    public int playerNumber = 0;
    public int pointPosition = 0;

    public int numberOfTurns = 0;
    public int numberOfBuffs = 0;
    public int numberOfFails = 0;

    // stat
    public int place = 0;
    public string playerName;

    public bool isMooving = false;

    public bool isEnd = false;

}
