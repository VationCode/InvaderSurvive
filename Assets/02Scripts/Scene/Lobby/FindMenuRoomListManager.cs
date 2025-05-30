using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DUS.UI
{
    // �ǻ������δ� FindMenu�� �ִ� ���η��ư ����Ʈ�� ����
    public class FindMenuRoomListManager : MonoBehaviour, IInitializeAtStart
    {
        [SerializeField]
        GameObject m_joinRoomBtnItemPrefab;
        [Header("[Get FindRoomMenu]")]
        [SerializeField] Transform m_roomListScrollViewContent;

        public string m_getRoomName { get; private set; }

        public Dictionary<string, FindRoomMenuRoomButtonItem> m_roomListDict { get; private set; } = new Dictionary<string, FindRoomMenuRoomButtonItem>();

        private void Start()
        {
            LobbySceneManager.Instance.InitializeAtStart += InitializeAtStart;
        }
        public void InitializeAtStart()
        {
        }
        // ������޴�UI���� ���� ��ư
        public string OnRoomCreate(string roomName)
        {
            m_getRoomName = roomName;
            if (string.IsNullOrEmpty(m_getRoomName))
            {
                m_getRoomName = "Room" + Random.Range(0, 1000);
            }

            CreateRoomButtonItem(m_getRoomName);
            return m_getRoomName;
        }

        public void FailedRoomCrate()
        {
            AllRemoveRoomList();
        }

        public void OnRoomLeave()
        {
            AllRemoveRoomList();
        }

        public void OnRoomUpdateList(bool isRemove, string roomName)
        {
            if (isRemove) RemoveRoomList(roomName);
            else CreateRoomButtonItem(roomName);
        }
        // �������� �� ���� �ø��� FindMenuUI�� �渮��Ʈ(��ư) ����
        private void CreateRoomButtonItem(string createRoomName)
        {
            if (m_roomListDict.ContainsKey(createRoomName)) return; // �̹� �����ϴ� �� �̸��̸� �������� ����

            GameObject roomList = Instantiate(m_joinRoomBtnItemPrefab, m_roomListScrollViewContent);
            FindRoomMenuRoomButtonItem joinBtnInfo = roomList.GetComponent<FindRoomMenuRoomButtonItem>();

            joinBtnInfo.SetRoomListInfo(createRoomName);
            m_roomListDict.Add(createRoomName, joinBtnInfo);
        }

        private void RemoveRoomList(string createRoomName)
        {
            if (!m_roomListDict.ContainsKey(createRoomName)) return;
            Destroy(m_roomListDict[createRoomName].gameObject);
            m_roomListDict.Remove(createRoomName);
        }

        private void AllRemoveRoomList()
        {
            foreach (var roomName in m_roomListDict.Keys)
            {
                Destroy(m_roomListDict[roomName].gameObject);
            }
            m_roomListDict.Clear();
        }
    }
}


