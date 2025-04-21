using UnityEngine;
using UnityEngine.UI;

public class IAPBuyButtonHandler : MonoBehaviour
{
    [SerializeField] private EProduct _product;
    [SerializeField] private TMPro.TMP_Text _priceText;

    private bool _isInitialized = false;

    private void OnEnable()
    {
        if (_isInitialized)
            return;

        // set price
        if (_priceText != null)
            _priceText.text = PurchaseManager.Instance.GetProductPrice(_product);

        _isInitialized = true;
    }

    public void BeginPurchase()
    {
        PurchaseManager.Instance.BeginPurchase(_product);
    }
}
