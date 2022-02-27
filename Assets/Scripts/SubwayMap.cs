using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubwayMap : MonoBehaviour
{
    public static SubwayMap Instance = null;

    public Transform BottomRightPoint; //Unity中右下点  （X正方向和Y轴的负方向之间）
    public Transform TopLeftPoint;//Unity中左上点  （Y轴正方向和X轴负方向之间）

    public Vector2 BottomRightSai;
    public Vector2 TopLeftSai;

    LineRenderer linerender;

    private float y_offset, x_offset, y_w_offset, x_w_offset;

    private RaycastHit rayHit;

    private void InitBasicNum()
    {
        //左上经纬度
        TopLeftSai = new Vector2(114.115102271622f, 30.6613123978243f);
        //右下经纬度
        BottomRightSai = new Vector2(114.434284230986f, 30.4410168171139f);
        y_offset = TopLeftSai.y - BottomRightSai.y;//地图中的纬度差  
        x_offset = BottomRightSai.x - TopLeftSai.x;//地图中的经度差  
        y_w_offset = TopLeftPoint.position.y - BottomRightPoint.position.y;//unity中的纬度差  
        x_w_offset = BottomRightPoint.position.x - TopLeftPoint.position.x;//unity中的经度差  
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(this);
        InitBasicNum();
        List<Vector3> points = new List<Vector3>();
        points.Add(TopLeftPoint.position);
        points.Add(BottomRightPoint.position);
        points.Add(new Vector3(100, 100, 0));
        linerender = this.GetComponent<LineRenderer>();
        //linerender.SetPositions(points.ToArray());
    }

    public Vector3 GetWorldPoint(Vector2 se)
    {
        float tempX = se.x - TopLeftSai.x;
        float tempY = se.y - BottomRightSai.y;
        float _tempX = (tempX * x_w_offset / x_offset + TopLeftPoint.position.x);
        float _tempY = (tempY * y_w_offset / y_offset + BottomRightPoint.position.y);
        //坐标偏差（在Unity中的坐标）
        //Debug.Log(tempX.ToString() + " " +tempY.ToString());
       // Debug.Log(new Vector3(_tempX, _tempY, 0));
        return new Vector3(_tempX, _tempY, 0);
    }
    public Vector3 GetLatLon(Vector3 curPoint)
    {
        //坐标偏差
        float _x_offset = (curPoint.x - BottomRightPoint.position.x) * x_offset / x_w_offset;
        float _y_offset = (curPoint.z - TopLeftPoint.position.z) * y_offset / y_w_offset;
        float resultX = _x_offset + BottomRightSai.x;
        float resultY = _y_offset + TopLeftSai.y;
        return new Vector2(resultX, resultY);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
