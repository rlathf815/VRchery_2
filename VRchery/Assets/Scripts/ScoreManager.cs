using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public PlayerData playerData; 

    private void Awake()
    {
        playerData.score = 0;
    }
}