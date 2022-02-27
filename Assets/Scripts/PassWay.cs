using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Time
{
    public string hour;
    public string min;
}

public class PassWay : MonoBehaviour
{
    public List<Vector3> points;
    public LineRenderer linerender;
    public List<Station> passstations=new List<Station>();
    public int cost;
    public float timetake;
    public float crow;
    public float distance;
    public List<Time> times=new List<Time>();
}
