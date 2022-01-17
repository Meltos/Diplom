using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToWayPoints : MonoBehaviour
{
    public Vector3 Offset;

    public float Speed;
    public float MaxSpeed;
    public List<Transform> Waypoints;

    private int _curWaypointIndex = 0;

    #region MONO

    private void Awake()
    {
        Speed = MaxSpeed;
    }

    #endregion

    #region BODY

    void Update()
    {
        if(_curWaypointIndex < Waypoints.Count)
        {
            transform.position = Vector3.MoveTowards(transform.position, Waypoints[_curWaypointIndex].position + Offset, Time.deltaTime * Speed / 5);
            transform.LookAt(Waypoints[_curWaypointIndex].position + Offset);
            if (Vector3.Distance(transform.position, Waypoints[_curWaypointIndex].position + Offset) < 0.0001f)
            {
                _curWaypointIndex++;
            }
        }
    }

    #endregion
}
