using UnityEngine.Purchasing;
using UnityEngine.Events;

/// <summary>
/// All possible products in this app for easy inspector setting and method calling<br></br>
/// Add all products to this enum
/// </summary>
[System.Serializable]
public enum EProduct
{
    Unknown,
    RemoveAds,
}

/// <summary>
/// Class to hold info about a product
/// </summary>
[System.Serializable]
public class ProductInfo
{
    /// <summary>
    /// Get string product id in the store
    /// </summary>
    public static string ProductIdByProduct(EProduct productType)
    {
        return productType switch
        {
            EProduct.RemoveAds => "com.oriplay.zoofruitymerge.remove_ads",
            _ => "unknown",
        };
    }

    /// <summary>
    /// This key will be used to store purchase in player prefs. 
    /// </summary>
    /// <remarks>
    /// Storing in player prefs can be used for non-consumable purchase in case if there is no internet connection.
    /// For example, if the user unlocked a new game mode, they should have acces to it even without internet
    /// </remarks>
    public static string PlayerPrefsKeyByProduct(EProduct productType)
    {
        return productType switch
        {
            EProduct.RemoveAds => "com.oriplay.zoofruitymerge.remove_ads",
            _ => "unknown",
        };
    }

    /// <summary>
    /// What product this represents
    /// </summary>
    public EProduct product;
    /// <summary>
    /// What product type does this product have (consumable, non-cons., subscription)
    /// </summary>
    public ProductType productType;
    /// <summary>
    /// Price to display on buttons with this product if could not retrieve price data from the store
    /// </summary>
    public string defaultPrice;
    /// <summary>
    /// Action to call when the product is purchased, set in the inspector
    /// </summary>
    public UnityEvent<EProduct> purchasedCallback;

    /// <summary>
    /// Price of this product in the store
    /// </summary>
    [UnityEngine.HideInInspector]
    public string storeLocalizedPrice;
    /// <summary>
    /// Was this product purchased? Only for non-consumable and subscription products
    /// </summary>
    [UnityEngine.HideInInspector]
    public bool? isPurchased;

    /// <summary>
    /// String id of product in the store
    /// </summary>
    public string ProductId 
    {
        get
        {
            return ProductIdByProduct(product);
        }
    }

}
