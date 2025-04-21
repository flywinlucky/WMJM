using System;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

/// <summary>
/// Implements IStoreListener interface and connects with UnityEngine.Purchasing library
/// Subscribe to events to handle purchasing
/// </summary>
public class IAPListener : IDetailedStoreListener
{
    #region Events

    /// <summary>
    /// Is called when the store listener is initialized. Subscribe to handle info about products price and status
    /// </summary>
    public Action<List<ProductInfo>> ProductInfoReceived;

    /// <summary>
    /// Is called when the product was succesfully purchased. Subscribe to handle product purchase
    /// </summary>
    public Action<Product> ProductPurchased;

    /// <summary>
    /// Is called when purchase was failed. Subscribe to unpause game (resume sound, etc)
    /// </summary>
    public Action<Product> PurchaseFailed;

    #endregion Events

    #region private variables

    // These are set by the Unity purchasing library in OnInitalize
    // And used to call functions from the library
    private IStoreController controller;

    private IExtensionProvider extensions;

    private List<ProductInfo> _initialProductInfo;    // Info about products that is set in the editor (ids, default price)
    private List<ProductInfo> _storeBasedProductInfo; // Info about products with their prices from the store and buy status

    #endregion private variables

    #region return value by parameter methods

    /// <summary>
    /// Checks if this product was purchased (must be called after initialization is finished)
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    private bool IsProductPurchased(string productId)
    {
        bool itemBought = false;
        UnityEngine.Purchasing.Product product = controller.products.WithID(productId);
        if (product != null && product.hasReceipt)
        {
            // Owned Non Consumables and Subscriptions should always have receipts.
            // So here the Non Consumable product has already been bought.
            itemBought = true;
        }

        return itemBought;
    }

    /// <summary>
    /// Gets product price from the store. Should be called when initalization is finished
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    private string GetLocalizedPriceString(string productId)
    {
        return controller.products.WithID(productId).metadata.localizedPriceString;
    }

    #endregion return value by parameter methods

    #region Create objects helpers

    /// <summary>
    /// Creates configuration builder with product ids for the game
    /// </summary>
    /// <param name="products"></param>
    /// <returns></returns>
    private ConfigurationBuilder CreateConfigurationBuilder(List<ProductInfo> products)
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        foreach (ProductInfo pi in products)
            builder.AddProduct(pi.ProductId, pi.productType);

        return builder;
    }

    /// <summary>
    /// Creates product info with price and buy status recieved from the store
    /// </summary>
    /// <returns></returns>
    private List<ProductInfo> CreateStoreBasedProductInfo()
    {
        List<ProductInfo> ret = new List<ProductInfo>();
        foreach (ProductInfo pi in _initialProductInfo)
        {
            ProductInfo pi2 = pi;
            pi2.storeLocalizedPrice = GetLocalizedPriceString(pi2.ProductId);
            pi2.isPurchased = IsProductPurchased(pi2.ProductId);

            ret.Add(pi2);
        }

        return ret;
    }

    #endregion Create objects helpers

    #region Constructor and initialization

    /// <summary>
    ///
    /// </summary>
    /// <param name="products">Products with ids and type</param>
    public IAPListener(List<ProductInfo> products, Action<List<ProductInfo>> productInfoReceived)
    {
        Debug.Log("[IAPListener] ctor");

        ProductInfoReceived += productInfoReceived;

        _initialProductInfo = products;
        InitializeServices(() =>
        {
            var builder = CreateConfigurationBuilder(products);
            UnityPurchasing.Initialize(this, builder);
        });
    }

    private async void InitializeServices(Action completeCallback)
    {
        await UnityServices.InitializeAsync(new InitializationOptions());

        completeCallback?.Invoke();
    }

    /// <summary>
    /// Called when Unity IAP is ready to make purchases.
    /// </summary>
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("[IAPListener] OnInitialized");

        this.controller = controller;
        this.extensions = extensions;

        _storeBasedProductInfo = CreateStoreBasedProductInfo();

        ProductInfoReceived?.Invoke(_storeBasedProductInfo);
    }

    /// <summary>
    /// Called when Unity IAP encounters an unrecoverable initialization error.
    ///
    /// Note that this will not be called if Internet is unavailable; Unity IAP
    /// will attempt initialization until it becomes available.
    /// </summary>
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("Failed to initialize IAP: " + error.ToString());
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log("Failed to initialize IAP2: " + error.ToString() + "\n" + message);
    }

    #endregion Constructor and initialization

    #region Control purchasing

    /// <summary>
    /// Call this to begin purchasing
    /// </summary>
    /// <param name="productId"></param>
    public void StartProductPurchase(string productId)
    {
        controller.InitiatePurchase(productId);
    }

    /// <summary>
    /// Called when a purchase completes.
    /// On Android, is called after every initialization for non-consumable and subscription products already purchased
    /// May be called at any time after OnInitialized().
    /// </summary>
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        // Retrieve the purchased product
        var product = e.purchasedProduct;

        Debug.Log($"Purchase Complete - Product: {product.definition.id}");

        ProductPurchased?.Invoke(product);

        // We return Complete, informing IAP that the processing on our side is done and the transaction can be closed.
        return PurchaseProcessingResult.Complete;
    }

    /// <summary>
    /// Called when a purchase fails.
    /// </summary>
    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        Debug.LogWarning($"Purchase failed - Product: '{i.definition.id}', PurchaseFailureReason: {p}");
        PurchaseFailed?.Invoke(i);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.LogWarning($"Purchase failed - Product: '{product.definition.id}', " +
            $"PurchaseFailureDescriptionMessage: {failureDescription.message}");
        PurchaseFailed?.Invoke(product);
    }

    #endregion Control purchasing
}