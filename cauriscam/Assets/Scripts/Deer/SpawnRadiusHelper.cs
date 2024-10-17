using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRadiusHelper : MonoBehaviour
{
    public bool drawDistanceSphere = true;
    public float activationDistance = 10f;
    void OnDrawGizmosSelected()
    {
        if (drawDistanceSphere)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, activationDistance);
        }
    }
}
