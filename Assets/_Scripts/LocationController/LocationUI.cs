using UnityEngine;
using UnityEngine.UI;

public class LocationUI : MonoBehaviour
{

    public int id;
    public string nameLocationUI;
    public int indexTypeWork;
    public Image imageLocation;

    public void Start()
    {
        imageLocation.alphaHitTestMinimumThreshold = 0.5f;
    }

    public void OnclickLocation()
    {
        UIManager.Instance.scene = TypeScene.LOCATION;
        AudioManager.Instance.Play("Click");
        GameManager.Instance.IDLocation = id;
        GameManager.Instance.LoadLocation();
        UIManager.Instance.worldManager.transform.SetAsFirstSibling();
        if (PlayerPrefs.GetInt("isTutorial") == 0)
        {
            if (!UIManager.Instance.popupTutorial.activeInHierarchy)
            {
                UIManager.Instance.popupTutorial.SetActive(true);
            }
            if (UIManager.Instance.objTutorial != null)
            {
                Destroy(UIManager.Instance.objTutorial);
            }
            UIManager.Instance.ControlHandTutorial(GameManager.Instance.lsLocation[0].lsWorking[0].icon.transform);
            UIManager.Instance.txtWait.text = "Tap to buy workshop";
        }
        if (!UIManager.Instance.locationManager.gameObject.activeInHierarchy)
            UIManager.Instance.locationManager.SetActive(true);

        if (!GameManager.Instance.lsLocation[id].forest.isAutoPlant)
        {
            if (GameManager.Instance.lsLocation[id].forest.tree == 0 
                && !UIManager.Instance.WarningForest.activeInHierarchy)
            {
                UIManager.Instance.WarningForest.SetActive(true);
            }
        }
    }
}
