using System.Collections;
using DUS.AssetLoad;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

namespace DUS.Scene
{
    public enum SceneType
    {
        Loaby = 0,
        Boot = 1,
        InGame = 2,
        InGame2 = 3,
        Test = 4
    }
    public class SceneLoadManager : MonoBehaviour
    {
        public static SceneLoadManager Instance { get; private set; }
        public static SceneType m_NextScene;

        private Coroutine m_asyncLoadCR;
        List<string> startAddressableList = new List<string>();
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        private void Start()
        {
            startAddressableList.Add("PlayerInfoList");
            m_NextScene = SceneType.Test;
            
            PushNextInfoToBootSceneAndLoadBootScene(m_NextScene, startAddressableList);
        }

        // ���� ������ �Ѿ�� �������� ����
        public void PushNextInfoToBootSceneAndLoadBootScene(SceneType nextScene, List<string> nextNeedAssetList)
        {
            m_NextScene = nextScene;
            AssetLoader.Instance.SetNextSceneNeedAddressable(nextNeedAssetList);
            // TODO : ���µ� ����(��ġ��, ���� ���) ����
            SceneManager.LoadScene(1);
        }

        public void LoadNextScene(SceneType nextScene)
        {
            //Boot Scene ����
            SceneManager.LoadScene((int)nextScene);
        }
    }
}