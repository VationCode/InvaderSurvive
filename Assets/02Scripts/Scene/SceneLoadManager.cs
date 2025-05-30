using System.Collections;
using DUS.AssetLoad;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Photon.Pun;

namespace DUS.Scene
{
    public enum SceneType
    {
        Boot = 0,
        Lobby = 1,
        InGame = 2,
        InGame2 = 3,
        Test = 4
    }
    public class SceneLoadManager : MonoBehaviour
    {
        public static SceneLoadManager Instance { get; private set; }
        public static SceneType m_NextScene;

        public SceneType m_CurrentScene;
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if(SceneManager.GetActiveScene().buildIndex == (int)SceneType.Boot)
            {
                m_NextScene = SceneType.Lobby;
            }
            
        }

        // ���� ������ �Ѿ�� �������� ����
        public void PushNextInfoToBootSceneAndLoadBootScene(SceneType nextScene, List<string> nextNeedAssetList = null)
        {
            m_NextScene = nextScene;

            // TODO : ���µ� ����(��ġ��, ���� ���) ����
            if (nextNeedAssetList != null)
            AssetLoadManager.Instance.SetNextSceneNeedAddressable(nextNeedAssetList);

            // Boot ������ �̵�
            if(SceneManager.GetActiveScene().buildIndex != (int)SceneType.Boot)
            {
                SceneManager.LoadScene((int)SceneType.Boot);
            }
        }

        public void LoadNextScene()
        {
            m_CurrentScene = m_NextScene;
            PhotonNetwork.LoadLevel((int)m_NextScene);
            //Boot Scene ����
            //SceneManager.LoadScene((int)m_NextScene);
        }
    }
}