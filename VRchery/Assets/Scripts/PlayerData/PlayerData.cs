using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Player Data", order = 1)]
public class PlayerData : ScriptableObject
{
    public string PlayerName;
    public int score;

}