using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonScaleAuto : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;
    private float x, y;
    void Start()
    {
        button = GetComponent<Button>();
        x = this.transform.localScale.x;
        y = this.transform.localScale.y;
    }

    public void PointerEnter()
    {
        if (button.interactable)
        {
            this.transform.SetAsLastSibling();
            this.transform.localScale = new Vector3(x + 0.1f, y + 0.1f, 1);
        }
    }

    public void PointerExit()
    {
        this.transform.localScale = new Vector3(x, y, 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExit();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnter();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            PointerExit();
        }
    }
}
