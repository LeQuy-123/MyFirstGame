using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    private IKitchenObjectParent kitchenObjectParent;
    public KitchenObjectSO GetkitchenObjectSO () { return kitchenObjectSO;}
    public void SetKitchenObjectParent (IKitchenObjectParent ikitchenObjectParent) {
        this.kitchenObjectParent?.ClearKitchenObject();
        this.kitchenObjectParent = ikitchenObjectParent;
        ikitchenObjectParent.SetKitchenObject(this);
        transform.parent = ikitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }
    public IKitchenObjectParent GetKitchenObjectParent () {
        return kitchenObjectParent;
    }

    public void DestroySelf()
    {
        kitchenObjectParent?.ClearKitchenObject();
        Destroy(gameObject);
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent) {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject  kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
        return kitchenObject;
    }

}
