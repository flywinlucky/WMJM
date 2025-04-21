/// <summary>
/// Extensions for the purchase manager class <br></br>
/// Contains useful methods for quick data access
/// </summary>
public static class PurchasesExtensions 
{
    /// <summary>
    /// Checks if at least one product was purchased
    /// </summary>
     public static bool IsAnyPurchaseDone(this PurchaseManager pm)
     {
        foreach (ProductInfo pi in pm.products)
            if (pi.isPurchased.GetValueOrDefault())
                return true;

        return false;
     }
}
