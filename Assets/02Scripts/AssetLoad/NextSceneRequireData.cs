// ���� �������� ��û�� ������

using DUS.scene;
using System.Collections.Generic;
using UnityEngine;


namespace DUS.AssetLoad
{
    
    public class NextSceneRequireData : ScriptableObject
    {
        public SceneType m_SceneType;
        public List<string> m_RequiredAddressableLabelKeyList = new List<string>(); // �󺧴����� ����
        public List<string> m_RequiredAddressableKeyList = new List<string>();      // �Ϲ� ������ ����
    }
}
