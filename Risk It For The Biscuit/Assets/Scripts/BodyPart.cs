using UnityEngine;
using UnityEngine.UI;

public class BodyPart : MonoBehaviour
{
    [SerializeField] public Image image;
    [SerializeField] public ParticleSystem blood;


    private void Awake()
    {
        blood.Stop();
    }
    
    public void Cut()
    {
        blood.Play();
        image.gameObject.SetActive(false);
    }

    public void Dismiss()
    {
        blood.Stop();
    }

    public void Restore()
    {
        image.gameObject.SetActive(true);
    }
    
}
