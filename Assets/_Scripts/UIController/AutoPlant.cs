using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AutoPlant : MonoBehaviour {

    public Text txtInfo;
    public Button btnYes;

    public void AutoPlant_Onclick(string str, UnityAction actionYes)
    {
        txtInfo.text = str;
        btnYes.onClick.AddListener(() =>
        {
            actionYes();
            Destroy(gameObject);
        });
    }

    public void btnNo()
    {
        Destroy(gameObject);
    }
}
