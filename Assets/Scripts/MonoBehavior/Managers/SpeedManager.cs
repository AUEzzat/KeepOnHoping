﻿using UnityEngine;

/// <summary>
/// This Script is responsible of changing in speed of the game.
/// </summary>
public class SpeedManager : MonoBehaviour
{
    public static SpeedManager Instance;

    public FloatField speed;
    
    private void Awake()
    {
        if (Instance = null)
        {
            Instance = this;
        }

    }

    //public float GetSpeedValue()
    //{
    //    return speed.Value;
    //}

    public void SetSpeedValue(float val)
    {
        speed.oldValue = speed.Value;
        speed.Value= val;
    }


}
