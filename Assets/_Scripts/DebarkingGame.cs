using UnityEngine;
using DG.Tweening;

public class DebarkingGame : MonoBehaviour
{
    public bool isMaterial;
    public Transform clamp;
    public Transform tree;
    public GameObject _Notification;

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
        int IndexType = GameManager.Instance.lsLocation[ID]._IndexType;
        if (GameManager.Instance.lsLocation[ID]._LsWorking[IndexType]._MaterialReceive > 0)
        {
            tree.gameObject.SetActive(true);
            ResetMaterial();
        }
        else
        {
            tree.gameObject.SetActive(false);
            _Notification.SetActive(true);
        }
    }

    public void Update()
    {
        if (isRun)
        {
            if (Input.mousePosition.y > posDown.y)
            {
                float dis = Input.mousePosition.y - posDown.y;
                clamp.position += new Vector3(0f, dis * 0.01f * Time.deltaTime, 0f);
            }
            if (clamp.position.y > posCheck.y)
            {
                CompleteJob();
            }
        }
    }

    public void TapDown()
    {
        if (isMaterial)
        {
            posDown = Input.mousePosition;
            isRun = true;
        }
        else
        {

        }
    }

    public void TapUp()
    {
        isRun = false;
    }

    public void ResetMaterial()
    {
        clamp.localPosition = Vector3.zero;
        tree.localPosition = new Vector3(11f, 0f, 0f);
        tree.DOLocalMove(new Vector3(1f, 0f, 0f), 2f).OnComplete(() => isMaterial = true);
    }

    public void CompleteJob()
    {
        isRun = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID]._IndexType;
        GameManager.Instance.lsLocation[ID].DebarkingComplete();
        if (GameManager.Instance.lsLocation[ID]._LsWorking[IndexType]._MaterialReceive > 0)
        {
            ResetMaterial();
        }
        else
        {
            tree.gameObject.SetActive(false);
            _Notification.SetActive(true);
        }
    }
}
