﻿using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Dryer : MonoBehaviour
{
    public bool isInput;
    public Transform cart;
    public GameObject tree;
    public GameObject notification;
    public Animator anim;
    public Transform needle;
    public SpriteRenderer imgHand;
    public GameObject tutorialHand;
    public Image imgBG;

    private bool isRun;
    private Vector3 posDown;
    private Vector3 posCheck;
    private Vector3 posCheckHand;
    private float timeNeedle;
    private bool isTutorial;
    private bool isStop;

    public void Start()
    {
        posCheck = transform.GetChild(0).position;
        posCheckHand = transform.GetChild(1).position;
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
            isStop = false;
            tree.SetActive(false);
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
                if (imgHand.transform.position.y > posCheckHand.y)
                {
                    imgHand.enabled = false;
                }
                if (cart.position.y > posCheck.y)
                {
                    CompleteJob();
                }
                timeNeedle += Time.deltaTime;
                if (timeNeedle >= 2f)
                {
                    needle.DOLocalRotate(new Vector3(0f, 0f, Random.Range(-90f, 45f)), 1.5f);
                    timeNeedle = 0;
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
            timeNeedle = 0;
            needle.DOLocalRotate(new Vector3(0f, 0f, Random.Range(-90f, 45f)), 1f);
            tutorialHand.SetActive(false);
            anim.enabled = true;
            imgHand.sprite = UIManager.Instance.spHand[0];
            AudioManager.Instance.Play("Debarking");
            posDown = Input.mousePosition;
            isRun = true;
        }
    }

    public void TapUp()
    {
        anim.enabled = false;
        imgHand.sprite = UIManager.Instance.spHand[1];
        AudioManager.Instance.Stop("Debarking");
        isRun = false;
        needle.DOLocalRotate(new Vector3(0f, 0f, 90f), 0.5f);
    }

    public void LoadInput()
    {
        tree.SetActive(true);
        cart.DOLocalMove(Vector3.zero, 1f).OnComplete(() =>
        {
            if (isTutorial)
            {
                tutorialHand.SetActive(true);
                isTutorial = false;
            }
            isInput = true;
            imgHand.enabled = true;
        });
    }

    public void CompleteJob()
    {
        anim.enabled = false;
        isRun = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        GameManager.Instance.lsLocation[ID].JobComplete(IndexType);
        cart.localPosition = new Vector3(-4f, 0f, 0f);
        isInput = false;
        needle.DOLocalRotate(new Vector3(0f, 0f, 90f), 0.5f);

        if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
        {
            tree.SetActive(false);
            LoadInput();
        }
        else
        {
            isStop = false;
            tree.SetActive(false);
            notification.SetActive(true);
        }

    }

    public void Help()
    {
        tutorialHand.SetActive(true);
    }
}
