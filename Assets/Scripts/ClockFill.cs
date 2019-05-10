using UnityEngine;
using UnityEngine.UI;

public class ClockFill : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Image>().fillAmount = 0.0f;
    }
}
