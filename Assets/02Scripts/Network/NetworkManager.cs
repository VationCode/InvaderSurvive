using Photon.Pun;
using UnityEngine;
using System.Threading.Tasks;
using Photon.Realtime;
using System;
using System.Collections.Generic;

namespace DUS.Network
{
    public class NetworkManager : MonoBehaviourPunCallbacks, INetworkService
    {
        public static NetworkManager Instance { get; private set; }
        public static INetworkService Service => Instance; //�ش� ���񽺿� �ִ� �Լ����� �����ϴ� ������� ���յ� ���� �̱��� ���� ������ �ƴ�

        public event Action<List<RoomInfo>> UpdateRoomList;
        public event Action<Player[]> JoinedPlayerList;
        // ���� ����
        public Player m_player { get; private set; }

        private bool m_isConnected;
        private bool m_isCreatedRoom;

        private List<string> m_roomList = new List<string>();
        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        #region ====================================================== BootScene���� ù ���۽�
        public void ConnectNetwork()
        {
            if (PhotonNetwork.IsConnected) return;
            PhotonNetwork.AutomaticallySyncScene = true; // ������ Ŭ���̾�Ʈ�� ���� �����ϸ� �ٸ� Ŭ���̾�Ʈ�鵵 �ڵ����� ����ȭ��
            PhotonNetwork.ConnectUsingSettings();
        }

        public bool CheckConnected() => m_isConnected;

        public override void OnConnected()
        {
            Debug.Log("OnConnected");
            m_isConnected = true;
        }

        public override void OnConnectedToMaster()
        {
            m_player = PhotonNetwork.LocalPlayer;
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            PhotonNetwork.ConnectUsingSettings();
            m_isConnected = false;
        }
        #endregion ====================================================== /BootScene���� ù ���۽�

        public void JoinLobby()
        {
            if(PhotonNetwork.InRoom)
            {
                Debug.Log("InRoom && LeaveRoom");
                PhotonNetwork.LeaveRoom();
            }
            PhotonNetwork.JoinLobby();
            Debug.Log("JoinLobby");
        }

        #region ====================================================== LobbyScene���� ����
        // �� ����
        public void CreateRoom(string roomName, string playerName)
        {
            Debug.Log("CreateRoom");
            PhotonNetwork.CreateRoom(roomName);
            m_player.NickName = playerName;
            m_isCreatedRoom = true;
        }

        public bool CheckCreateRoom() => m_isCreatedRoom;

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log(message);
            m_isCreatedRoom = false;
        }

        // �� ����

        // �� ������ ��

        // �� �÷��̾�� ����

        // �� ��� ����, �� ����Ʈ�� ������Ʈ�� �� ���� �ҷ����� ���� �Լ�
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("OnRoomListUpdate");
            UpdateRoomList?.Invoke(roomList);
        }

        public void JoinRoom(string roomNmae)
        {
            PhotonNetwork.JoinRoom(roomNmae);
        }
        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoomNewPlayer");
            Player[] players = PhotonNetwork.PlayerList;

            JoinedPlayerList?.Invoke(players);
        }

        #endregion ====================================================== /LobbyScene���� ����

    }
}
