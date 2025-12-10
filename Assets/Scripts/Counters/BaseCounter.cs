using System;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnAnyObjectPlaceHere;
    
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject; 
    
    public virtual void Interact(Player player)
    {
        Debug.Log("BaseCounter Interact");
    }
    
    public virtual void InteractAlternate(Player player)
    {
        Debug.Log("BaseCounter InteractAlternate");
    }
    
    public Transform GetKitchenObjectFollowTransform() => counterTopPoint;

    public void SetkitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnAnyObjectPlaceHere?.Invoke(this, EventArgs.Empty);
        }
    }
    public KitchenObject GetKitchenObject() => kitchenObject;
    public void RemoveKitchenObject() => kitchenObject = null;
    public bool HasKitchenObject() => kitchenObject != null;
    
    
}
