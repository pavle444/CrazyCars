using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class Car : MonoBehaviour
{
    public static float speed = 1;
    private PathCreator pathCreator;
    private GameObject[] paths;
    private bool isMerging;
    private float distance;
    private Transform mergePosition;

    void Start()
    {
        ChoosePath();
    }

    void Update()
    {
        mergePosition = GameObject.FindGameObjectWithTag("SpawnPoint").transform;

        Move();
        Accelerate();
    }

    public void ChoosePath()
    {
        paths = GameObject.FindGameObjectsWithTag("Path");
        pathCreator = paths[Random.Range(0, paths.Length)].GetComponent<PathCreator>();
    }

    void Move()
    {
        distance += speed * Time.deltaTime;

        if(!isMerging)
        {
            transform.position = pathCreator.path.GetPointAtDistance(distance);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distance);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, mergePosition.position, 5 * Time.deltaTime);
        }
    }

    void Accelerate()
    {
        if (Input.touchCount >= 1)
        {
            Manager.dalHold = true;

            if (speed < 3)
            {
                speed += Time.deltaTime;
            }
        }
        else if (speed > 1)
        {
            speed -= Time.deltaTime;
        }
    }

    public void Merge()
    {
        isMerging = true;
    }

    public void RandomDistance()
    {
        distance = Random.Range(0, 50);
    }
}
