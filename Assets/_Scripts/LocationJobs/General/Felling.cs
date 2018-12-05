using UnityEngine;
using UnityEngine.UI;

public class Felling : MonoBehaviour
{

    public Animator anim;
    public GameObject notification;

    public GameObject tutorialHand;
    private bool isWaiting;

    public Transform tfStart;
    public Transform tfEnd;
    public Sprite iconOutPut;

    public void OnEnable()
    {
        if (isWaiting)
        {
            isWaiting = false;
            GameManager.Instance.lsLocation[GameManager.Instance.IDLocation].FellingComplete();
        }
        int ID = GameManager.Instance.IDLocation;
        if (GameManager.Instance.lsLocation[ID].forest.tree > 0)
        {
            if(PlayerPrefs.GetInt("isTutorial") != 0)
            {
                tutorialHand.SetActive(true);
            }
            notification.SetActive(false);
        }
        else
        {
            notification.SetActive(true);
        }
    }

    public void FellingTree()
    {
        if (!isWaiting)
        {
            int ID = GameManager.Instance.IDLocation;
            if (GameManager.Instance.lsLocation[ID].forest.tree > 0)
            {
                AudioManager.Instance.PlayOneShot("Saw");
                anim.SetBool("isFelling", true);
                isWaiting = true;
            }
        }
    }

    public void StopFelling()
    {
        if (isWaiting)
        {
            int ID = GameManager.Instance.IDLocation;
            double valueOutput = GameManager.Instance.lsLocation[GameManager.Instance.IDLocation].FellingComplete();
            GameManager.Instance.AddOutPut(valueOutput, iconOutPut, tfStart.position, tfEnd.position);
            tutorialHand.SetActive(false);
            if (GameManager.Instance.lsLocation[ID].forest.tree <= 0)
            {
                notification.SetActive(true);
            }
            AudioManager.Instance.Stop("Saw");
            anim.SetBool("isFelling", false);
            isWaiting = false;
            if (PlayerPrefs.GetInt("isTutorial") == 0)
            {
                UIManager.Instance.btnFellingTutorial.gameObject.SetActive(true);
                if (UIManager.Instance.objTutorial != null)
                {
                    Destroy(UIManager.Instance.objTutorial);
                }
                UIManager.Instance.ControlHandTutorial(UIManager.Instance.btnCloseFellingTutorial);
                UIManager.Instance.txtWait.text = "Tap to leave the workshop";
            }
        }
    }

    public void Help()
    {
        tutorialHand.SetActive(true);
    }

    private void Update()
    {
        int ID = GameManager.Instance.IDLocation;
        if (GameManager.Instance.lsLocation[ID].forest.tree > 0)
        {
            notification.SetActive(false);
        }
        else
        {
            notification.SetActive(true);
        }
    }

    public void FellingTreeTutorial()
    {
        if (PlayerPrefs.GetInt("isTutorial") == 0)
        {
            FellingTree();
            UIManager.Instance.txtWait.text = "wait to fell a tree";
        }
    }

    public void CloseFelling()
    {
        if (PlayerPrefs.GetInt("isTutorial") == 0)
        {
            UIManager.Instance.isOnClickTrunk = true;
            UIManager.Instance.popupTutorial.SetActive(false);
            UIManager.Instance.txtWait.text = "Wait to Tap the Truck";
        }
    }
}
