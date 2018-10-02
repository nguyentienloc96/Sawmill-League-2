﻿using UnityEngine;
using DG.Tweening;

public class Canting : MonoBehaviour
{
    public bool isInput;
    public Transform cart;
    public Transform tree;
    public GameObject notification;
    public Animator anim;
    public ParticleSystem particleEmissions;
    public ParticleSystem particleCanting1;
    public ParticleSystem particleCanting2;


    public SpriteRenderer imgHand;

    private bool isRun;
    private Vector3 posDown;
    private Vector3 posCheck;

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
            tree.gameObject.SetActive(true);
            notification.SetActive(false);
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
        }
    }

    public void TapDown()
    {
        if (isInput)
        {
            anim.enabled = true;
            particleEmissions.Play();
            particleCanting1.Play();
            particleCanting2.Play();

            imgHand.sprite = UIManager.Instance.spHand[0];
            AudioManager.Instance.Play("Debarking");
            posDown = Input.mousePosition;
            isRun = true;
        }
    }

    public void TapUp()
    {
        anim.enabled = false;
        particleEmissions.Stop();
        particleCanting1.Stop();
        particleCanting2.Stop();

        imgHand.sprite = UIManager.Instance.spHand[1];
        AudioManager.Instance.Stop("Debarking");
        isRun = false;
    }

    public void LoadInput()
    {
        tree.localPosition = Vector3.zero;
        tree.GetChild(0).localEulerAngles = Vector3.zero;
        tree.GetChild(1).localEulerAngles = Vector3.zero;
        cart.localPosition = new Vector3(-4f, 0f, 0f);
        cart.DOLocalMove(Vector3.zero, 1f).OnComplete(() =>
        {
            isInput = true;
            imgHand.enabled = true;
        });
    }

    public void CompleteJob()
    {
        anim.enabled = false;
        particleEmissions.Stop();
        particleCanting1.Stop();
        particleCanting2.Stop();
        isRun = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        GameManager.Instance.lsLocation[ID].JobComplete(IndexType);
        tree.DOLocalMove(new Vector3(0f, 3.2f, 0f), 0.5f).OnComplete(() =>
        {
            tree.GetChild(0).DOLocalRotate(new Vector3(0f, 0f, -45f), 0.5f);
            tree.GetChild(1).DOLocalRotate(new Vector3(0f, 0f, -45f), 0.5f).OnComplete(() =>
                CallBackDG(ID, IndexType)
            );
        });

    }

    public void CallBackDG(int ID, int IndexType)
    {
        if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
        {
            imgHand.enabled = false;
            isInput = false;
            LoadInput();
        }
        else
        {
            tree.gameObject.SetActive(false);
            notification.SetActive(true);
        }
    }
}