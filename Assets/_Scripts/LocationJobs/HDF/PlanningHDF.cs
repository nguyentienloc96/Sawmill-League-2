using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlanningHDF : MonoBehaviour {

	public bool isInput;
    public Transform cart;
    public Transform tree;
    public GameObject notification;
    public Animator anim;
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
			cart.localPosition = new Vector3(-4f, 0f, 0f);
            notification.SetActive(false);
            tree.gameObject.SetActive(true);
            LoadInput();
        }
        else
        {
            isStop = false;
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
            AudioManager.Instance.Play("Polish");
            posDown = Input.mousePosition;
            isRun = true;
            tutorialHand.SetActive(false);
        }
    }

    public void TapUp()
    {
        anim.enabled = false;
        AudioManager.Instance.Stop("Polish");
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
        GameManager.Instance.lsLocation[ID].JobComplete(IndexType);
        cart.localPosition = new Vector3(-4f, 0f, 0f);

        if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
        {
            LoadInput();
        }
        else
        {
            isStop = false;
            tree.gameObject.SetActive(false);
            notification.SetActive(true);
        }
    }
}
