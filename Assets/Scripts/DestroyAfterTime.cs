using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float timeToDestroy = 2f;

    void Update()
    {
        timeToDestroy -= Time.deltaTime;

        if (timeToDestroy <= 0)
        {
            Destroy(gameObject);
        }
    }
}
