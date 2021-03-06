﻿using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public class Planing : MonoBehaviour
{
    public bool isInput;
    public Transform cart;
    public GameObject[] tree;
    public GameObject notification;
    public Animator anim;
    public ParticleSystem particleEmissions;
    public Transform level1;
    public Transform level2;
    public Transform level3;

    public GameObject tutorialHand;
    public Image imgBG;

    private bool isRun;
    private Vector3 posDown;
    private Vector3 posCheck;
    private int random;
    private bool isTutorial;
    private bool isStop;

    public Transform tfStart;
    public Transform tfEnd;
    public Sprite iconOutPut;

    public void Start()
    {
        posCheck = transform.GetChild(0).position;
    }

    private void OnEnable()
    {
        int randomBG = Random.Range(0, UIManager.Instance.spBG.Length);
        imgBG.sprite = UIManager.Instance.spBG[randomBG];
        isTutorial = true;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
        {
            notification.SetActive(false);
            LoadInput();
        }
        else
        {
            isStop = true;
            HideTree();
            notification.SetActive(true);
        }
    }

    public void Update()
    {
        if (!isStop)
        {
            if (isRun)
            {
                if (Input.mousePosition.y > posDown.y)
                {
                    float dis = Input.mousePosition.y - posDown.y;
                    cart.position += new Vector3(0f, dis * 0.01f * Time.deltaTime, 0f);
                    level1.localEulerAngles += new Vector3(0f, 0f, dis * 5f * Time.deltaTime);
                    level2.localEulerAngles -= new Vector3(0f, 0f, dis * 5f * Time.deltaTime);
                    level3.localEulerAngles += new Vector3(0f, 0f, dis * 5f * Time.deltaTime);

                }
                if (cart.position.y > posCheck.y)
                {
                    StartCoroutine(CompleteJob());
                }
            }
        }
        else
        {
            if (GameManager.Instance.lsLocation[GameManager.Instance.IDLocation]
               .lsWorking[GameManager.Instance.lsLocation[GameManager.Instance.IDLocation].indexType].input > 0)
            {
                notification.SetActive(false);
                LoadInput();
                isStop = false;
            }
        }
    }

    public void TapDown()
    {
        if (isInput)
        {
            anim.enabled = true;
            particleEmissions.Play();
            AudioManager.Instance.Play("Polish");
            posDown = Input.mousePosition;
            isRun = true;
        }
    }

    public void TapUp()
    {
        anim.enabled = false;
        particleEmissions.Stop();
        AudioManager.Instance.Stop("Polish");
        isRun = false;
    }

    public void LoadInput()
    {
        random = Random.Range(0, tree.Length);
        tree[random].SetActive(true);
        cart.DOLocalMove(Vector3.zero, 1f).OnComplete(() =>
        {
            if (isTutorial)
            {
                tutorialHand.SetActive(true);
                isTutorial = false;
            }
            isInput = true;
        });
    }

    public IEnumerator CompleteJob()
    {
        anim.enabled = false;
        particleEmissions.Stop();
        isRun = false;
        isInput = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        yield return new WaitForSeconds(0.5f);
        double valueOutput = GameManager.Instance.lsLocation[ID].JobComplete(IndexType);
        GameManager.Instance.AddOutPut(valueOutput, iconOutPut, tfStart.position, tfEnd.position);
        cart.localPosition = new Vector3(-4f, 0f, 0f);
        tutorialHand.SetActive(false);
        if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
        {
            tree[random].SetActive(false);
            LoadInput();
        }
        else
        {
            isStop = true;
            tree[random].SetActive(false);
            notification.SetActive(true);
        }

    }

    public void HideTree()
    {
        foreach (GameObject obj in tree)
        {
            obj.SetActive(false);
        }
    }

    public void Help()
    {
        tutorialHand.SetActive(true);
    }
}
