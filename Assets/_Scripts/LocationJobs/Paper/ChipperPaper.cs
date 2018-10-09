using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipperPaper : MonoBehaviour
{

    public bool isInput;
    public Transform cart;
    public Transform tree;
    public GameObject notification;
    public Animator anim;
    public Animator animFlour;

    public ParticleSystem particleEmissions;
    public ParticleSystem particleLimbing;

    public Transform gear;
    public Transform gear1;
    public Transform gear2;

    private bool isRun;
    private Vector3 posDown;
    private Vector3 posCheck;
    private bool time;

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
                gear.localEulerAngles += new Vector3(0f, 0f, -dis * 5f * Time.deltaTime);
                gear1.localEulerAngles += new Vector3(0f, 0f, dis * 5f * Time.deltaTime);
                gear2.localEulerAngles += new Vector3(0f, 0f, dis * 5f * Time.deltaTime);
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
            AudioManager.Instance.Play("Debarking");
            posDown = Input.mousePosition;
            isRun = true;
        }
    }

    public void TapUp()
    {
        anim.enabled = false;
        particleEmissions.Stop();

        AudioManager.Instance.Stop("Debarking");
        isRun = false;
    }

    public void LoadInput()
    {
        cart.localPosition = new Vector3(-4f, 0f, 0f);
        cart.DOLocalMove(Vector3.zero, 1f).OnComplete(() =>
        {
            isInput = true;
        });
    }

    public void CompleteJob()
    {
        anim.enabled = false;
        particleEmissions.Stop();
        particleLimbing.Play();
        animFlour.enabled = true;
        isRun = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        gear.DOLocalRotate(gear2.localEulerAngles - new Vector3(0f, 0f, 180f), 1.5f);
        gear1.DOLocalRotate(gear2.localEulerAngles + new Vector3(0f, 0f, 180f), 1.5f);
        gear2.DOLocalRotate(gear2.localEulerAngles + new Vector3(0f, 0f, 180f), 1.5f).OnComplete(() =>
        {
            particleLimbing.Stop();
            animFlour.Rebind();
            animFlour.enabled = false;
            GameManager.Instance.lsLocation[ID].JobComplete(IndexType);
            isInput = false;

            if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
            {
                LoadInput();
            }
            else
            {
                tree.gameObject.SetActive(false);
                notification.SetActive(true);
            }
        });
    }
}
