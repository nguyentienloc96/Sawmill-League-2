using DG.Tweening;
using UnityEngine;

public class ChipperPellet : MonoBehaviour
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
    public GameObject tutorialHand;

    private bool isRun;
    private Vector3 posDown;
    private Vector3 posCheck;
    private bool time;
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
                cart.position -= new Vector3(0f, dis *0.01f* Time.deltaTime, 0f);
                gear.localEulerAngles += new Vector3(0f, 0f, -dis * 5f * Time.deltaTime);
                gear1.localEulerAngles -= new Vector3(0f, 0f, dis * 5f * Time.deltaTime);
                gear2.localEulerAngles -= new Vector3(0f, 0f, dis * 2.5f * Time.deltaTime);
            }
            if (gear2.localEulerAngles.z > 150f)
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
        gear1.DOLocalRotate(gear2.localEulerAngles + new Vector3(0f, 0f, 180f), 1.5f).OnComplete(() =>
        {
            particleLimbing.Stop();
            animFlour.Rebind();
            animFlour.enabled = false;
            gear2.localEulerAngles = Vector3.zero;
            GameManager.Instance.lsLocation[ID].JobComplete(IndexType);
            cart.localPosition = new Vector3(-4f, 0f, 0f);
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
    }

    public void Help()
    {
        tutorialHand.SetActive(true);
    }
}
