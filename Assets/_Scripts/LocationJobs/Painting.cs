using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

[System.Serializable]
public struct Way
{
    public Transform[] arrTf;
    //[HideInInspector]
    public Vector3[] arrDis;
}

public class Painting : MonoBehaviour
{

    public Transform cart;
    public Transform pen;

    public List<Way> way;

    public int index;

    public void Start()
    {
        index = 0;
        foreach(Way w in way)
        {
            for(int i = 0;i< w.arrTf.Length; i++)
            {
                w.arrDis[i] = w.arrTf[i].localPosition;
            }
        }
        pen.localPosition = way[0].arrDis[0];
        cart.DOLocalMove(Vector3.zero, 1f);
        pen.DOLocalPath(way[0].arrDis,2f);
    }

    public void TapDownPen()
    {
        index++;
        //pen.DOLocalMove(arrPos[index], 1f).OnComplete(() =>
        //{
        //    if ((index + 1) < arrPos.Length)
        //    {
        //        if (pen.localPosition.y == arrPos[index + 1].y)
        //        {
        //            pen.localEulerAngles = new Vector3(0f, 0f, -90f);
        //        }
        //        else
        //        {
        //            if (pen.localPosition.y < arrPos[index + 1].y)
        //            {
        //                pen.localEulerAngles = new Vector3(0f, 0f, 0f);
        //            }
        //            else
        //            {
        //                pen.localEulerAngles = new Vector3(0f, 0f, -180f);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        cart.localPosition = new Vector3(-10f, 0f, 0f);
        //        LoadInput();
        //    }
        //});

    }

    public void LoadInput()
    {
        index = 0;
        //pen.localPosition = arrPos[index];
        pen.localEulerAngles = new Vector3(0f, 0f, -90f);
        cart.DOLocalMove(Vector3.zero, 1f);
    }
}
