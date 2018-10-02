﻿using UnityEngine;
using DG.Tweening;

public class Decking : MonoBehaviour
{
    public bool isInput;
    public Transform cart;
    public Transform[] tree;
    public GameObject notification;
    public Animator anim;
    public ParticleSystem particleEmissions;

    public SpriteRenderer imgHand;

    private bool isRun;
    private Vector3 posDown;
    private Vector3 posCheck;
    private int random;

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
            LoadInput();
        }
        else
        {
            tree[0].gameObject.SetActive(false);
            tree[1].gameObject.SetActive(false);
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

        imgHand.sprite = UIManager.Instance.spHand[1];
        AudioManager.Instance.Stop("Debarking");
        isRun = false;
    }

    public void LoadInput()
    {
        random = Random.Range(0, 2);
        tree[random].gameObject.SetActive(true);
        tree[1 - random].gameObject.SetActive(false);
        tree[random].localPosition = Vector3.zero;
        tree[random].localEulerAngles = Vector3.zero;
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

        isRun = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        GameManager.Instance.lsLocation[ID].JobComplete(IndexType);
        tree[random].position = posCheck;
        if (random == 0)
        {
            tree[random].DOLocalMove(new Vector3(0f, 2.5f, 0f), 0.5f).OnComplete(() =>
            {
                tree[random].DOLocalRotate(new Vector3(0f, 0f, -45f), 0.5f).OnComplete(() =>
                     CallBackDG(ID, IndexType)
                );
            });
        }
        else
        {
            tree[random].DOLocalMove(new Vector3(1.8f, 0f, 0f), 0.5f).OnComplete(() =>
            {
                tree[random].DOLocalRotate(new Vector3(0f, 0f, -45f), 0.5f).OnComplete(() =>
                     CallBackDG(ID, IndexType)
                );
            });
        }

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
            tree[0].gameObject.SetActive(false);
            tree[1].gameObject.SetActive(false);
            notification.SetActive(true);
        }
    }
}