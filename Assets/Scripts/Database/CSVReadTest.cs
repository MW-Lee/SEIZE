using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVReadTest : MonoBehaviour
{
    public int _exp = 0;

    public List<Soldiers> lSoldierList = new List<Soldiers>();

    private void Start()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("Test");               

        for (int i = 0; i < data.Count; i++)
        {
            Soldiers temp = new Soldiers();
            Stats tempstat = new Stats();

            temp.sSoldierName = data[i]["name"].ToString();

            if (data[i]["class"].ToString() == "W") temp.eSoldierType = SoldierType.Warrior;
            else temp.eSoldierType = SoldierType.Magician;

            int.TryParse(data[i]["ad"].ToString(), out tempstat.AD);
            int.TryParse(data[i]["ap"].ToString(), out tempstat.AP);
            float.TryParse(data[i]["hp"].ToString(), out tempstat.HP);
            temp.sSoldierStat = tempstat;

            temp.sSoldierInfo = data[i]["info"].ToString();

            temp.iSoldierImg = Resources.Load("Char/" + temp.sSoldierName, typeof(Sprite)) as Sprite;
            temp.iSoldierImgBig = Resources.Load("Char_Big/" + temp.sSoldierName, typeof(Sprite)) as Sprite;

            lSoldierList.Add(temp);

            print(lSoldierList[i].sSoldierName);
        }
    }
}
