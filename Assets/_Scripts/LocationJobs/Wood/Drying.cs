﻿using UnityEngine;
using DG.Tweening;

public class Drying : MonoBehaviour
{
    public bool isInput;
    public Transform cart;
    public Transform tree;
    public Transform treeMask;
    public GameObject notification;
    public Animator anim;
    public Transform needle;

    public SpriteRenderer imgHand;

    private bool isRun;
    private Vector3 posDown;
    private Vector3 posCheck;
    private float timeNeedle;

    public void Start()
    {
        posCheck = transform.GetChild(0).position;
    }

    private void OnEnable()
    {
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
            tree.gameObject.SetActive(false);
            notification.SetActive(true);
        }
    }

    public void Update()
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

            timeNeedle += Time.deltaTime;
            if(timeNeedle >= 2f)
            {
                needle.DOLocalRotate(new Vector3(0f, 0f, Random.Range(-90f, 45f)), 1.5f);
                timeNeedle = 0;
            }
        }
    }

    public void TapDown()
    {
        if (isInput)
        {
            timeNeedle = 0;
            needle.DOLocalRotate(new Vector3(0f, 0f, Random.Range(-90f, 45f)), 1f);
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
        
        cart.DOLocalMove(Vector3.zero, 1f).OnComplete(() =>
        {
            isInput = true;
            imgHand.enabled = true;
        });
    }

    public void CompleteJob()
    {
        anim.enabled = false;
        needle.DOLocalRotate(new Vector3(0f, 0f, 90f), 0.5f);
        isRun = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        GameManager.Instance.lsLocation[ID].JobComplete(IndexType);
        treeMask.DOLocalMove(new Vector3(0f, 1.8f, 0f), 1f).OnComplete(() =>
        {
            treeMask.DOLocalRotate(new Vector3(0f, 0f, -45f), 0.25f).OnComplete(() =>
            {
                treeMask.localPosition = Vector3.zero;
                treeMask.localEulerAngles = Vector3.zero;
                cart.localPosition = new Vector3(-4f, 0f, 0f);
                treeMask.localPosition = Vector3.zero;
                needle.localEulerAngles = new Vector3(0f, 0f, 90f);
                imgHand.enabled = false;

                if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
                {
                    isInput = false;
                    LoadInput();
                }
                else
                {
                    tree.gameObject.SetActive(false);
                    notification.SetActive(true);
                }
            });          
        });
        
    }
}
