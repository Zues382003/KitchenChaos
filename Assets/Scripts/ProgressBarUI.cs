using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;
    
    private IHasProgress hasProgress;

    private void Start()
    {
        // hasProgressGameObject.GetComponent<IHasProgress>() RETURN (IHasProgress)cuttingCounterScript
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null)
        {
            Debug.LogError("Object " + hasProgressGameObject + " does not have a component that implements IHasProgress!");
        }
        
        hasProgress.OnProgressChanged +=  HasProgress_OnProgressChanged;
        barImage.fillAmount = 0f;
        
        Hide();
    }
    
    void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.progressNormalized;

        if (e.progressNormalized == 0f || e.progressNormalized == 1f)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
