﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PressingHDF : MonoBehaviour
{

    public bool isInput;
    public Transform cart;
    public Transform tree;
    public GameObject notification;
    public Animator anim;
    public Transform[] gear;
    public GameObject tutorialHand;
    public Image imgBG;
    private bool isRun;
    private Vector3 posDown;
    private Vector3 posCheck;
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
            cart.localPosition = new Vector3(-4f, 0f, 0f);
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
                if (Input.mousePosition.x > posDown.x)
                {
                    float dis = Input.mousePosition.x - posDown.x;
                    cart.position += new Vector3(dis * 0.01f * Time.deltaTime, 0f, 0f);
                    foreach (Transform tf in gear)
                    {
                        tf.localEulerAngles += new Vector3(0f, 0f, dis * 5f * Time.deltaTime);
                    }
                }
                if (cart.position.x > posCheck.x)
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
        isRun = false;
        isInput = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        yield return new WaitForSeconds(0.5f);
        double valueOutput = GameManager.Instance.lsLocation[ID].JobComplete(IndexType);
        GameManager.Instance.AddOutPut(valueOutput, iconOutPut, tfStart.position, tfEnd.position);
        cart.localPosition = new Vector3(-1f, 0f, 0f);
        tutorialHand.SetActive(false);
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
    }
}
