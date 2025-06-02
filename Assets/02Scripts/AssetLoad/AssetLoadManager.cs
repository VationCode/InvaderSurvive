// ���� : �������� �ٿ�ε� -> �޸� ���ε� -> �� ���ε�

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DUS.AssetLoad
{
    public class AssetLoadManager : MonoBehaviour, IAssetLoadService
    {
        public static AssetLoadManager Instance { get; private set; }
        public static IAssetLoadService assetLoadService => Instance;

        // �ٿ� �� �޸� ���ε� �ͷ�� Ű���� ����
        public List<string> m_downLoadedAddressables { get; private set; } = new List<string>();

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        public async Task InitializeAsync()
        {
            try
            {
                await Addressables.InitializeAsync().Task;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[Addressables] �ʱ�ȭ ���� - �����ϰ� �����մϴ�: {e.Message}");
            }
        }

        // TODO : ���� DownloadAssets�� LoadAssetsIntoMemoryAsync�� �ߺ� �����丵
        int currentProgress = 0;
        public async Task DownLoadAndSceneUpload(Action<float> onProgress, NextSceneRequireData nextSceneRequireData = null)
        {
            if (nextSceneRequireData == null) return;
            currentProgress = 0;
            m_downLoadedAddressables.Clear(); // �޸𸮿� �ε�� ���� ����Ʈ �ʱ�ȭ

            List<string> _labelUnityKeyList = nextSceneRequireData.m_RequiredAddressableLabelKeyList;
            List<string> _keyList = nextSceneRequireData.m_RequiredAddressableKeyList;

            int _total = _labelUnityKeyList.Count + _keyList.Count;

            // �� ����
            if(_labelUnityKeyList.Count > 0)
            {
                await DownloadAssets(_total, currentProgress, onProgress, _labelUnityKeyList);               // �������� �ٿ�ε�
                await LoadAssetsIntoMemoryAsync(_total, currentProgress, onProgress, _labelUnityKeyList);     // �ٿ���� ���� �޸𸮿� ���ε�
            }

            if(_keyList.Count > 0)
            {
                await DownloadAssets(_total, currentProgress, onProgress, _keyList);
                await LoadAssetsIntoMemoryAsync(_total, currentProgress, onProgress, _keyList);
            }

            onProgress?.Invoke(1f); // ������ 1�� ä����

            // ���� ���� ����
            await CreateAddressableAsset(m_downLoadedAddressables);
        }

        private async Task DownloadAssets(int total, int currentProgress,Action<float> onProgress, List<string> AssetKeyList) //isLabel�� Memory������ üũ
        {
            foreach (string label in AssetKeyList)
            {
                AsyncOperationHandle handle= Addressables.DownloadDependenciesAsync(label);
                
                while (!handle.IsDone)
                {
                    float progress = (currentProgress + handle.PercentComplete) / total;
                    onProgress?.Invoke(progress);
                    await Task.Yield();
                }

                if (handle.Status == AsyncOperationStatus.Failed)
                {
                    Debug.LogError($"Failed to load assets for label: {label}");
                    Addressables.Release(handle);
                    return;
                }
                Addressables.Release(handle);
                currentProgress++;
            }
        }

        public async Task LoadAssetsIntoMemoryAsync(int total,int currentProgress,Action<float> onProgress, List<string> AssetKeyList)
        {
            foreach (string label in AssetKeyList)
            {
                AsyncOperationHandle<IList<GameObject>> _handle = Addressables.LoadAssetsAsync<GameObject>(label, null, false);// �ڵ� ������ ���� (false�� ���� Release �ʿ�)
                
                while (!_handle.IsDone)
                {
                    float progress = (currentProgress + _handle.PercentComplete) / total;
                    onProgress?.Invoke(progress);
                    await Task.Yield();
                }

                if (_handle.Status == AsyncOperationStatus.Failed)
                {
                    Debug.LogError($"Failed to load asset for label: {label}");
                    return;
                }

                var locHandle = Addressables.LoadResourceLocationsAsync(label, typeof(GameObject));
                await locHandle.Task;

                // �ε�� ������ �ּ� Ű ��������
                if (locHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    foreach (var item in locHandle.Result)
                    {
                        string addressKey = item.PrimaryKey;
                        m_downLoadedAddressables.Add(addressKey);
                    }
                }
                Addressables.Release(_handle);
                currentProgress++;
            }
        }

        // �޸𸮿� �ö�� ������ ������ Scene�� ����
        public async Task CreateAddressableAsset(List<string> assetObj)
        {
            foreach (var asset in assetObj)
            {
                AsyncOperationHandle<GameObject> _handle = Addressables.InstantiateAsync(asset);
                GameObject obj = await _handle.Task; // �񵿱� �Ϸ���� ��� �� GameObject ȹ��
            }
        }
    }
}