using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CoolerPellet : MonoBehaviour
{
    public bool isInput;
    public Transform cart;
    public Transform tree;
    public GameObject notification;
    public Animator anim;
    public Transform lever;

    public Transform needle;
    public Transform needle1;

    public SpriteRenderer imgHand;
    public GameObject tutorialHand;
    public Image imgBG;

    private bool isRun;
    private Vector3 posDown;
    private Vector3 posCheck;
    private float timeNeedle;
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
                if (Input.mousePosition.y < posDown.y)
                {
                    float dis = Input.mousePosition.y - posDown.y;
                    cart.position -= new Vector3(dis * 0.01f * Time.deltaTime, 0f, 0f);
                    lever.localEulerAngles -= new Vector3(0f, 0f, dis * 2.5f * Time.deltaTime);
                }
                if (cart.position.x > posCheck.x)
                {
                    CompleteJob();
                }
                timeNeedle += Time.deltaTime;
                if (timeNeedle >= 2f)
                {
                    needle.DOLocalRotate(new Vector3(0f, 0f, Random.Range(-90f, 45f)), 1.5f);
                    needle1.DOLocalRotate(new Vector3(0f, 0f, Random.Range(-90f, 45f)), 1f);
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
            needle.DOLocalRotate(new Vector3(0f, 0f, Random.Range(-90f, 45f)), 1f);
            needle1.DOLocalRotate(new Vector3(0f, 0f, Random.Range(-90f, 45f)), 1f);
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
        needle.DOLocalRotate(new Vector3(0f, 0f, 90f), 0.5f);
        needle1.DOLocalRotate(new Vector3(0f, 0f, 90f), 0.5f);
        anim.enabled = false;
        isRun = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        GameManager.Instance.lsLocation[ID].JobComplete(IndexType);
        lever.localEulerAngles = Vector3.zero;
        cart.localPosition = new Vector3(-1.5f, 0f, 0f);
        tree.localPosition = Vector3.zero;
        imgHand.enabled = false;
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
