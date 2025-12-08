using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
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
    public void SetkitchenObject(KitchenObject kitchenObject) => this.kitchenObject = kitchenObject;
    public KitchenObject GetKitchenObject() => kitchenObject;
    public void RemoveKitchenObject() => kitchenObject = null;
    public bool HasKitchenObject() => kitchenObject != null;
    
    
}
