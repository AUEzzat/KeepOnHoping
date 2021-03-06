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
using UnityEngine.UI;

public class PickUpCoin : MonoBehaviour
{
    public WorkerConfig wc;
    TileReturner cReturn;
    private CoinMagnet coinMagnet;
    float origYPos;

    void Awake()
    {
        cReturn = GetComponent<TileReturner>();
        coinMagnet = GetComponent<CoinMagnet>();
        RegisterListeners();
        origYPos = transform.position.y;
    }

    public void OnEnable()
    {
        if (PowerUpManager.Instance.magnet.InAct)
        {
            coinMagnet.enabled = true;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Worker" || other.tag == "SlaveMerger")
        {
            AudioManager.Instance.PlaySound("Coin");

            ScoreManager.Instance.coinsCount.Value++;

            if (WorkersManager.Instance.DoubleCoinOn)
                ScoreManager.Instance.coinsCount.Value++;

            StartCoroutine(cReturn.ReturnToPool(0));
        }
    }

    public void RegisterListeners()
    {
        PowerUpManager.Instance.magnet.BeginAction.AddListener(ActWithMagnet);
        PowerUpManager.Instance.magnet.EndAction.AddListener(ActWithoutMagnet);
    }

    public void ActWithMagnet()
    {
        coinMagnet.enabled = true;
    }

    public void ActWithoutMagnet()
    {
        coinMagnet.onAct = false;
    }

    void OnDisable()
    {
        coinMagnet.enabled = false;

        transform.position = new Vector3(0, origYPos, 0);
    }

}
