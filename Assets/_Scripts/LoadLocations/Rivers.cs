using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rivers : MonoBehaviour {

    public Location location;

    public Sprite[] arrPrefabRiverRight;
    public Sprite[] arrPrefabRiverLeft;

    public List<Image> lsPointRight;
    public List<Image> lsPointLeft;

    public void LoadRiverRandom()
    {
        location.lsRiverRight = new List<int>();
        for (int i = 0; i < lsPointRight.Count; i++)
        {
            int random = Random.Range(0, arrPrefabRiverRight.Length);
            lsPointRight[i].sprite = arrPrefabRiverRight[random];
            location.lsRiverRight.Add(random);
        }

        location.lsRiverLeft = new List<int>();
        for (int i = 0; i < lsPointLeft.Count; i++)
        {
            int random = Random.Range(0, arrPrefabRiverLeft.Length);
            lsPointLeft[i].sprite = arrPrefabRiverLeft[random];
            location.lsRiverLeft.Add(random);
        }
        location.isLoaded = true;
    }

    public void LoadRiverJson()
    {
        for (int i = 0; i < lsPointRight.Count; i++)
        {
            lsPointRight[i].sprite = arrPrefabRiverRight[location.lsRiverRight[i]];
        }

        for (int i = 0; i < lsPointLeft.Count; i++)
        {
            lsPointLeft[i].sprite = arrPrefabRiverLeft[location.lsRiverLeft[i]];
        }
        location.isLoaded = true;
    }
}
