//

//
using UnityEngine;
using UnityEngine.AddressableAssets;
using DUS.AssetLoad;
using UnityEngine.SceneManagement;
using DUS.Scene;

public class BootSceneManager : MonoBehaviour
{
    [SerializeField]
    LoadingProgress m_loadingProgress;

    private async void Start()
    {
        // 1.��巹���� �ʱ�ȭ
        await Addressables.InitializeAsync().Task;

        // 2. ���� �� �ʿ� ���µ� �������� �ٿ�ε�
        bool downloadSuccess = await AssetLoader.Instance.DownloadDependenciesAsync(m_loadingProgress.m_onProgress);

        if(!downloadSuccess)
        {
            Debug.LogError("Download failed");
            return;
        }

        // 3. �ٿ� ���� ���µ� �޸𸮿� �ε�(���� ���� �ø��� ���� �ƴ�)
        bool _isInMemoryAsset = await AssetLoader.Instance.LoadAssetsIntoMemoryAsync(m_loadingProgress.m_onProgress);
        if(!_isInMemoryAsset)
        {
            return;
        }

        // 4. ������ �̵�
        SceneLoadManager.Instance.LoadNextScene(SceneLoadManager.m_NextScene);

        // 5. �ٿ���� ���µ� �ν��Ͻ�ȭ�� ���� �� �Ŵ������� Start�κп��� �ۼ�
    }
}
