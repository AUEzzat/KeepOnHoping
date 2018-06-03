﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour, iHalt
{
    int timeScore;
    int coinScore;
    int coinvalue = 5;
    int secValue = 1;
    int score;
    public int oldCoinCount;
    public Text scoreText;
    public Text coinNum;
    public GameData gData;
    public IEnumerator scoreCoroutine;

    private void Awake()
    {
        RegisterListeners();
        scoreCoroutine = ScorePerSec();
    }

    // Use this for initialization
    void OnEnable()
    {
        timeScore = 0;
        coinScore = 0;
        oldCoinCount = 0;
    }

    // Update is called once per frame
    void Update()
    {

        coinScore = coinvalue * (gData.CoinCount - oldCoinCount) * gData.workersNum;
        // calc score
        score = timeScore + coinScore;
        //Display score
        scoreText.text = score.ToString();
        coinNum.text = gData.CoinCount.ToString();
        oldCoinCount = gData.CoinCount;
    }

    IEnumerator ScorePerSec()
    {
        while (true)
        {
            timeScore += secValue;
            yield return new WaitForSeconds(0.125f);
        }
    }

    public void RegisterListeners()
    {
        gData.OnStart.AddListener(Begin);
        gData.onPause.AddListener(Halt);
        gData.OnResume.AddListener(Resume);
        gData.onEnd.AddListener(End);
        
    }

    public void Begin()
    {
        timeScore = 0;
        coinScore = 0;
        oldCoinCount = 0;
        StartCoroutine(scoreCoroutine);
    }

    public void Halt()
    {
        StopCoroutine(scoreCoroutine);
    }

    public void Resume()
    {
        StartCoroutine(scoreCoroutine);
    }

    public void End()
    {
        Halt();
    }
}