using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DUS.UI
{
    public class PlayerListUIManager : MonoBehaviour, IInitializeAtStart
    {
        [SerializeField]
        GameObject m_playerListItemPrefab;

        [Header("[Get RoomMenu]"),SerializeField]
        Transform m_playerListScrollViewContent; // m_playerListItemPrefab ���� �θ�
        public string m_minePlayerName { get; private set; }
        public Dictionary<string, PlayerListItem> m_playerListDict { get; private set; } = new Dictionary<string, PlayerListItem>();

        private void Start()
        {
            LobbySceneManager.Instance.InitializeAtStart += InitializeAtStart;
        }

        public void InitializeAtStart(){}

        // �� ���� ���� m_playerListDict�� Clear�ϱ⿡ �����ÿ��� �Ű澲���
        public void CreatePlayerListItem(Player[] players)
        {
            // ������ ���� ��
            AllRemove();

            foreach (var playerItem in players)
            {
                GameObject playerList = Instantiate(m_playerListItemPrefab, m_playerListScrollViewContent);
                PlayerListItem PlayerListItem = playerList.GetComponent<PlayerListItem>();
                m_playerListDict.Add(playerItem.NickName, PlayerListItem);

                PlayerListItem.SetPlayerListText(playerItem.NickName);
            }
        }

        public void AllRemove()
        {
            foreach (var playerItem in m_playerListDict.Keys)
            {
                Destroy(m_playerListDict[playerItem].gameObject);
            }
            m_playerListDict.Clear();
        }


    }
}