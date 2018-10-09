using UnityEngine;
using DG.Tweening;

public class DrumDryerPellet : MonoBehaviour
{
    public bool isInput;
    public Transform cart;
    public Transform tree;
    public GameObject notification;
    public Animator anim;
    public ParticleSystem particleEmissions;
    public Transform lever;

    public SpriteRenderer imgHand;
    public GameObject tutorialHand;

    private bool isRun;
    private Vector3 posDown;
    private Vector3 posCheck;
    private bool isTutorial;

    public void Start()
    {
        posCheck = transform.GetChild(0).position;
    }

    private void OnEnable()
    {
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
            tree.gameObject.SetActive(false);
            notification.SetActive(true);
        }
    }

    public void Update()
    {
        if (isRun)
        {
            if (Input.mousePosition.y < posDown.y)
            {
                float dis = Input.mousePosition.y - posDown.y;
                cart.position -= new Vector3(dis * 0.01f * Time.deltaTime,0f, 0f);
                lever.localEulerAngles += new Vector3(0f, 0f, dis * 2.5f * Time.deltaTime);
            }
            if (cart.position.x > posCheck.x)
            {
                CompleteJob();
            }
        }
    }

    public void TapDown()
    {
        if (isInput)
        {
            tutorialHand.SetActive(false);
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
        particleEmissions.Stop();
        isRun = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        GameManager.Instance.lsLocation[ID].JobComplete(IndexType);
        lever.localEulerAngles = new Vector3(0f, 0f, 180f);
        cart.localPosition = new Vector3(-1.5f, 0f, 0f);
        tree.localPosition = Vector3.zero;
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
    }

    public void Help()
    {
        tutorialHand.SetActive(true);
    }
}
