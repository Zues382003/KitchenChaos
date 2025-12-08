using UnityEngine;

public interface IKitchenObjectParent
{
    public Transform GetKitchenObjectFollowTransform();
    public void SetkitchenObject(KitchenObject kitchenObject);
    public KitchenObject GetKitchenObject();
    public void RemoveKitchenObject();
    public bool HasKitchenObject();
}
