﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TutorialManager : MonoBehaviour
{
    static TutorialManager instance;
    public static TutorialManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<TutorialManager>();

            return instance;
        }
    }

    [SerializeField]
    private TutorialState tutorialState = TutorialState.Null;

    [SerializeField]
    bool active = false;

    public bool Active
    {
        get
        {
#if UNITY_EDITOR
            return active;
#else
            return PlayerPrefs.GetInt("PlayedTutorial") == 0;
#endif
        }
    }

    //public GameData gd;
    public WorkerConfig wc;

    public GameObject pauseBtn;

    public float slowingRatio = 0.1f;
    public float slowingRate = 0.5f;

    public GameObject addWorkerBtn;
    public GameObject leftArrow;
    public GameObject rightArrow;
    public GameObject upArrow;
    public GameObject downArrow;
    public GameObject doubleTap;
    public GameObject buyWorkersText;
    public List<GameObject> mergeTextList;
    public GameObject ScoreText;
    public GameObject GoldText;
    [SerializeField]
    float mergeSpeed = 3;
    [SerializeField]
    float delayBetweenMessages = 2;

    int mergeListIndex;

    [SerializeField]
    Animator addBtnAnimator;

    public TutorialState TutorialState
    {
        get
        {
            return tutorialState;
        }

        set
        {
            tutorialState = value;

            if (Active && value != TutorialState.Null)
            {
                SpeedManager.Instance.speed.Value = 0;

                EnterState();
            }

        }
    }

    private void Start()
    {
        GameManager.Instance.OnStart.AddListener(TutStart);
    }

    public void EnterState()
    {
        //Debug.Log("Entering State: " + TutorialState);
        switch (TutorialState)
        {
            case TutorialState.Jump:
                upArrow.SetActive(true);
                break;
            case TutorialState.Slide:
                downArrow.SetActive(true);
                break;
            case TutorialState.LeftStrafe:
                leftArrow.SetActive(true);
                break;
            case TutorialState.RightStrafe:
                rightArrow.SetActive(true);
                break;
            case TutorialState.AddWorker:
                doubleTap.SetActive(true);
                addBtnAnimator.SetBool("Play", true);
                GoldText.SetActive(true);
                break;
            case TutorialState.MergeWorker:
                SpeedManager.Instance.speed.Value = mergeSpeed;
                StartCoroutine(StartMergeTutorial());
                break;

            case TutorialState.End:
                SpeedManager.Instance.ResetSpeed();
                pauseBtn.SetActive(true);
                gameObject.SetActive(false);

                ScoreText.SetActive(true);
                active = false;

#if !UNITY_EDITOR
                PlayerPrefs.SetInt("PlayedTutorial", 1);
#endif
                break;
        }
    }

    void TutStart()
    {
        //if (gd.tutorialActive && gd.difficulty == 0)
        if (Active)
        {
            pauseBtn.SetActive(false);
            addWorkerBtn.SetActive(false);
            StartCoroutine(BecomeATutor());
            ScoreText.SetActive(false);
            GoldText.SetActive(false);
        }
    }

    IEnumerator BecomeATutor()
    {
        yield return new WaitForSeconds(0.5f);
        WorkersManager.Instance.leader.ChangeState(WorkerStateTrigger.StartTutoring);
    }

    public void ExitState()
    {
        //Debug.Log("Exiting State: " + TutorialState);

        SpeedManager.Instance.ResetSpeed();

        switch (TutorialState)
        {
            case TutorialState.Jump:
                upArrow.SetActive(false);
                break;
            case TutorialState.Slide:
                downArrow.SetActive(false);
                break;
            case TutorialState.LeftStrafe:
                leftArrow.SetActive(false);
                break;
            case TutorialState.RightStrafe:
                addWorkerBtn.SetActive(true);
                rightArrow.SetActive(false);
                break;
            case TutorialState.AddWorker:
                doubleTap.SetActive(false);
                addBtnAnimator.SetBool("Play", false);
                ScoreManager.Instance.coinsCount.Value = 0;
                break;
        }
        TutorialState = TutorialState.Null;
    }

    IEnumerator StartMergeTutorial()
    {
        buyWorkersText.SetActive(true);
        WorkersManager.Instance.leader.ChangeState(WorkerStateTrigger.EndTutoring);
        yield return new WaitForSeconds(3);

        for (int i = 0; i < 3; i++)
        {
            WorkersManager.Instance.AddWorker();
        }

        StartCoroutine(CollideTut());
    }

    IEnumerator CollideTut()
    {
        mergeTextList[mergeListIndex].SetActive(true);
        yield return new WaitForSeconds(delayBetweenMessages);

        mergeTextList[mergeListIndex].SetActive(false);
        mergeTextList[++mergeListIndex].SetActive(true);
        yield return new WaitForSeconds(delayBetweenMessages);

        mergeTextList[mergeListIndex].SetActive(false);

        StartCoroutine(EndMergeCollide());
    }

    IEnumerator EndMergeCollide()
    {
        mergeTextList[++mergeListIndex].SetActive(true);
        yield return new WaitForSeconds(delayBetweenMessages);

        SpeedManager.Instance.ResetSpeed();
        mergeTextList[mergeListIndex].SetActive(false);
        GameManager.Instance.OnStart.RemoveListener(TutStart);
    }
}
