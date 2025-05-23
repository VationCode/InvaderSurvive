using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

namespace DUS.AssetLoad
{
    public class AssetLoader : MonoBehaviour
    {
        public static AssetLoader Instance { get; private set; }
        public List<string> m_NextNeedAssetLabelList { get; private set; } = new List<string>();        // ����� ��巹����
        public List<string> m_SaveAddressKeyList { get; private set; } = new List<string>();             // Label�����δ� Addressables.InstantiateAsync���� �� �̾Ƴ��⿡ ���� ����Ʈ�� ����
        public void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        // ���� ������ ���� ���� �ʿ� ���¸���Ʈ ����
        public void SetNextSceneNeedAddressable(List<string> nextSceneNeedAddressableList)
        {
            m_NextNeedAssetLabelList.Clear();
            m_NextNeedAssetLabelList = nextSceneNeedAddressableList;
        }

        // �ٿ�ε� �Լ� ���� �� ���� ����
        public async Task<bool> DownloadDependenciesAsync(Action<float> onProgress = null)
        {
            int _total = m_NextNeedAssetLabelList.Count;

            //������ ���� ��� �ѹ� �� üũ
            if (_total == 0)                 
            {
                onProgress?.Invoke(1f);
                return true;
            }

            int _currentProgress = 0;
            foreach (string label in m_NextNeedAssetLabelList)
            {
                AsyncOperationHandle _handle = Addressables.DownloadDependenciesAsync(label);

                while (!_handle.IsDone)
                {
                    float progress = (_currentProgress + _handle.PercentComplete) / _total;
                    //onProgress?.Invoke(progress);
                    await Task.Yield();
                }
                
                if(_handle.Status == AsyncOperationStatus.Failed)
                {
                    Debug.LogError($"Failed to download dependencies for label: {label}");
                    Addressables.Release(_handle);
                    return false;
                }
                _currentProgress++;
                Addressables.Release(_handle); // �����ص� �ݵ�� ����
            }
            onProgress?.Invoke(1f); // ������ 1�� ä����
            return true;
        }

        // �ٿ�ε�� ������ �޸𸮿� �ε�(��, �ش� ���� �ε�)
        public async Task<bool> LoadAssetsIntoMemoryAsync(Action<float> onProgress = null)
        {
            m_SaveAddressKeyList.Clear(); // �޸𸮿� �ε�� ���� ����Ʈ �ʱ�ȭ

            int _total = m_NextNeedAssetLabelList.Count;

            //������ ���� ��� �ѹ� �� üũ
            if (_total == 0)
            {
                onProgress?.Invoke(1f);
                return true;
            }

            int _currentProgress = 0;

            foreach (string label in m_NextNeedAssetLabelList)
            {
                AsyncOperationHandle<IList<GameObject>> _handle = Addressables.LoadAssetsAsync<GameObject>(label, null, false);// �ڵ� ������ ���� (false�� ���� Release �ʿ�)

                while (!_handle.IsDone)
                {
                    float progress = (_currentProgress + _handle.PercentComplete) / _total;
                    //onProgress?.Invoke(_totalProgress);
                    await Task.Yield();
                }

                if (_handle.Status == AsyncOperationStatus.Failed)
                {
                    Debug.LogError($"Failed to load asset for label: {label}");
                    return false;
                }

                var locHandle = Addressables.LoadResourceLocationsAsync(label, typeof(GameObject));

                // �ε�� ������ �ּ� Ű ��������
                foreach (var item in locHandle.Result)
                {
                    string addressKey = item.PrimaryKey;
                    m_SaveAddressKeyList.Add(addressKey);
                }
                
                Addressables.Release(_handle);
                _currentProgress++;
            }
            //onProgress?.Invoke(1f); // ������ 1�� ä����
            return true;
        }

        public async Task<long> CheckDownloadSizeAsync(string label)
        {
            var sizeHandle = Addressables.GetDownloadSizeAsync(label);
            await sizeHandle.Task;

            if (sizeHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log($"[Check] Download size for {label}: {sizeHandle.Result} bytes");
                return sizeHandle.Result;
            }
            else
            {
                Debug.LogError($"[Check] Failed to get download size for {label}");
                return -1;
            }
        }

        // �޸𸮿� �ö�� ������ ������ Scene�� ����
        public void CreateAddressableAsset(string assetObj, Transform transform = null, Transform parent = null)
        {
            Transform tr = transform;
            if (tr == null) tr = new GameObject(assetObj).transform;

            tr.position = new Vector3(UnityEngine.Random.Range(-2f,2f),0,0);
            Addressables.InstantiateAsync(assetObj, tr, true);
            
        }
    }
}