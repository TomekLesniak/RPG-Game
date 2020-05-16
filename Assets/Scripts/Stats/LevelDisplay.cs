using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;
using UnityEngine.UI;

public class LevelDisplay : MonoBehaviour
{
    private int level = 1;
    private BaseStats baseStats;

    private void Awake()
    {
        baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
    }

    void Update()
    {
        GetComponent<Text>().text = String.Format("{0}", baseStats.GetLevel());
    }
}
