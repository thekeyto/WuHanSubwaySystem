using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class Subway : MonoBehaviour
{
    public Transform stations;
    csvController csvControl;
    public GameObject wayInfo;
    public int totalLines;
    public string csvPath,fileName;
    public Transform tempEmpty;
    public GameObject stationPrefab;
    public GameObject StartStation;
    public GameObject EndStation;
    public GameObject waybutton1;
    public GameObject waybutton2;
    public GameObject waybutton3;
    public bool stationSelect;
    public InputField inputNumber;
    public InputField hourinput;
    public InputField mininput;
    public Material lineMaterial;
    List<GameObject>[] lines=new List<GameObject>[20];
    SubwayMap getMap;
    LineRenderer lineRender;
    ExeCall exeProcess;
    Dictionary<string, GameObject> nameToStation=new Dictionary<string, GameObject>();
    List<Station> checkName=new List<Station>();
    int csvsize;
    List<LineRenderer> subwayrender=new List<LineRenderer>();
    List<LineRenderer> wayrender = new List<LineRenderer>();
    List<List<Vector3>> waypositions = new List<List<Vector3>>();
    Vector3 offsetpos = new Vector3(-684.3f, -192.3f, 0f) - new Vector3(-442.5f, -183.88f, 0);
    PassWay[] ways = new PassWay[20];
    bool ifmatch(string a,string b)
    {
        if (a.Length>b.Length) { string c = a;a = b;b = c; }
        for(int i=0;i<a.Length;i++)
            if (a[i]!=b[i]) return false;
        return true;
    }
    void clearLines()
    {
        wayInfo.SetActive(false);
        for(int i=0;i<totalLines;i++)
        {
            for(int j=0;j<lines[i].Count;j++)
            {
                lines[i][j].GetComponent<Station>().staName.SetActive(false);
                lines[i][j].GetComponent<Station>().staInfo.SetActive(false);
                lines[i][j].GetComponent<Station>().staTime.SetActive(false);
            }
        }
        deleteallways();
    }

    void stationsinit(int ln,int csvid,int id,string name)
    {
        if (checkName.Exists((Station c)=>c.name.Equals(name)))
        {
            nameToStation[name].GetComponent<Station>().ifTransfer = true;
            nameToStation[name].GetComponent<Station>().lineids.Add(ln);
            lines[ln].Add(nameToStation[name]);
            return;
        }
        Vector2 tempse = new Vector2(csvControl.getFloat(csvid, 2), csvControl.getFloat(csvid, 3));
        Vector3 tempPos = getMap.GetWorldPoint(tempse);
        tempEmpty.position = tempPos;
        GameObject tempstation = GameObject.Instantiate(stationPrefab, tempEmpty) as GameObject;
        tempstation.GetComponent<Station>().lineid = ln;
        tempstation.GetComponent<Station>().stationid = id;
        tempstation.GetComponent<Station>().name = name;
        tempstation.GetComponent<Station>().lineids.Add(ln);
        tempstation.GetComponent<Station>().staName.GetComponent<Text>().text = name;
        tempstation.GetComponent<Station>().subway = this.GetComponent<Subway>();
        tempstation.transform.SetParent(stations);
        tempstation.transform.position = tempPos;
        tempstation.name = name;
        //Debug.Log(tempstation.transform.position);
        //if (ln==6)Debug.Log(ln.ToString()+" "+id+" "+tempPos+" "+name);
        //Debug.Log(ln);
        lines[ln].Add(tempstation);
        if (id - 1 >= 0)
        {
            lines[ln][id - 1].GetComponent<Station>().nextstation = name;

            lines[ln][id].GetComponent<Station>().laststation = lines[ln][id - 1].name;
           // Debug.Log(csvid); 
        }
        checkName.Add(tempstation.GetComponent<Station>());
        nameToStation.Add(name, tempstation);            
        //Debug.Log(id.ToString()+" "+lines[ln].Count.ToString());
    }

    void matching(string name,int templineid,int tempstationid)
    {
        //Debug.Log(name + " " + templineid + " " + tempstationid);
        for (int k = 0; k < lines[templineid].Count; k++)
            {
                if (lines[templineid][k].name.Equals(name)==true)
                {
                    lines[templineid][k].GetComponent<Station>().stationid = tempstationid;
                    break;
                }
            }
    }

    void outalllines()
    {
        for(int i=0;i<totalLines;i++)
        {
            List<Vector3> points=new List<Vector3>();
            for (int j = 0; j < lines[i].Count; j++)
                points.Add(lines[i][j].transform.position);
            //Debug.Log(points.Count);
            subwayrender[i].positionCount = lines[i].Count;
            subwayrender[i].SetPositions(points.ToArray());
        }
    }

    void deleteallways()
    {
        for (int i = 0; i < totalLines; i++)
        {
            subwayrender[i].positionCount = 0;
        }
    }
    void MapInit()
    {
        int cnt = 0;
        int tempid = 0;
        
        for(int i = 1; i < csvControl.arrayData.Count; i++)
        {
            if (cnt >= 18) break;
            tempid = 0;
            lines[cnt] = new List<GameObject>();
            string tempname = csvControl.getString(i, 1);
            stationsinit(cnt, i,tempid++,tempname);
            int j = i+1;
            while(csvControl.getInt(j,0)==csvControl.getInt(j-1,0))
            {
                tempname = csvControl.getString(j, 1);
                stationsinit(cnt, j,tempid++,tempname);
                //Debug.Log(j);
                j++;
                if (j >= csvControl.arrayData.Count) break;
            }
            i = j-1;
            cnt++;
        }
        totalLines = cnt;
        Debug.Log(1);
        fileName = "test1.csv";
        int tempflag = 0;
        csvControl.loadFile(csvPath, fileName);
        cnt = 0;
        for (int i = 0; i < totalLines; i++)
            for (int j = 0; j < lines[i].Count; j++)
                lines[i][j].GetComponent<Station>().stationid = -1;
        for (int i = 1; i < csvControl.arrayData.Count; i++)
        {
            if (cnt >= totalLines) break;
            string tempname = csvControl.getString(i, 2);
            int tempstationid = csvControl.getInt(i, 1);
            //Debug.Log(tempname + tempstationid);
            int templineid = cnt;
            matching(tempname, templineid, tempstationid);
            int j = i + 1;
            while (csvControl.getInt(j, 0) == csvControl.getInt(j - 1, 0))
            {
                tempname = csvControl.getString(j, 2);
                tempstationid = csvControl.getInt(j, 1);
                templineid = cnt;
                //Debug.Log(tempname + tempstationid);
                matching(tempname, templineid, tempstationid);
                j++;
                if (j >= csvControl.arrayData.Count) break;
            }
            cnt++;
            i = j-1;
        }

        for (int i = 0; i < totalLines; i++)
        {
            lines[i][0].AddComponent<LineRenderer>();
            subwayrender.Add(lines[i][0].GetComponent<LineRenderer>());
        }
        for(int i=0;i<totalLines;i++)
        {
            subwayrender[i].material = lineMaterial;
            //subwayrender[i].SetColors(Color.red, Color.yellow);
            //设置宽度  
            subwayrender[i].SetWidth(1f, 1f);
        }
        outalllines();
    }

    void Start()
    {
        lineRender = this.GetComponent<LineRenderer>();
        getMap = this.GetComponent<SubwayMap>();
        csvControl = csvController.GetInstance();
        exeProcess = this.GetComponent<ExeCall>();
        csvPath = "D:/myproject/subway_system/Assets/Resources";
        fileName = "positions.csv";
        csvControl.loadFile(csvPath, fileName);
        MapInit();
        //lineout();
        //clearLines();
    }

    public void Startselect()
    {
        stationSelect = false;
    }

    public void DestinationSelect()
    {
        stationSelect = true;
    }

    void lineout()
    {
        wayrender.Clear();
        waypositions.Clear();
        wayInfo.SetActive(false);
        for(int i=0;i<3;i++)
        {
            ways[i] = new PassWay();
        }Debug.Log(csvControl.arrayData.Count);
        clearLines();
        fileName = "ways.csv";
        int templines=0;
        csvControl.loadFile(csvPath, fileName);
        if (csvControl.arrayData.Count==2)
        {
            wayInfo.GetComponent<Text>().text = "无可用路线";
            wayInfo.SetActive(true);
            return;
        }Debug.Log(1);
        for (int i = 2; i < csvControl.arrayData.Count; i++)
        {
            List<Vector3> points = new List<Vector3>();
            int passLineid = csvControl.getInt(i, 2);
            int passStationid = csvControl.getInt(i, 3);
            int tempstationid=0;

            Time temptime = new Time();
            string temphour=csvControl.getString(i,4);
            string tempmin = csvControl.getString(i, 5);
            temptime.hour = temphour;temptime.min = tempmin;

            for (int k = 0; k < lines[passLineid].Count; k++)
                if (lines[passLineid][k].GetComponent<Station>().stationid == passStationid)
                    tempstationid = k;
            points.Add(lines[passLineid][tempstationid].transform.position);
            ways[templines].passstations.Add(lines[passLineid][tempstationid].GetComponent<Station>());
            ways[templines].times.Add(temptime);
            ways[templines].cost = csvControl.getInt(i,8);
            ways[templines].timetake = csvControl.getFloat(i,7);
            ways[templines].crow = csvControl.getFloat(i, 9);
            ways[templines].distance = csvControl.getFloat(i,6);
            int j = i + 1;
            while (csvControl.getInt(j, 0) == csvControl.getInt(j - 1, 0))
            {
                bool flag = false;
                passLineid = csvControl.getInt(j, 2);
                passStationid = csvControl.getInt(j, 3);
                tempstationid = 0;

                temptime = new Time();
                temptime.hour = csvControl.getString(j, 4);
                temptime.min = csvControl.getString(j, 5);
                for (int k = 0; k < lines[passLineid].Count; k++)
                    if (lines[passLineid][k].name == csvControl.getString(j, 1))
                    {
                        flag = true;
                        tempstationid = k;
                    }
                if (flag == false) { j++; if (j >= csvControl.arrayData.Count) break; continue; }
                if (tempstationid<lines[passLineid].Count)
                   // if (lines[passLineid][tempstationid].GetComponent<Station>().stationid==passStationid)
                Debug.Log(lines[passLineid][tempstationid].GetComponent<Station>().stationid.ToString() + " " + passStationid.ToString());
                Debug.Log(templines.ToString()+" "+ csvControl.getString(j,1)+ j.ToString()+" "+passLineid.ToString()+" "+ passStationid.ToString()+" "+tempstationid+" "+ lines[passLineid].Count.ToString());
                if (tempstationid < lines[passLineid].Count)
                {
                    Debug.Log(lines[passLineid][tempstationid].name+" "+ j.ToString() + " " + passLineid.ToString() + " " + passStationid.ToString() + " " + tempstationid + " " + lines[passLineid].Count.ToString());
                    points.Add(lines[passLineid][tempstationid].transform.position);
                    ways[templines].passstations.Add(lines[passLineid][tempstationid].GetComponent<Station>());
                    ways[templines].times.Add(temptime);
                    Debug.Log(ways[templines].passstations[ways[templines].passstations.Count-1].name);
                }
                j++;
                if (j >= csvControl.arrayData.Count) break;
            }
            //Debug.Log(lines[1][30].name + " " + lines[1][30].GetComponent<Station>().stationid) ;
            if (lines[templines][1].GetComponent<LineRenderer>()==null)
             lines[templines][1].AddComponent<LineRenderer>();
            LineRenderer templinerender = lines[templines][1].GetComponent<LineRenderer>();
            //设置颜色 
            templinerender.material = lineMaterial;
            //设置宽度  
            templinerender.SetWidth(0.5f, 0.5f);
            //templinerender.positionCount = points.Count;
            //templinerender.SetPositions(points.ToArray());
            waypositions.Add(points);
            wayrender.Add(templinerender);
            i = j-1;
            ways[templines].points = points;
            ways[templines].linerender = templinerender;
            templines++;
        }
        waybutton3.SetActive(true);
        waybutton2.SetActive(true);
        waybutton1.SetActive(true);
        if (templines < 3) waybutton3.SetActive(false);
        if (templines < 2) waybutton2.SetActive(false);
        if (templines < 1) waybutton1.SetActive(false);
    }

    public void way0out()
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < ways[i].passstations.Count; j++)
            {
                ways[i].passstations[j].staName.SetActive(false);
                ways[i].passstations[j].staTime.SetActive(false);
            }
        wayInfo.GetComponentInChildren<Text>().text = "总距离：" + ways[0].distance.ToString() + "  总花费：" + ways[0].cost.ToString() + " 总用时：" + ways[0].timetake.ToString() + "\n综合拥挤度：" + ways[0].crow.ToString();
        wayInfo.SetActive(true);
        for (int i = 0; i < wayrender.Count; i++)
            wayrender[i].positionCount = 0;
        LineRenderer templinerender = wayrender[0];
        List<Vector3> points = waypositions[0];
        templinerender.positionCount = points.Count;
        templinerender.SetPositions(points.ToArray());
        for(int i=0;i<ways[0].passstations.Count;i++)
        {
            Station tempstation = ways[0].passstations[i];
            string temphour = ways[0].times[i].hour, tempmin = ways[0].times[i].min;
            tempstation.staTime.GetComponent<Text>().text = temphour + ":" + tempmin;
            tempstation.staTime.SetActive(true);
            tempstation.staName.SetActive(true);
        }
    }
    public void way1out()
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < ways[i].passstations.Count; j++)
            {
                ways[i].passstations[j].staName.SetActive(false);
                ways[i].passstations[j].staTime.SetActive(false);
            }
        wayInfo.GetComponentInChildren<Text>().text = "总距离：" + ways[1].distance.ToString() + "  总花费：" + ways[1].cost.ToString() + " 总用时：" + ways[1].timetake.ToString() + "\n综合拥挤度：" + ways[1].crow.ToString();
        wayInfo.SetActive(true);
        for (int i = 0; i < wayrender.Count; i++)
            wayrender[i].positionCount = 0;
        LineRenderer templinerender = wayrender[1];
        List<Vector3> points = waypositions[1];
        templinerender.positionCount = points.Count;
        templinerender.SetPositions(points.ToArray());
        for (int i = 0; i < ways[1].passstations.Count; i++)
        {
            Station tempstation = ways[1].passstations[i];
            string temphour = ways[1].times[i].hour, tempmin = ways[1].times[i].min;
            tempstation.staTime.GetComponent<Text>().text = temphour + ":" + tempmin;
            tempstation.staTime.SetActive(true);
            tempstation.staName.SetActive(true);
        }
    }
    public void way2out()
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < ways[i].passstations.Count; j++)
            {
                ways[i].passstations[j].staName.SetActive(false);
                ways[i].passstations[j].staTime.SetActive(false);
            }
        wayInfo.GetComponentInChildren<Text>().text = "总距离：" + ways[2].distance.ToString() + "  总花费：" + ways[2].cost.ToString() + " 总用时：" + ways[2].timetake.ToString() + "\n综合拥挤度：" + ways[2].crow.ToString();
        wayInfo.SetActive(true);
        for (int i = 0; i < wayrender.Count; i++)
            wayrender[i].positionCount = 0;
        LineRenderer templinerender = wayrender[2];
        List<Vector3> points = waypositions[2];
        templinerender.positionCount = points.Count;
        templinerender.SetPositions(points.ToArray());
        for (int i = 0; i < ways[2].passstations.Count; i++)
        {
            Station tempstation = ways[2].passstations[i];
            if (tempstation.name == "长港路站") Debug.Log("here");
            string temphour = ways[2].times[i].hour, tempmin = ways[2].times[i].min;
            Debug.Log(tempstation.name);
            tempstation.staTime.GetComponent<Text>().text = temphour + ":" + tempmin;
            tempstation.staTime.SetActive(true);
            tempstation.staName.SetActive(true);
        }
    }
    public void determine()
    {
        clearLines();
        exeProcess.startline = StartStation.GetComponent<Station>().lineid.ToString();
        exeProcess.startstation= StartStation.GetComponent<Station>().stationid.ToString();
        exeProcess.endline = EndStation.GetComponent<Station>().lineid.ToString();
        exeProcess.endstation = EndStation.GetComponent<Station>().stationid.ToString();
        exeProcess.standNumber = inputNumber.text;
        exeProcess.hour = hourinput.text;
        exeProcess.min = mininput.text;
        exeProcess.StartExe();
        fileName = "ways.csv";
        csvControl.loadFile(csvPath, fileName);
        lineout();
        encry();
    }

    public void encry()
    {
        List<string> arraydata = new List<string>();
        string line = "";
        using (StreamReader sr = new StreamReader("D:/myproject/subway_system/Assets/Resources/ways.csv"))
        {
            while ((line = sr.ReadLine()) != null)
            {
                arraydata.Add(AESEncryption.myEncrypt(line));
            }
        }

        using (StreamWriter sw = new StreamWriter("D:/myproject/subway_system/Assets/Resources/ways.csv"))
        {
            for (int i = 0; i < arraydata.Count; i++)
            {
                sw.WriteLine(arraydata[i]);
            }
        }

    }

    public void decry()
    {
        List<string> arraydata = new List<string>();
        string line = "";
        using (StreamReader sr = new StreamReader("D:/myproject/subway_system/Assets/Resources/testAes.csv"))
        {
            while ((line = sr.ReadLine()) != null)
            {
                arraydata.Add(AESEncryption.myDecrypt(line));
            }
        }

        using (StreamWriter sw = new StreamWriter("D:/myproject/subway_system/Assets/Resources/testAes.csv"))
        {
            for (int i = 0; i < arraydata.Count; i++)
            {
                sw.WriteLine(arraydata[i]);

            }
        }
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  //camare2D.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.GetComponent<Station>()!=null)
            {
                //Debug.Log("hit:" + hit.collider.gameObject.name);
                hit.collider.gameObject.GetComponent<Station>().MouseStay();
            }
        }
    }
}