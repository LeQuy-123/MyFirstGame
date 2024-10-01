using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenObject : NetworkBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    private IKitchenObjectParent kitchenObjectParent;
    private FollowTransform followTransform;
    protected virtual void Awake() 
    {
        followTransform = GetComponent<FollowTransform>();
    }
    public KitchenObjectSO GetkitchenObjectSO () { return kitchenObjectSO;}
    public void SetKitchenObjectParent (IKitchenObjectParent ikitchenObjectParent) 
    {
        SetKitchenObjectParentServerRpc(ikitchenObjectParent?.GetNetworkObject());
    }
    [ServerRpc(RequireOwnership = false)]
    private void SetKitchenObjectParentServerRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference) 
    {
        SetKitchenObjectParentClientRpc(kitchenObjectParentNetworkObjectReference);

    }
    [ClientRpc]
    private void SetKitchenObjectParentClientRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
        IKitchenObjectParent kitchenObjectParent = kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();
        this.kitchenObjectParent?.ClearKitchenObject();
        this.kitchenObjectParent = kitchenObjectParent;
        if(kitchenObjectParent.HasKitchenObject()) return;
        kitchenObjectParent.SetKitchenObject(this);
        followTransform.SetTargetTransform(kitchenObjectParent.GetKitchenObjectFollowTransform());
    }
    public IKitchenObjectParent GetKitchenObjectParent () {
        return kitchenObjectParent;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
    public void ClearKitchenObjectParent()  
    {
        kitchenObjectParent?.ClearKitchenObject();
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
        
    }
 
    public static void DestroyKitchenObject(KitchenObject kitchenObject)
    {
        KitchenGameMultiplayer.Instance.DestroyKitchenObject(kitchenObject);
    }
}
