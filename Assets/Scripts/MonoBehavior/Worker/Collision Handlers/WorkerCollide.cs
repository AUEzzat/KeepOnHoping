﻿/*Licensed to the Apache Software Foundation (ASF) under one
or more contributor license agreements.  See the NOTICE file
distributed with this work for additional information
regarding copyright ownership.  The ASF licenses this file
to you under the Apache License, Version 2.0 (the
"License"); you may not use this file except in compliance
with the License.  You may obtain a copy of the License at

  http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing,
software distributed under the License is distributed on an
"AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
KIND, either express or implied.  See the License for the
specific language governing permissions and limitations
under the License.*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerCollide : IWCollide
{
    Animator animator;
    Rigidbody rb;

    public WorkerCollide(Animator animator, Rigidbody rb)
    {
        this.animator = animator;
        this.rb = rb;
    }

    public void ScriptReset()
    {

    }

    public WorkerStateTrigger Collide(Collider collider, ref int health)
    {
        IObstacle collidableObstacle = collider.GetComponent<IObstacle>();
        // When a worker hits an obstacle it decreases his health by its health
        // and vice versa, if the worker loses all his health he dies
        if (collider.gameObject.CompareTag("Obstacle"))
        {
            int obsHealth = collidableObstacle.Gethealth();
            int preCollisionWH = health;
            health = health - obsHealth;
            collidableObstacle.ReactToCollision(preCollisionWH);
            if (health <= 0)
            {
                collidableObstacle.PlayEffect(animator, rb);
                return WorkerStateTrigger.Die;
            }
        }
        return WorkerStateTrigger.Null;
    }

    public void FixedUpdate(float fixedDeltaTime)
    {

    }
}
