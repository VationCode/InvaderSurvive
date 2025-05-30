// �� ��ȯ �� �ʿ� ���µ��� �̸� �ٿ�ε��ϰ� �ε��ϴ� �� �Ŵ���
// �ε��ϴµ����� �ε��ٸ� �����ְ�, �ε尡 ������ ���� ������ ��ȯ
using UnityEngine;
using UnityEngine.AddressableAssets;
using DUS.AssetLoad;
using UnityEngine.SceneManagement;
using DUS.Scene;
using System.Threading.Tasks;
using Photon.Pun;

public class BootSceneManager : MonoBehaviour
{
    [SerializeField]
    LoadingProgress m_loadingProgress;

    private async void Start()
    {
        // ���۽� ���� ����ȭ ��������ϱ⿡ �κ���� ��Ʈ��ũ�� �ƴ� ���⼭ ����
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.JoinLobby();
            PhotonNetwork.AutomaticallySyncScene = true; // ������ Ŭ���̾�Ʈ�� ���� �����ϸ� �ٸ� Ŭ���̾�Ʈ�鵵 �ڵ����� ����ȭ��
        }

        // Boot ������ �����ϹǷ�
        if (GameManager.Instance.m_StartCheck == -1)
        {
            SceneLoadManager.Instance.PushNextInfoToBootSceneAndLoadBootScene(SceneType.Lobby);
            GameManager.Instance.SetStartCheck();
            SceneLoadManager.Instance.LoadNextScene();
        }

        await Task.Delay(2000); // 2�� ���
        SceneLoadManager.Instance.LoadNextScene();


        /*// 1.��巹���� �ʱ�ȭ
        await Addressables.InitializeAsync().Task;

        // 2. ���� �� �ʿ� ���µ� �������� �ٿ�ε�
        bool downloadSuccess = await AssetLoadManager.Instance.DownloadDependenciesAsync(m_loadingProgress.m_onProgress);

        if(!downloadSuccess)
        {
            Debug.LogError("Download failed");
            return;
        }

        // 3. �ٿ� ���� ���µ� �޸𸮿� �ε�(���� ���� �ø��� ���� �ƴ�)
        bool _isInMemoryAsset = await AssetLoadManager.Instance.LoadAssetsIntoMemoryAsync(m_loadingProgress.m_onProgress);
        if(!_isInMemoryAsset)
        {
            return;
        }

        // 4. ������ �̵�
        SceneLoadManager.Instance.LoadNextScene(SceneLoadManager.m_NextScene);

        // 5. �ٿ���� ���µ� �ν��Ͻ�ȭ�� ���� �� �Ŵ������� Start�κп��� �ۼ�*/
    }
}
