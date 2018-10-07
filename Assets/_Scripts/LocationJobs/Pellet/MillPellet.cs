using UnityEngine;
using DG.Tweening;

public class MillPellet : MonoBehaviour
{
    public bool isInput;
    public Transform tray;
    public GameObject notification;
    public Transform lever;
    public Transform flour;
    public MeshRenderer scroll;
    public SpriteRenderer imgHand;

    private bool isRun;
    private Vector3 posDown;
    private Vector3 posCheck;

    private float ScrollSpeed = 0.5f;
    private float Offset;
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
                lever.localPosition += new Vector3(0f, dis * 0.018f * Time.deltaTime, 0f);
                flour.localPosition -= new Vector3(dis * 0.015f * Time.deltaTime, 0f, 0f);
                tray.localPosition -= new Vector3(dis * 0.05f * Time.deltaTime, 0f, 0f);
                scroll.material.mainTextureOffset += new Vector2(dis * 0.005f * Time.deltaTime, 0f);
            }
            if (tray.position.x > posCheck.x)
            {
                CompleteJob();
            }
        }
    }

    public void TapDown()
    {
        if (isInput)
        {
            imgHand.sprite = UIManager.Instance.spHand[0];
            AudioManager.Instance.Play("Debarking");
            posDown = Input.mousePosition;
            isRun = true;
        }
    }

    public void TapUp()
    {
        imgHand.sprite = UIManager.Instance.spHand[1];
        AudioManager.Instance.Stop("Debarking");
        isRun = false;
    }

    public void LoadInput()
    {
        tray.localPosition = Vector3.zero;
        flour.localPosition = Vector3.zero;
        isInput = true;
        imgHand.enabled = true;
    }

    public void CompleteJob()
    {
        isRun = false;
        tray.position = posCheck;
        tray.GetChild(0).gameObject.SetActive(true);
        lever.DOLocalMove(new Vector3(2.5f, 1f, 0f), 1.5f).OnComplete(() =>
        {
            tray.GetChild(0).gameObject.SetActive(false);
            int ID = GameManager.Instance.IDLocation;
            int IndexType = GameManager.Instance.lsLocation[ID].indexType;
            GameManager.Instance.lsLocation[ID].JobComplete(IndexType);
            if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
            {
                imgHand.enabled = false;
                isInput = false;
                LoadInput();
            }
            else
            {
                notification.SetActive(true);
            }
        });
    }
}
