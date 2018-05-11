﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lanes", menuName = "Config/Lane/Lanes")]
public class Lanes : ScriptableObject
{
    public TileConfig tc;
    public float laneCount = 3;//number of available lanes
    public float laneWidth;//width of each seperate lane

    [SerializeField]
    List<LaneName> onGridLanes;
    [SerializeField]
    List<LaneName> gridLanes;

    [SerializeField]
    private LaneName currentLane;
    [SerializeField]
    private LaneName lastLane;
    private int currentLaneIndex;

    public LaneName CurrentLane
    {
        get
        {
            return currentLane;
        }
    }

    public int CurrentLaneIndex
    {
        get
        {
            return currentLaneIndex;
        }
    }

    public List<LaneName> GridLanes
    {
        get
        {
            return gridLanes;
        }
    }

    public List<LaneName> OnGridLanes
    {
        get
        {
            return onGridLanes;
        }
    }

    private void OnEnable()
    {
        laneWidth = tc.laneWidth;

        //start with middle lane
        currentLaneIndex = 2;
        currentLane = gridLanes[2];
        onGridLanes = new List<LaneName>();
        OnGridLanes.Add(currentLane);

        //initialize left and right lanes
        for (int i = 1; i < laneCount / 2; i++)
        {
            gridLanes[2 - i].laneCenter = -laneWidth * i;
            OnGridLanes.Insert(0, gridLanes[2 - i]);
            gridLanes[2 + i].laneCenter = laneWidth * i;
            OnGridLanes.Add(gridLanes[2 + i]);
        }

    }

    //get lane by index
    public LaneName this[int index]
    {
        get
        {
            index %= onGridLanes.Count;
            return onGridLanes[index];
        }
    }

    //get lane index by lane
    public int this[LaneName laneName]
    {
        get { return OnGridLanes.IndexOf(laneName); }
    }

    //return the next lane left
    //or the same lane if it were that last to the left
    public float GoLeft()
    {
        if (currentLaneIndex == 0)
        {
            return currentLane.laneCenter;
        }
        lastLane = currentLane;
        currentLane = OnGridLanes[--currentLaneIndex];
        return currentLane.laneCenter;
    }


    //return the next lane right
    //or the same lane if it were that last to the right
    public float GoRight()
    {
        if (currentLaneIndex == OnGridLanes.Count - 1)
        {
            return currentLane.laneCenter;
        }
        lastLane = currentLane;
        currentLane = OnGridLanes[++currentLaneIndex];
        return currentLane.laneCenter;
    }

    //Add a lane to the left
    public bool AddLeft()
    {
        //add a lane to the left if there is no more than 1 lane to the left
        if (OnGridLanes[0].LaneNum > 0)
        {
            gridLanes[OnGridLanes[0].LaneNum - 1].laneCenter = laneWidth * (gridLanes[OnGridLanes[0].LaneNum - 1].LaneNum - 2);
            OnGridLanes.Insert(0, gridLanes[OnGridLanes[0].LaneNum - 1]);
            return true;
        }
        return false;
    }

    //Add a lane to the right
    public bool AddRight()
    {
        //add a lane to the right if there is no more than 1 lane to the right
        if (OnGridLanes[OnGridLanes.Count - 1].LaneNum < 4)
        {
            gridLanes[OnGridLanes[0].LaneNum + 1].laneCenter = laneWidth * (gridLanes[OnGridLanes[0].LaneNum + 1].LaneNum - 2);
            OnGridLanes.Add(gridLanes[OnGridLanes[OnGridLanes.Count - 1].LaneNum + 1]);
            return true;
        }
        return false;
    }

    //Remove a lane from the left
    public bool RemoveLeft()
    {
        //remove a lane from the left if there is at least two lanes to the left of the middle one
        if (OnGridLanes[0].LaneNum < 1)
        {
            OnGridLanes.RemoveAt(0);
            return true;
        }
        return false;
    }

    //Remove a lane from the right
    public bool RemoveRight()
    {
        //remove a lane from the right if there is at least two lanes to the right of the middle one
        if (OnGridLanes[OnGridLanes.Count - 1].LaneNum > 3)
        {
            OnGridLanes.RemoveAt(OnGridLanes.Count - 1);
            return true;
        }
        return false;
    }
}