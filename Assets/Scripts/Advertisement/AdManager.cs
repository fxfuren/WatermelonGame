using UnityEngine;
using YG;
using System;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    _instance = singletonObject.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

public class AdManager : Singleton<AdManager>
{

    public string rewardVideoID;

    public override void Awake()
    {
        base.Awake();

        // --- Interstitial (YG2) events ---
        YG2.onOpenInterAdv += OnOpenInter;
        YG2.onCloseInterAdvWasShow += OnCloseInter;
        YG2.onErrorInterAdv += OnErrorInter;

    }


    private void OnDestroy()
    {
        // Unsubscribe Interstitial events
        YG2.onOpenInterAdv -= OnOpenInter;
        YG2.onCloseInterAdvWasShow -= OnCloseInter;
        YG2.onErrorInterAdv -= OnErrorInter;
    }

    // --------------------- Interstitial (YG2) ----------------------

    /// <summary>
    /// Show an interstitial ad using YG2 plugin.
    /// </summary>
    public void ShowAdInterstitial()
    {
        Debug.Log("YG2: Calling InterstitialAdvShow()");
        YG2.InterstitialAdvShow();
    }

    private void OnOpenInter()
    {
        Debug.Log("YG2: Interstitial ad opened");
    }

    private void OnCloseInter(bool wasShown)
    {
        Debug.Log($">> YG2: Interstitial closed; wasShown = {wasShown}");
      //  GameManager.Instance.SceneLoad();
    }

    private void OnErrorInter()
    {
        Debug.LogError("YG2: Error opening interstitial ad");
        // GameManager.Instance.SceneLoad();
    }

    // --------------------- Rewarded (YG2) -------------------------
    /// <summary>
    /// Called by YG2 when a reward is granted (via onRewardAdv event).
    /// </summary>
    private void OnReward(string id)
    {
        if (id == rewardVideoID)
        {
            Debug.Log($"YG2: onRewardAdv fired for id = {id}");
            GiveReward();
        }
    }

    private void GiveReward()
    {
        // TODO: Implement your reward logic, e.g.:
        //GameManager.Instance.AddCoins(1);
    }

    private void OnOpenReward()
    {
        Debug.Log("YG2: Rewarded ad opened");
    }

    private void OnCloseReward()
    {
        Debug.Log("YG2: Rewarded ad closed");
    }

    private void OnErrorReward()
    {
        Debug.LogError("YG2: Error opening rewarded ad");
    }
}
