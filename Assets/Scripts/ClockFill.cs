using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockFill : MonoBehaviour
{
    void Start()
    {
        GetComponent<Image>().fillAmount = 0.0f;
    }
}
