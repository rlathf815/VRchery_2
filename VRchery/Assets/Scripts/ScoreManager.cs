using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public PlayerData playerData;
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        playerData.score = 0;
    }

    void Update()
    {
        text.text = "Score: " + playerData.score;
    }

    public void Arrow()
    {

    }
}