using UnityEngine;

/// <summary>
/// This script contains methods that will be called for each product when it is purchased.
/// </summary>
/// <remarks>
/// Put this on the same object as PurchaseManager, and add this methods to products list in it
/// </remarks>
public class PurchaseDoneMethods : MonoBehaviour
{
    // Call this in every purchase method
    private void AnyPurchaseDone()
    {
        Debug.Log("Purchase done!");
        //PlatformDialog.SetButtonLabel("OK");
        //PlatformDialog.Show("Great!", "Thanks for your support!", PlatformDialog.Type.SubmitOnly, () => { });
    }

    /// <summary>
    /// Only displays "Thanks" dialog
    /// </summary>
    public void CommonPurchaseDone()
    {
        AnyPurchaseDone();
    }
}
