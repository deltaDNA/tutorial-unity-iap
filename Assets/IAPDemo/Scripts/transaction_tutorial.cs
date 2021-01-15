using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics; 
using DeltaDNA; 


// A couple of little classes to simplify transaction receipt parsing from JSON.
[System.Serializable]
public class TransactionReceipt
{
    public string Store;
    public string TransactionID;
    public string Payload;
}
[System.Serializable]
public class GooglePlayReceipt
{
    public string json;
    public string signature;
}



public class transaction_tutorial : MonoBehaviour
{

    // Building on Unity IAP Tutorial
    // https://learn.unity.com/tutorial/unity-iap

    // Unsing Store setup info from
    // https://docs.unity3d.com/Manual/UnityIAP.html

    // To populate a deltaDNA transaction event
    // https://github.com/deltaDNA/unity-sdk#tracking-revenue

    public bool disableUnityAnalytics = false; 

    // Start is called before the first frame update
    void Start()
    {
        // Disabling Unity Analytics for this tutorial.
        // You wouldn't normally want to do this.
        if (disableUnityAnalytics)
        {
            DisableUnityAnalytics();
        }

        // Start the deltaDNA SDK, if it isn't already running
        if (!DDNA.Instance.HasStarted)
        {
            DDNA.Instance.SetLoggingLevel(DeltaDNA.Logger.Level.DEBUG);
            DDNA.Instance.StartSDK();
        }


    }

    public void RecordIapTransaction(UnityEngine.Purchasing.Product iap)
    {                     
        // Check that we have a revenue amount to record.
        if (!string.IsNullOrEmpty(iap.metadata.isoCurrencyCode)  && iap.metadata.localizedPrice > 0 )
        {
            // Add the Cost of the IAP to a productsSpent object
            var productsSpent = new DeltaDNA.Product()
                .SetRealCurrency(
                    iap.metadata.isoCurrencyCode, 
                    Product.ConvertCurrency(iap.metadata.isoCurrencyCode,iap.metadata.localizedPrice
                    )
            );

            // Add the items or currency received to a productsReceived object
            // We have cheated by hardwiring this for this tutorial!
            // Note that the virtualCurrencyType should be one of these 3 values ["PREMIUM","PREMIUM_GRIND","GRIND"]
            var productsReceived = new DeltaDNA.Product()
                .AddVirtualCurrency("Coins", "PREMIUM", 100);

            // Create a transaction using the products spent and received objects 
            // Note that the transaction type should always be "PURCHASE" for IAPs
            var transactionEvent = new Transaction(
                iap.metadata.localizedTitle,
                "PURCHASE",
                productsReceived,
                productsSpent)
                .SetTransactionId(iap.transactionID)
                .AddParam("productID",iap.definition.id);
            
            // Add the transaction receipt if we have one.
            if(iap.hasReceipt)
            {
                // Populate transaction receipt fields dependig on type of receipt
                // https://github.com/deltaDNA/unity-sdk#tracking-revenue
                // https://docs.unity3d.com/Manual/UnityIAPPurchaseReceipts.html 
                
                TransactionReceipt transactionReceipt = new TransactionReceipt();
                transactionReceipt = JsonUtility.FromJson<TransactionReceipt>(iap.receipt);

                if(transactionReceipt.Store == "AppleAppStore")
                {
                    transactionEvent.SetServer("APPLE");
                    transactionEvent.SetReceipt(transactionReceipt.Payload.ToString());
                }
                else if (transactionReceipt.Store == "GooglePlay")
                {
                    GooglePlayReceipt googleReceipt = new GooglePlayReceipt();
                    googleReceipt = JsonUtility.FromJson<GooglePlayReceipt>(transactionReceipt.Payload);

                    transactionEvent.SetServer("GOOGLE");
                    transactionEvent.SetReceipt(googleReceipt.json);
                    transactionEvent.SetReceiptSignature(googleReceipt.signature);
                }             
            }

            // Record the transaction event
            DDNA.Instance.RecordEvent(transactionEvent);
            Debug.Log(string.Format("Sent IAP transaction event to DDNA : {0}", iap.definition.id));
        }
    }


    public void DisableUnityAnalytics()
    {
        // Disabling Unity Analytics for this tutorial.
        // You wouldn't normally want to do this.

        Analytics.initializeOnStartup = false;
        Analytics.enabled = false;
        PerformanceReporting.enabled = false;
        Analytics.limitUserTracking = true;
        Analytics.deviceStatsEnabled = false;
    }
}
