using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using System.Linq;
using System;

/// <summary>
/// Main class to handle purchases. Stores info about products and communicates with the Unity IAP plug-in.
/// </summary>
/// <remarks>
/// Can be used in any game without changes. Just change products in the inspector-editable list.
/// </remarks>
public class PurchaseManager : MonoBehaviour
{
    /// <summary>
    /// Is set on Awake
    /// </summary>
    public static PurchaseManager Instance;
    /// <summary>
    /// Products that can be purchased in the app
    /// </summary>
    public List<ProductInfo> products;

    private Action Initialized;
    private bool _initialized = false;

    private IAPListener _iapListener; // A class to communicate with the IAP plug-in

    private Action OnPurchaseFailed;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Debug.Log("[PurchaseManager] Start");
        _iapListener = new IAPListener(products, RewriteProductInfoWithStoreInfo);

        _iapListener.ProductPurchased += HandleProductPurchased;
        _iapListener.PurchaseFailed += HandlePurchaseFailed;
    }

    /// <summary>
    /// Returns a product's price from the store, or default price if price from the store was not received
    /// </summary>
    public string GetProductPrice(EProduct product)
    {
        ProductInfo productInfo = products.Where(pi => pi.product == product).FirstOrDefault();

        if (productInfo.storeLocalizedPrice != null)
            return productInfo.storeLocalizedPrice;

        return productInfo.defaultPrice;
    }

    /// <summary>
    /// Returns if this product was purchased. <br></br>
    /// Checks purchase info from store account and player prefs
    /// Only for non-consumable and subscription products.
    /// </summary>
    public bool GetProductPurchased(EProduct product)
    {
        var productKey = ProductInfo.PlayerPrefsKeyByProduct(product);

        if (PlayerPrefs.HasKey("purchase_" + productKey))
            return true;

        ProductInfo productInfo = products.Where(pi => pi.product == product).FirstOrDefault();

        if (productInfo.isPurchased.HasValue)
            return productInfo.isPurchased.Value;

        return false;
    }

    /// <summary>
    /// Stores info about products that was received from the store.
    /// This is kind of the end of the initialization process?
    /// </summary>
    /// <param name="storeBasedProductInfo"></param>
    private void RewriteProductInfoWithStoreInfo(List<ProductInfo> storeBasedProductInfo)
    {
        products = storeBasedProductInfo;

        _initialized = true;
        Initialized?.Invoke();
    }

    /// <summary>
    /// Will be called by IAPlistener when product purchased succesfully. <br></br>
    /// Calls product purchased callback (set in inspector, product info) and stores purchase in player prefs
    /// </summary>
    /// <param name="purchasedId"></param>
    private void HandleProductPurchased(Product product)
    {
        OnPurchaseProcessFinished(false);

        ProductInfo productInfo = products.Where(pi => pi.ProductId.Equals(product.definition.id)).FirstOrDefault();
        productInfo.isPurchased = true;
        productInfo.purchasedCallback?.Invoke(productInfo.product);

        var productKey = ProductInfo.PlayerPrefsKeyByProduct(productInfo.product);
        PlayerPrefs.SetInt("purchase_" + productKey, 1);

        Debug.Log($"[IAP] Product: {product.definition.id}" +
            $"\nReceipt: {product.receipt}");
    }

    /// <summary>
    /// Called when the purchase failed. Shows error dialog
    /// </summary>
    private void HandlePurchaseFailed(Product product)
    {
        Debug.LogWarning("Purchase failed!");
        OnPurchaseProcessFinished(true);
        //PlatformDialog.SetButtonLabel("OK");
        //PlatformDialog.Show(
        //    "Sorry", 
        //    "Error happened. Please try later.", 
        //    PlatformDialog.Type.SubmitOnly, 
        //    () => OnPurchaseProcessFinished(true));
    }

    private void OnPurchaseProcessFinished(bool failed)
    {
        if (failed)
            OnPurchaseFailed?.Invoke();

        OnPurchaseFailed = null;
    }

    /// <summary>
    /// Starts purchase with the IAPListener
    /// </summary>
    /// <param name="product"></param>
    public void BeginPurchase(EProduct product, Action callback = null)
    {
        OnPurchaseFailed = callback;

        ProductInfo productInfo = products.Where(pi => pi.product == product).FirstOrDefault();

        _iapListener.StartProductPurchase(productInfo.ProductId);
    }

    public void RunWhenInitialized(Action action)
    {
        if (_initialized)
            action?.Invoke();
        else
            Initialized += action;
    }
}
