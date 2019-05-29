using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLog : MonoBehaviour
{

    private string _area = "undefined";

    public string Area
    {
        get
        {
            return _area;
        }
        set
        {
            _area = value;
        }
    }


}

