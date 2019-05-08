using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    public Color gizmosColor = new Color(0.9f, 0.5f, 0.5f, 0.5f);

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
