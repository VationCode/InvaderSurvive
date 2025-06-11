using Photon.Pun;
using UnityEngine;
using System.Threading.Tasks;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine.XR;
using System.Collections;

namespace DUS.Network
{
    public class NetworkManager : MonoBehaviourPunCallbacks, INetworkService
    {
        public static NetworkManager Instance { get; private set; }
        public static INetworkService Service => Instance; //�ش� ���񽺿� �ִ� �Լ����� �����ϴ� ������� ���յ� ���� �̱��� ���� ������ �ƴ�

        public event Action<List<RoomInfo>> UpdateRoomList;
        public event Action<Player[]> JoinedPlayerList;
        public event Action<Player> EntertedPlayer;
        public event Action<Player> LeftPlayer;

        public event Action<bool> ActivateStartInGame;
        // ���� ����
        public Player m_player { get; private set; }

        private bool m_isConnected;
        private bool m_isCreatedRoom;

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
            Debug.Log("OnConnectedToMaster" + PhotonNetwork.IsConnectedAndReady);
            m_player = PhotonNetwork.LocalPlayer;
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            //PhotonNetwork.ReconnectAndRejoin();
            //PhotonNetwork.ConnectUsingSettings();
            m_isConnected = false;
        }
        #endregion ====================================================== /BootScene���� ù ���۽�

        // �񵿱� ó���̱⿡ LeaveRoom�� JoinLobby�� ���� ó���� �κ���� ������ ����ȵ� ���� ����
        public void JoinLobby()
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                Debug.LogWarning("JoinLobby");
                PhotonNetwork.JoinLobby();
            }
            else
            {
                Debug.LogWarning("Photon not ready. Cannot join lobby.");
            }
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("�κ� ���� ����");
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
        public void JoinRoom(string roomNmae, string playerName)
        {
            Debug.Log("JoinRoom");
            m_player.NickName = playerName;
            PhotonNetwork.LeaveLobby();
            PhotonNetwork.JoinRoom(roomNmae);
        }

        /*private void Update()
        {
            Debug.Log(PhotonNetwork.IsConnectedAndReady);
        }*/
        // �� �÷��̾�� ����
        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom");
            Player[] players = PhotonNetwork.PlayerList;

            JoinedPlayerList?.Invoke(players);
        }
        // �濡 ���� ����� ���� ���� �濡 �ִ� ������� ó��
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log("EntertedPlayer");
            EntertedPlayer?.Invoke(newPlayer);
        }

        // ������ ������ �� ȣ��
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log("OnPlayerLeftRoom");
            LeftPlayer?.Invoke(otherPlayer);
        }

        // �� ��� ����, �� ����Ʈ�� ��ȭ�� ������ ȣ��(��ȭ�� �����鿡 ����)
        // ���� ������ �κ�� ���ƿ��� �� �� ����Ʈ ��ȭ ������ �ҷ����� ����(��, �濡 ������ ������Ʈ�� �̷������ ����)
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("OnRoomListUpdate");
            UpdateRoomList?.Invoke(roomList);
        }

        public void LeaveRoom()
        {
            // ��->�κ��ϰ��
            if (PhotonNetwork.InRoom)
            {
                Debug.Log("LeaveRoom");
                PhotonNetwork.LeaveRoom();
            }
        }

        // StartCoroutine ����Ͽ� �κ�� ���� �ð� �ִ� ����
        // �񵿱� ó���̱⿡ LeaveRoom�� JoinLobby�� ���� ó���� �κ���� ������ ����ȵ� ���� ����
        public override void OnLeftRoom()
        {
            Debug.Log("OnLeftRoom");
            StartCoroutine(SafeJoinLobby());
        }

        private IEnumerator SafeJoinLobby()
        {
            yield return new WaitUntil(() => PhotonNetwork.IsConnectedAndReady);
            Debug.Log("JoinLobby �غ� ���� �Ϸ�");
            PhotonNetwork.JoinLobby();
        }
        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            ActivateStartInGame(PhotonNetwork.IsMasterClient);
        }
        #endregion ====================================================== /LobbyScene���� ����

        #region ====================================================== InGameScene���� ����



        #endregion ====================================================== /InGameScene���� ����

    }
}
