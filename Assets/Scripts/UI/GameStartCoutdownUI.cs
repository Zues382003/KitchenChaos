using System;
using TMPro;
using UnityEngine;

public class GameStartCoutdownUI : MonoBehaviour
{
    private const string NUMBER_POPUP = "NumberPopup";
    
    [SerializeField] private TextMeshProUGUI countdownText;
    
    private Animator animator;
    private int previousCountdownNumber;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
        
    }

    private void Update()
    {
        int coutdownNumber = Mathf.CeilToInt(KitchenGameManager.Instance.GetCountdownToStartTimer());
        countdownText.text = coutdownNumber.ToString();

        if (previousCountdownNumber != coutdownNumber)
        {
            previousCountdownNumber = coutdownNumber;
            animator.SetTrigger(NUMBER_POPUP);
            SoundManager.Instance.PlayCountdownSound();
        }
    }
    
    void KitchenGameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountdownToStartActive())
        {
            Show();
        }
        else
        {
            Hide();
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
