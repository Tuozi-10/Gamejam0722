using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDrawer : MonoBehaviour {
    [SerializeField] private bool isDrawing;

    [SerializeField] private DiceClass diceData = new DiceClass();
    public DiceClass DiceData => diceData;
}
