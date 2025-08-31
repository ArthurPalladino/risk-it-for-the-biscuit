using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetButtonsHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] Sprite originalSprite;
    [SerializeField] Sprite hoverSprite;
    [SerializeField] Image buttonImage;

    [SerializeField] AudioClip clickSound;
    [SerializeField] Button self;
    void Start()
    {
        self = GetComponent<Button>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position, 0.3f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (self.interactable) buttonImage.sprite = hoverSprite;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.sprite = originalSprite;
    }
}
