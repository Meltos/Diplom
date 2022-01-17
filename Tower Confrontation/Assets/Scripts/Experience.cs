using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Experience : MonoBehaviour
{
    public int Count;

    #region BODY

    void Update()
    {
        GetComponent<Text>().text = Count.ToString();
    }

    #endregion
}
