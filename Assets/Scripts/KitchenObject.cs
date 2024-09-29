using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenObject : NetworkBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    private IKitchenObjectParent kitchenObjectParent;
    public KitchenObjectSO GetkitchenObjectSO () { return kitchenObjectSO;}
    public void SetKitchenObjectParent (IKitchenObjectParent ikitchenObjectParent) {
        this.kitchenObjectParent?.ClearKitchenObject();
        this.kitchenObjectParent = ikitchenObjectParent;
        ikitchenObjectParent.SetKitchenObject(this);
        //this is for putting the object to the correct place (on counter, on player, on plate... etc)
        // transform.parent = ikitchenObjectParent.GetKitchenObjectFollowTransform();
        // transform.localPosition = Vector3.zero;
    }
    public IKitchenObjectParent GetKitchenObjectParent () {
        return kitchenObjectParent;
    }

    public void DestroySelf()
    {
        kitchenObjectParent?.ClearKitchenObject();
        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject) 
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        plateKitchenObject = null;
        return false;
    }

    public static void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent) {
        KitchenGameMultiplayer.Instance.SpawnKitchenObject(kitchenObjectSO, kitchenObjectParent);
        // Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        // KitchenObject  kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        // kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
        // return kitchenObject;
    }

}
