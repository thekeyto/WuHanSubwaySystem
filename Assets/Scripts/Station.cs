using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Station : MonoBehaviour
{
    public GameObject staInfo;
    public GameObject staName;
    public GameObject staTime;
    public Vector2 position;
    public int lineid;
    public int stationid;
    public string nextstation;
    public string laststation;
    public Subway subway;
    public bool ifTransfer=false;
    public bool ifEndStation;
    public List<int> lineids;
    public List<int> stationids;
    public void whenOnClick()
    {
        if (subway.stationSelect == false)
        {
            subway.StartStation = this.gameObject;
        }
        else if (subway.stationSelect == true)
        {
            subway.EndStation = this.gameObject;
        }
    }
    IEnumerator waitforInfo()
    {
        yield return new WaitForSeconds(0.5f);
        staInfo.SetActive(false);
    }
    public void MouseStay()
    {
        if (staInfo.active == true) return;
        if (ifTransfer==false)
        {
            staInfo.GetComponent<Text>().text ="本站："+this.gameObject.name+"\n上一站：" + laststation + "\n下一站：" + nextstation; 
        }
        else
        {
            staInfo.GetComponent<Text>().text = "本站："+this.gameObject.name;
            staInfo.GetComponent<Text>().text += "\n中转站:";
            for(int i=0;i<lineids.Count;i++)
            staInfo.GetComponent<Text>().text += lineids[i].ToString();
        }
        staInfo.SetActive(true);
        StartCoroutine(waitforInfo());
    }
}
