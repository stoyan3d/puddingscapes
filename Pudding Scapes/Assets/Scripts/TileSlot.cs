using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSlot {

    public GameObject tileObj;
    public string type;

    public TileSlot(GameObject obj, string t)
    {
        tileObj = obj;
        type = t;
    }
}
