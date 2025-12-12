using System;
using TMPro;
using UnityEngine;

public class ToturialUI : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI keyMoveUpText;
   [SerializeField] private TextMeshProUGUI keyMoveDownText;
   [SerializeField] private TextMeshProUGUI keyMoveLeftText;
   [SerializeField] private TextMeshProUGUI keyMoveRightText;
   [SerializeField] private TextMeshProUGUI keyInteractText;
   [SerializeField] private TextMeshProUGUI keyInteractAltText;
   [SerializeField] private TextMeshProUGUI keyPauseText;
   [SerializeField] private TextMeshProUGUI keyGamepadInteractText;
   [SerializeField] private TextMeshProUGUI keyGamepadInteractAltText;
   [SerializeField] private TextMeshProUGUI keyGamepadPauseText;

   private void Start()
   {
      GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;
      KitchenGameManager.Instance.OnStateChanged += KinchenGameManager_OnStateChanged;
      
      UpdateVisual();
      
      Show();

   }

   
   void GameInput_OnBindingRebind(object sender, EventArgs e)
   {
      UpdateVisual();
   }


   private void UpdateVisual()
   {
      keyMoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
      keyMoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
      keyMoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
      keyMoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
      keyInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
      keyInteractAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
      keyPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
      keyGamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact);
      keyGamepadInteractAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_InteractAlternate);
      keyGamepadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause);
      
   }

   private void Show()
   {
      gameObject.SetActive(true);
   }

   private void Hide()
   {
      gameObject.SetActive(false);
   }
   
   void KinchenGameManager_OnStateChanged(object sender, EventArgs e)
   {
      if (KitchenGameManager.Instance.IsCountdownToStartActive())
      {
         Hide();
      }
   }
}
