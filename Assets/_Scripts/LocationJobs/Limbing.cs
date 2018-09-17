using UnityEngine;
using DG.Tweening;

public class Limbing : MonoBehaviour
{
    public bool isInput;
    public Transform cart;
    public Transform tree;
    public GameObject notification;

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
            notification.SetActive(false);
            tree.gameObject.SetActive(true);
            ResetMaterial();
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
            AudioManager.Instance.Play("Debarking");
            posDown = Input.mousePosition;
            isRun = true;
        }
    }

    public void TapUp()
    {
        AudioManager.Instance.Stop("Debarking");
        isRun = false;
    }

    public void ResetMaterial()
    {
        cart.localPosition = Vector3.zero;
        tree.localPosition = new Vector3(11f, 0f, 0f);
        tree.DOLocalMove(new Vector3(1f, 0f, 0f), 2f).OnComplete(() => isInput = true);
    }

    public void CompleteJob()
    {
        isRun = false;
        int ID = GameManager.Instance.IDLocation;
        int IndexType = GameManager.Instance.lsLocation[ID].indexType;
        GameManager.Instance.lsLocation[ID].JobComplete(IndexType);
        if (GameManager.Instance.lsLocation[ID].lsWorking[IndexType].input > 0)
        {
            ResetMaterial();
        }
        else
        {
            tree.gameObject.SetActive(false);
            notification.SetActive(true);
        }
    }
}
