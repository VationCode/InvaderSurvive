using Photon.Realtime;
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

        public Dictionary<string, FindRoomMenuJoinBtn> m_roomListDict { get; private set; } = new Dictionary<string, FindRoomMenuJoinBtn>();

        private void Start()
        {
            LobbySceneManager.Instance.InitializeAtStart += InitializeAtStart;
        }
        public void InitializeAtStart()
        {
        }
        // ������޴�UI���� ���� ��ư

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
            //else UpdateFindRoomMenuButtonList(roomName);
        }

        // ��Ʈ��ũ���� �� ������Ʈ �� ���� ����Ʈ�� ���� ����
        // TODO : ���� �����ϱ⺸�ٴ� ���� ������ ���� �Ϻθ� ���� ���� �ǵ���
        public void UpdateFindRoomMenuButtonList(List<RoomInfo> createRoomList)
        {
            // ��ü ����
            foreach(var item in m_roomListDict.Keys)
            {
                Destroy(m_roomListDict[item].gameObject);
            }
            m_roomListDict.Clear();

            // ���� ����Ʈ ����
            foreach(var roomInfo in createRoomList)
            {
                GameObject _roomList = Instantiate(m_joinRoomBtnItemPrefab, m_roomListScrollViewContent);
                FindRoomMenuJoinBtn _joinBtn = _roomList.GetComponent<FindRoomMenuJoinBtn>();
                m_roomListDict.Add(roomInfo.Name, _joinBtn);
            }
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


