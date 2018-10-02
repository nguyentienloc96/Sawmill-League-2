using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

[System.Serializable]
public struct Cart
{
    public GameObject[] gTree;
    public SpriteRenderer[] spTree;
}

public class Painting : MonoBehaviour
{

    public bool isInput;
    public Transform cart;
    public Transform pen;
    public Animator anim;
    public GameObject notification;


    public List<Transform> way;
    public List<Cart> lsCart;

    private bool isRun;
    private Vector3 posDown;
    private int random;
    private int indexPos;
    private int indexTree;


    private void OnEnable()
    {
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
        {
            notification.SetActive(false);
            LoadInput();
        }
        else
        {
            notification.SetActive(true);
        }
    }

    public void Update()
    {
        if (isRun)
        {
            if ((indexPos + 1) < way.Count)
            {
                if (way[indexPos].localPosition.y == way[indexPos + 1].localPosition.y)
                {
                    if (Input.mousePosition.x > posDown.x)
                    {
                        float dis = Input.mousePosition.x - posDown.x;
                        pen.localPosition += new Vector3(dis * 0.01f * Time.deltaTime, 0f, 0f);
                        if (pen.localPosition.x >= way[indexPos + 1].localPosition.x)
                        {
                            pen.localPosition = way[indexPos + 1].localPosition;
                            if (indexPos == 1 || indexPos == 2 || indexPos == 5 || indexPos == 6 || indexPos == 9 || indexPos == 10)
                            {
                                lsCart[indexTree].spTree[random].color = new Color32(255, 255, 255, 255);
                                indexTree++;
                            }
                            indexPos++;
                        }
                    }
                    pen.localEulerAngles = new Vector3(0f, 0f, -90f);
                }
                else
                {
                    if (way[indexPos].localPosition.y > way[indexPos + 1].localPosition.y)
                    {
                        if (Input.mousePosition.y < posDown.y)
                        {
                            float dis = Input.mousePosition.y - posDown.y;
                            pen.localPosition += new Vector3(0f, dis * 0.01f * Time.deltaTime, 0f);
                            if (pen.localPosition.y <= way[indexPos + 1].localPosition.y)
                            {
                                pen.localPosition = way[indexPos + 1].localPosition;
                                if (indexPos == 1 || indexPos == 2 || indexPos == 5 || indexPos == 6 || indexPos == 9 || indexPos == 10)
                                {
                                    lsCart[indexTree].spTree[random].color = new Color32(255, 255, 255, 255);
                                    indexTree++;
                                }
                                indexPos++;
                            }
                        }
                        pen.localEulerAngles = new Vector3(0f, 0f, -180f);
                    }
                    else if (way[indexPos].localPosition.y < way[indexPos + 1].localPosition.y)
                    {
                        if (Input.mousePosition.y > posDown.y)
                        {
                            float dis = Input.mousePosition.y - posDown.y;
                            pen.localPosition += new Vector3(0f, dis * 0.01f * Time.deltaTime, 0f);
                            if (pen.localPosition.y >= way[indexPos + 1].localPosition.y)
                            {
                                pen.localPosition = way[indexPos + 1].localPosition;
                                if (indexPos == 1 || indexPos == 2 || indexPos == 5 || indexPos == 6 || indexPos == 9 || indexPos == 10)
                                {
                                    lsCart[indexTree].spTree[random].color = new Color32(255, 255, 255, 255);
                                    indexTree++;
                                }
                                indexPos++;
                            }
                        }
                        pen.localEulerAngles = new Vector3(0f, 0f, 0f);
                    }
                }
            }
            else
            {
                CompleteJob();
            }
        }
    }

    public void TapDown()
    {
        if (isInput)
        {
            anim.enabled = true;
            AudioManager.Instance.Play("Debarking");
            posDown = Input.mousePosition;
            isRun = true;
        }
    }

    public void TapUp()
    {
        anim.enabled = false;
        AudioManager.Instance.Stop("Debarking");
        isRun = false;
    }

    public void LoadInput()
    {
        random = Random.Range(0, 4);
        for(int i = 0; i < lsCart.Count; i++)
        {
            lsCart[i].gTree[random].SetActive(true);
        }
        indexTree = 0;
        indexPos = 0;
        cart.localPosition = new Vector3(-10f, 0f, 0f);
        pen.localPosition = way[indexPos].localPosition;
        cart.DOLocalMove(Vector3.zero, 0.5f).OnComplete(() =>
        {
            isInput = true;
        });
    }

    public void CompleteJob()
    {
        for (int i = 0; i < lsCart.Count; i++)
        {
            lsCart[i].gTree[random].SetActive(false);
            lsCart[i].spTree[random].color = new Color32(255, 255, 255, 0);
        }
        anim.enabled = false;
        isRun = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        GameManager.Instance.lsLocation[ID].JobComplete(IndexType);
        if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
        {
            isInput = false;
            LoadInput();
        }
        else
        {
            notification.SetActive(true);
        }
    }
}
