using System.Collections.Generic;
using DUS.AssetLoad;
using DUS.Scene;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SceneLauncher : MonoBehaviour
{
    [SerializeField] SceneType m_currentSceneType;
    [SerializeField] List<AssetReference> m_nextNeedAddressableList = new List<AssetReference>();

    List<string> m_nextSceneNeedAddressableList = new List<string>();

    // ������ ���ؼ� �ּ�Ű �� �޾ƿ���
    // Label�� ���ؼ� �������°� AssetLoadManager���� ó��
    public void GetNextNeedSceneInfo()
    {
        SceneLoadManager.m_NextScene = m_currentSceneType;

        if(m_nextNeedAddressableList.Count == 0)
        {
            Debug.Log("No need addressable list");
            return;
        }

        foreach (var item in m_nextNeedAddressableList)
        {
            m_nextSceneNeedAddressableList.Add(item.RuntimeKey.ToString());
        }
        AssetLoadManager.Instance.SetNextSceneNeedAddressable(m_nextSceneNeedAddressableList);
    }
}
