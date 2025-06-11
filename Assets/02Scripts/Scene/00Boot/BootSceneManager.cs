// �� ��ȯ �� �ʿ� ���µ��� �̸� �ٿ�ε��ϰ� �ε��ϴ� �� �Ŵ���
// �ε��ϴµ����� �ε��ٸ� �����ְ�, �ε尡 ������ ���� ������ ��ȯ
using UnityEngine;
using UnityEngine.AddressableAssets;
using DUS.AssetLoad;
using DUS.scene;
using System.Threading.Tasks;
using Photon.Pun;
using DUS.Network;
using DUS.Manager;
using System;

namespace DUS.Scene
{
    public class BootSceneManager : MonoBehaviour
    {
        [SerializeField]
        LoadingEffectController m_loadingEffectController;

        // ���յ� ���߱����� �������̽��� Ȱ���Ͽ� �Լ��� ����
        private INetworkService m_networkService;
        private IAssetLoadService m_assetLoadService;
        private ISceneLoadService m_sceneLoadService;
        private void Awake()
        {
            m_networkService = NetworkManager.Service;
            m_assetLoadService = AssetLoadManager.assetLoadService;
            m_sceneLoadService = SceneLoadManager.sceneLoadService;
        }
        private async void Start()
        {
            // 1.����ȭ �ʱ�ȭ

            await AssetLoadManager.Instance.InitializeAsync();
            m_networkService?.ConnectNetwork();

            // ��Ʈ��ũ ����ɶ�����
            while (!m_networkService.CheckConnected()) await Task.Delay(100);


            await m_assetLoadService?.DownLoadAndSceneUpload(m_loadingEffectController.m_onProgress, SceneLoadManager.Instance.m_nextSceneRequireData);

            await Task.Delay(1000);

            if (SceneLoadManager.m_NextScene == SceneType.Lobby) m_networkService.JoinLobby();

            m_sceneLoadService?.LoadNextScene();
        }
    }
}

/*
           await Addressables.InitializeAsync().Task;

           // 2. ���� �� �ʿ� ���µ� �������� �ٿ�ε�
           bool downloadSuccess = await AssetLoadManager.Instance.DownloadAsset(m_loadingEffectController.m_onProgress);

           if(!downloadSuccess)
           {
               Debug.LogError("Download failed");
               return;
           }

           // 3. �ٿ� ���� ���µ� �޸𸮿� �ε�(���� ���� �ø��� ���� �ƴ�)
           bool _isInMemoryAsset = await AssetLoadManager.Instance.LoadAssetsIntoMemoryAsync(m_loadingEffectController.m_onProgress);
           if(!_isInMemoryAsset)
           {
               return;
           }

           // 4. ������ �̵�
           SceneLoadManager.Instance.LoadNextScene_Boot(SceneLoadManager.m_NextScene);

           // 5. �ٿ���� ���µ� �ν��Ͻ�ȭ�� ���� �� �Ŵ������� Start�κп��� �ۼ�*/