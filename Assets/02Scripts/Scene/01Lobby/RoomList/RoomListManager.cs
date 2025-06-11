using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DUS.UI
{
    // �ǻ������δ� FindMenu�� �ִ� ���η��ư ����Ʈ�� ����
    public class RoomListManager : MonoBehaviour, IInitializeAtStart
    {
        [SerializeField]
        GameObject m_joinRoomBtnPrefab;
        [Header("[Get FindRoomMenu]")]
        [SerializeField] Transform m_roomListScrollViewContent;

        public string m_getRoomName { get; private set; }

        public Dictionary<string, JoinRoomBtn> m_roomListDict { get; private set; } = new Dictionary<string, JoinRoomBtn>();

        private void Start()
        {
            LobbySceneManager.Instance.InitializeAtStart += InitializeAtStart;
        }
        public void InitializeAtStart()
        {
        }
        // ������޴�UI���� ���� ��ư

        public void CreateJoinRoomBtn(string roomName)
        {
            if (m_roomListDict.ContainsKey(roomName)) return;
            GameObject _roomList = Instantiate(m_joinRoomBtnPrefab, m_roomListScrollViewContent);
            JoinRoomBtn _joinBtn = _roomList.GetComponent<JoinRoomBtn>();
            _joinBtn.SetBtnRoomName(roomName);
            m_roomListDict.Add(roomName, _joinBtn);
        }

        // ��Ʈ��ũ���� �� ������Ʈ(��ȭ �߻�) �� ���� ��ư ����Ʈ�� ���� ���� ����
        // TODO : ���� �����ϱ⺸�ٴ� ���� ������ ���� �Ϻθ� ���� ���� �ǵ���
        /*public void UpdateJoinButtonList(List<RoomInfo> createRoomList)
        {
            // ��ü ����
            AllRemoveRoomList();

            // ���� ����Ʈ ����
            foreach (var roomInfo in createRoomList)
            {
                // ���� �����ǰ� �����Ǵ°� �ƴ϶� ������ �������ϴ°��̱⿡ �̸� �Ǵ��ؼ� ����Ʈ ����
                if(roomInfo.IsVisible)
                {
                    GameObject _roomList = Instantiate(m_joinRoomBtnPrefab, m_roomListScrollViewContent);
                    JoinRoomBtn _joinBtn = _roomList.GetComponent<JoinRoomBtn>();
                    _joinBtn.SetBtnRoomName(roomInfo.Name);
                    m_roomListDict.Add(roomInfo.Name, _joinBtn);
                }
            }
        }*/

        public void RemoveRoomList(string createRoomName)
        {
            if (!m_roomListDict.ContainsKey(createRoomName)) return;
            Destroy(m_roomListDict[createRoomName].gameObject);
            m_roomListDict.Remove(createRoomName);
        }

        public void AllRemoveRoomList()
        {
            foreach (var roomName in m_roomListDict.Keys)
            {
                Destroy(m_roomListDict[roomName].gameObject);
            }
            m_roomListDict.Clear();
        }
    }
}


