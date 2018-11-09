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
            UIManager.Instance.txtWait.text = "Tap to plant trees\nYou will need to reforest periodically";
        }
        if (!UIManager.Instance.locationManager.gameObject.activeInHierarchy)
            UIManager.Instance.locationManager.SetActive(true);
    }
}
