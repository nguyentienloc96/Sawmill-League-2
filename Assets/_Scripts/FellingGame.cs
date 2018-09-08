using UnityEngine;

public class FellingGame : MonoBehaviour
{

    public Animator _Anim;
    public Animator _AnimTrunk;
    public GameObject _ObjTrunk;
    public Transform _MaskFelling;
    public GameObject _Notification;

    public void OnEnable()
    {
        int ID = GameManager.Instance.IDLocation;
        if (GameManager.Instance.lsLocation[ID]._Forest._Tree > 0)
        {
            _ObjTrunk.SetActive(true);
            _Notification.SetActive(false);
        }
        else
        {
            _ObjTrunk.SetActive(false);
            _Notification.SetActive(true);
        }
    }

    public void FellingTree()
    {
        int ID = GameManager.Instance.IDLocation;
        if (GameManager.Instance.lsLocation[ID]._Forest._Tree > 0)
        {
            _Anim.SetBool("isFelling", true);
        }
    }

    public void StopFelling()
    {
        int ID = GameManager.Instance.IDLocation;
        if (GameManager.Instance.lsLocation[ID]._Forest._Tree > 0)
        {
            _Anim.SetBool("isFelling", false);
            _MaskFelling.localScale += new Vector3(0.5f, 0f, 0f);
            if (_MaskFelling.localScale.x >= 2.5f)
            {
                _AnimTrunk.SetBool("isFall", true);
                Invoke("ResetTree", 0.5f);
            }
        }
        else
        {
            _Anim.SetBool("isFelling", false);
            _ObjTrunk.SetActive(false);
            _MaskFelling.localScale = new Vector3(0f, 2f, 1f);
            _AnimTrunk.SetBool("isFall", false);
            _Notification.SetActive(true);
        }
    }

    public void ResetTree()
    {
        GameManager.Instance.lsLocation[GameManager.Instance.IDLocation].FellingComplete();
        if(GameManager.Instance.lsLocation[GameManager.Instance.IDLocation]._Forest._Tree > 0)
        {
            _MaskFelling.localScale = new Vector3(0f, 2f, 1f);
            _AnimTrunk.SetBool("isFall", false);
        }
    }
}
