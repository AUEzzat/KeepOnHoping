﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TileReturner))]
public class ObstacleCollisionHandler : MonoBehaviour, ICollidable
{

    public int obsHealth;
    TileReturner objReturner;
    HealthState obstacleState = HealthState.Healthy;

    private void Awake()
    {
        objReturner = GetComponent<TileReturner>();
    }

    public void ReactToCollision(int collidedHealth)
    {
        obsHealth = obsHealth - collidedHealth;
        if (obsHealth <= 0)
        {
            obstacleState = HealthState.Wrecked;
            obsHealth = 1;
            StartCoroutine(objReturner.ReturnToPool(0));
        }
        else
        {
            obstacleState = HealthState.Fractured;
        }
        //-----------------------------------------      
    }

    public int Gethealth()
    {
        return obsHealth;
    }
    //-----------------------------------------------------------------------------------------

}