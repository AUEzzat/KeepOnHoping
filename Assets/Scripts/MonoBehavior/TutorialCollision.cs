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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCollision : MonoBehaviour
{

    //public GameData gd;

    /// <summary>
    /// Check the tutorial item to allow action according to it
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Worker"))
        {
            WorkerFSM workerFSM = other.GetComponent<WorkerFSM>();
            if (workerFSM.currentState == WorkerState.Worker || workerFSM.currentState == WorkerState.SlaveMerger)
                return;
            switch (tag)
            {
                case "TutJump":
                    TutorialManager.Instance.TutorialState = TutorialState.Jump;
                    break;
                case "TutSlide":
                    TutorialManager.Instance.TutorialState = TutorialState.Slide;
                    break;
                case "TutStrafeLeft":
                    TutorialManager.Instance.TutorialState = TutorialState.LeftStrafe;
                    break;
                case "TutStrafeRight":
                    TutorialManager.Instance.TutorialState = TutorialState.RightStrafe;
                    break;
                case "TutAddWorker":
                    TutorialManager.Instance.TutorialState = TutorialState.AddWorker;
                    break;
                case "TutMerge":
                    TutorialManager.Instance.TutorialState = TutorialState.MergeWorker;
                    break;
                case "TutCollide":
                    workerFSM.health += 1;
                    break;
                case "TutEnd":
                    TutorialManager.Instance.TutorialState = TutorialState.End;
                    break;
            }
            gameObject.SetActive(false);
        }
    }
}