using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics; 
using DeltaDNA; 
public class transaction_tutorial : MonoBehaviour
{

    // Building on Unity IAP Tutorial
    // https://learn.unity.com/tutorial/unity-iap

    // Unsing Store setup info from
    // https://docs.unity3d.com/Manual/UnityIAP.html


    public bool disableUnityAnalytics = false; 

    // Start is called before the first frame update
    void Start()
    {

        if (disableUnityAnalytics)
        {
            Analytics.initializeOnStartup = false;
            Analytics.enabled = false;
            PerformanceReporting.enabled = false;
            Analytics.limitUserTracking = true;
            Analytics.deviceStatsEnabled = false;

        }

        DDNA.Instance.SetLoggingLevel(DeltaDNA.Logger.Level.DEBUG); 
        DDNA.Instance.StartSDK();


    }

}
