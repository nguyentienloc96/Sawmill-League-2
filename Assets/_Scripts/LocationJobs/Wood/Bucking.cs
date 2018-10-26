﻿using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Bucking : MonoBehaviour
{
    public bool isInput;
    public Transform cart;
    public Transform tree;
    public GameObject notification;
    public Animator anim;
    public ParticleSystem particleEmissions;

    public GameObject tutorialHand;
    public Image imgBG;

    private bool isRun;
    private Vector3 posDown;
    private Vector3 posCheck;
    private bool isTutorial;
    private bool isStop;

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
            tree.gameObject.SetActive(true);
            LoadInput();
        }
        else
        {
            isStop = true;
            tree.gameObject.SetActive(false);
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
                }
                if (cart.position.y > posCheck.y)
                {
                    CompleteJob();
                }
            }
        }
        else
        {
            if (GameManager.Instance.lsLocation[GameManager.Instance.IDLocation]
               .lsWorking[GameManager.Instance.lsLocation[GameManager.Instance.IDLocation].indexType].input > 0)
            {
                notification.SetActive(false);
                tree.gameObject.SetActive(true);
                LoadInput();
                isStop = false;
            }
        }
    }

    public void TapDown()
    {
        if (isInput)
        {
            tutorialHand.SetActive(false);
            anim.enabled = true;
            particleEmissions.Play();
            AudioManager.Instance.Play("Drill");
            posDown = Input.mousePosition;
            isRun = true;
        }
    }

    public void TapUp()
    {
        anim.enabled = false;
        particleEmissions.Stop();
        AudioManager.Instance.Stop("Drill");
        isRun = false;
    }

    public void LoadInput()
    {
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

    public void CompleteJob()
    {
        anim.enabled = false;
        particleEmissions.Stop();
        isRun = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        tree.GetChild(0).GetChild(0).DOLocalMove(new Vector3(0f, 3f, 0f), 0.5f).OnComplete(() =>
        {
            tree.GetChild(0).GetChild(0).DOLocalRotate(new Vector3(0f, 0f, Random.Range(-90, -45)), 0.5f).OnComplete(()=> tree.GetChild(0).GetChild(0).gameObject.SetActive(false));
            tree.GetChild(0).GetChild(1).DOLocalMove(new Vector3(0f, 3f, 0f), 0.5f).OnComplete(() =>
            {
                tree.GetChild(0).GetChild(1).DOLocalRotate(new Vector3(0f, 0f, Random.Range(-90, -45)), 0.5f).OnComplete(() =>
                {
                    GameManager.Instance.lsLocation[ID].JobComplete(IndexType);
                    tree.GetChild(0).GetChild(0).localPosition = 
                        tree.GetChild(0).GetChild(1).localPosition =
                        tree.GetChild(0).GetChild(0).localEulerAngles = 
                        tree.GetChild(0).GetChild(1).localEulerAngles = Vector3.zero;
                        tree.GetChild(0).GetChild(0).gameObject.SetActive(true);
                    cart.localPosition = new Vector3(-4f, 0f, 0f);
                    tree.localPosition = Vector3.zero;
                    isInput = false;

                    if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
                    {
                        LoadInput();
                    }
                    else
                    {
                        isStop = true;
                        tree.gameObject.SetActive(false);
                        notification.SetActive(true);
                    }
                });
            });
        });
        
    }

    public void Help()
    {
        tutorialHand.SetActive(true);
    }
}
