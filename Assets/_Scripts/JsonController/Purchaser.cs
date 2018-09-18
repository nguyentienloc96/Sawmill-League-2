﻿using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.storage;
using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class Purchaser : MonoBehaviour, IStoreListener
{
    private static IStoreController m_StoreController;          
    private static IExtensionProvider m_StoreExtensionProvider;

    // Apple App Store-specific product identifier for the subscription product.
    private static string kProductNameAppleSubscription = "com.unity3d.subscription.new";

    // Google Play Store-specific product identifier subscription product.
    private static string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";

    void Start()
    {
        // If we haven't set up the Unity Purchasing reference
        if (m_StoreController == null)
        {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        // If we have already connected to Purchasing ...
        if (IsInitialized())
        {
            // ... we are done here.
            return;
        }

        //var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        //builder.AddProduct(GameConfig.Instance.kProductID50, ProductType.Consumable);
        //builder.AddProduct(GameConfig.Instance.kProductID300, ProductType.Consumable);
        //builder.AddProduct(GameConfig.Instance.kProductID5000, ProductType.Consumable);
        //UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        // Only say we are initialized if both the Purchasing references are set.
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    public void Buy50()
    {
        BuyProductID(GameConfig.Instance.kProductID50);
    }
    public void Buy300()
    {
        BuyProductID(GameConfig.Instance.kProductID300);
    }
    public void Buy5000()
    {
        BuyProductID(GameConfig.Instance.kProductID5000);
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            //Mng.mng.ui.loading.SetActive(true);

            // ... look up the Product reference with the general product identifier and the Purchasing 
            // system's products collection.
            Product product = m_StoreController.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                m_StoreController.InitiatePurchase(product);
            }
            // Otherwise ...
            else
            {
                // ... report the product look-up failure situation  
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        // Otherwise ...
        else
        {
            //Mng.mng.ui.loading.SetActive(false);

            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }




    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        Debug.Log("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        // A consumable product has been purchased by this user.
        if (String.Equals(args.purchasedProduct.definition.id, GameConfig.Instance.kProductID50, StringComparison.Ordinal))
        {
            PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold", 10) + 50);
            if (PlayerPrefs.GetInt("Gold", 10) > 50 && Mathf.Abs(PlayerPrefs.GetInt("GoldPre", 0) - PlayerPrefs.GetInt("Gold", 10)) >= 50)
            {
                PlayerPrefs.GetInt("GoldPre", PlayerPrefs.GetInt("Gold", 10));
                StorageService storageService = App42API.BuildStorageService();
                storageService.UpdateDocumentByKeyValue("Db", "Data", "id", GameConfig.id, JsonUtility.ToJson(new SaveGold(GameConfig.id, PlayerPrefs.GetInt("Gold", 10))), new UnityCallBack2());
            }
            //Mng.mng.ui.gold.text = Mng.mng.ui.SetNumberString(PlayerPrefs.GetInt("Gold", 10));
            //Mng.mng.ui.loading.SetActive(false);
            if (PlayerPrefs.GetInt("NoAds") == 0)
                PlayerPrefs.SetInt("NoAds", 1);
        }
        // Or ... a non-consumable product has been purchased by this user.
        else if (String.Equals(args.purchasedProduct.definition.id, GameConfig.Instance.kProductID300, StringComparison.Ordinal))
        {
            PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold", 10) + 300);
            if (PlayerPrefs.GetInt("Gold", 10) > 50 && Mathf.Abs(PlayerPrefs.GetInt("GoldPre", 0) - PlayerPrefs.GetInt("Gold", 10)) >= 50)
            {
                PlayerPrefs.GetInt("GoldPre", PlayerPrefs.GetInt("Gold", 10));
                StorageService storageService = App42API.BuildStorageService();
                storageService.UpdateDocumentByKeyValue("Db", "Data", "id", GameConfig.id, JsonUtility.ToJson(new SaveGold(GameConfig.id, PlayerPrefs.GetInt("Gold", 10))), new UnityCallBack2());
            }
            //Mng.mng.ui.gold.text = Mng.mng.ui.SetNumberString(StarPrefs.GetInt("Gold", 10));
            //Mng.mng.ui.loading.SetActive(false);
            if (PlayerPrefs.GetInt("NoAds") == 0)
                PlayerPrefs.SetInt("NoAds", 1);
        }
        // Or ... a subscription product has been purchased by this user.
        else if (String.Equals(args.purchasedProduct.definition.id, GameConfig.Instance.kProductID5000, StringComparison.Ordinal))
        {
            PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold", 10) + 5000);
            if (PlayerPrefs.GetInt("Gold", 10) > 50 && Mathf.Abs(PlayerPrefs.GetInt("GoldPre", 0) - PlayerPrefs.GetInt("Gold", 10)) >= 50)
            {
                PlayerPrefs.GetInt("GoldPre", PlayerPrefs.GetInt("Gold", 10));
                StorageService storageService = App42API.BuildStorageService();
                storageService.UpdateDocumentByKeyValue("Db", "Data", "id", GameConfig.id, JsonUtility.ToJson(new SaveGold(GameConfig.id, PlayerPrefs.GetInt("Gold", 10))), new UnityCallBack2());
            }
            //Mng.mng.ui.gold.text = Mng.mng.ui.SetNumberString(StarPrefs.GetInt("Gold", 10));
            if (PlayerPrefs.GetInt("NoAds") == 0)
                PlayerPrefs.SetInt("NoAds", 1);
        }
        // Or ... an unknown product has been purchased by this user. Fill in additional products here....
        else
        {
            //Mng.mng.ui.loading.SetActive(false);
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 
        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        //Mng.mng.ui.loading.SetActive(false);
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}