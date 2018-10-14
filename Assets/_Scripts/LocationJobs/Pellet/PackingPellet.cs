using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PackingPellet : MonoBehaviour
{
    public bool isInput;
    public Transform cart;
    public Transform tree;
    public GameObject notification;
    public Animator anim;
    public ParticleSystem particleEmissions;

    public SpriteRenderer imgHand;
    public GameObject tutorialHand;
    public Image imgBG;

    private bool isRun;
    private Vector3 posDown;
    private Vector3 posCheck;
    private Vector3 posCheckHand;
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
                if (Input.mousePosition.x > posDown.x)
                {
                    float dis = Input.mousePosition.x - posDown.x;
                    cart.position += new Vector3(dis * 0.01f * Time.deltaTime, 0f, 0f);
                }
                if (imgHand.transform.position.x > posCheckHand.x)
                {
                    imgHand.enabled = false;
                }
                if (cart.position.x > posCheck.x)
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
            particleEmissions.Play();
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
        particleEmissions.Stop();

        imgHand.sprite = UIManager.Instance.spHand[1];
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
            imgHand.enabled = true;
        });
    }

    public void CompleteJob()
    {
        particleEmissions.Stop();
        anim.enabled = false;
        isRun = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        GameManager.Instance.lsLocation[ID].JobComplete(IndexType);
        cart.localPosition = new Vector3(-1f, 0f, 0f);
        tree.localPosition = Vector3.zero;
        isInput = false;

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

    public void Help()
    {
        tutorialHand.SetActive(true);
    }
}
