using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    [SerializeField] public List<WayPoint> connectedPoints;


    public WayPoint GetRandomPointPosition()
    {
        return connectedPoints[Random.Range(0,connectedPoints.Count)];
    }
    
}
