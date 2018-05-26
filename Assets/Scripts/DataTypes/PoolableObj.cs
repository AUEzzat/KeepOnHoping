﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PoolableObj {
    public PoolableType type;
    public int count;
    public GameObject prefab;

    public string Name
    {
        get
        {
            return type.name;
        }
    }

    public PoolableObj(PoolableType _type, int _count, GameObject _prefab)
    {
        type = _type;
        count = _count;
        prefab = _prefab;
    }
}
