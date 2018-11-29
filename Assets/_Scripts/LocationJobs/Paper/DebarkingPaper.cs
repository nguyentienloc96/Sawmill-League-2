﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DebarkingPaper : MonoBehaviour
{

    public bool isInput;
    public Transform cart;
    public Transform tree;
    public GameObject notification;
    public Animator anim;
    public ParticleSystem particleEmissions;
    public ParticleSystem particleLimbing;
    public Transform propeller;
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
                    propeller.localEulerAngles += new Vector3(0f, 0f, -dis * 5f * Time.deltaTime);
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
            particleLimbing.Play();
            particleEmissions.Play();
            AudioManager.Instance.Play("Debarking");
            posDown = Input.mousePosition;
            isRun = true;
        }
    }

    public void TapUp()
    {
        anim.enabled = false;
        particleLimbing.Stop();
        particleEmissions.Stop();
        AudioManager.Instance.Stop("Debarking");
        isRun = false;
    }

    public void LoadInput()
    {
        cart.localPosition = new Vector3(-4f, 0f, 0f);
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
        particleLimbing.Stop();
        particleEmissions.Stop();
        isRun = false;
        isInput = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.lsLocation[ID].JobComplete(IndexType);
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
