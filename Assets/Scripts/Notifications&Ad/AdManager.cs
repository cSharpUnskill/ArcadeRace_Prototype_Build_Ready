using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Cars
{
    public class AdManager : MonoBehaviour, IUnityAdsListener
    {
        [SerializeField]
        private string _androidGameId;
        [SerializeField]
        private string _iOSGameId;
        [SerializeField]
        private string _androidInterstitialAdId;
        [SerializeField]
        private string _iOSInterstitialAdId;
        [SerializeField]
        private string _androidRewardedId;
        [SerializeField]
        private string _iOSRewardedId;
        [SerializeField]
        private string _androidBannerId;
        [SerializeField]
        private string _iOSBannerId;
        [SerializeField]
        private bool _testMode;

        private string GameId => Application.platform == RuntimePlatform.IPhonePlayer ? _iOSGameId : _androidGameId;
        private string InterstitialId => Application.platform == RuntimePlatform.IPhonePlayer ? _iOSInterstitialAdId : _androidInterstitialAdId;
        private string RewardedId => Application.platform == RuntimePlatform.IPhonePlayer ? _iOSRewardedId : _androidRewardedId;
        private string BannerId => Application.platform == RuntimePlatform.IPhonePlayer ? _iOSBannerId : _androidBannerId;

        private void Awake()
        {
            Advertisement.Initialize(GameId, _testMode, true);
            Advertisement.AddListener(this);
            StartCoroutine(WaitForInit());
        }

        private IEnumerator WaitForInit()
        {
            yield return new WaitUntil(() => Advertisement.isInitialized);
            StartBannerAd();
        }

        private void StartBannerAd(BannerPosition position = BannerPosition.BOTTOM_CENTER)
        {
            if (!Advertisement.isInitialized) return;

            Advertisement.Banner.SetPosition(position);
            Advertisement.Banner.Load(BannerId, new BannerLoadOptions()
            {
                errorCallback = OnBannerLoadError,
                loadCallback = OnBannerLoad
            });
        }

        private static void OnBannerLoadError(string message)
        {
            Debug.LogError("Ad banner error " + message);
        }

        private void OnBannerLoad()
        {
            Advertisement.Banner.Show();
        }

        public void StopBanner()
        {
            Advertisement.Banner.Hide();
        }

        private void StartInterstitialAd()
        {
            if (!Advertisement.isInitialized) return;

            Advertisement.Load(InterstitialId);
        }

        private void StartRewardedAd()
        {
            if (!Advertisement.isInitialized) return;

            Advertisement.Load(RewardedId);
        }

        public void OnUnityAdsReady(string placementId)
        {
            Advertisement.Show(placementId);
        }

        public void OnUnityAdsDidError(string message)
        {
            Debug.LogError("Ad error " + message);
        }

        public void OnUnityAdsDidStart(string placementId)
        {
        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            if (placementId == RewardedId && showResult == ShowResult.Finished)
            {
                //todo reward
            }
        }
    }
}