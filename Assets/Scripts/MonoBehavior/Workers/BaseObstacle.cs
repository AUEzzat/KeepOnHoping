﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectReturner))]
public class BaseObstacle : MonoBehaviour, ICollidable
{

    public int obsHealth;
    public Vector3 obsVelocity;
    ObjectReturner objReturner;
    obstacleState state = obstacleState.Idle;

    enum obstacleState
    {
        Idle = 1, // No one collide with it yet
        Destroyed = 2, //the obstacle is destroid 
        Broken = 3  // lesa feh shewaia health metb2e
    }

    private void Start()
    {
        objReturner = GetComponent<ObjectReturner>();
    }

    public void ReactToCollision(int collidedHealth)
    {
        obsHealth = obsHealth - collidedHealth;
        if (obsHealth <= 0)
        {
            state = obstacleState.Destroyed;
            objReturner.ReturnToObjectPool();
        }
        else
        {
            state = obstacleState.Broken;
        }
        //-----------------------------------------      
    }

    public int Gethealth()
    {
        return obsHealth;
    }
    //-----------------------------------------------------------------------------------------

}
