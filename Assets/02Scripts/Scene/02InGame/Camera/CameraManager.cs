using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera m_mainCamera; // ���� ī�޶�
    
    [SerializeField] private Transform m_target; // ī�޶� ���� ���

    public Vector3 m_Dir { get; private set; } // ī�޶� ����
    private Vector3 m_currentVelocity;
    [SerializeField] private float smoothSpeed = 0.125f;

    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 15f;

    PlayerCore m_playerCore; // �÷��̾� �ھ� ��ũ��Ʈ
    float mouseX;
    float mouseY;

    private void Awake()
    {
        if(!m_playerCore.m_photonView.IsMine)
        {
            m_mainCamera.GetComponent<AudioListener>().enabled = false;
            //Destroy(this.gameObject); // �ٸ� �÷��̾��� ī�޶�� ����
        }
    }

    public void Initialize(PlayerCore core)
    {
        this.m_playerCore = core;
    }

    private void Start()
    {
        transform.parent = null; // ī�޶� �ٸ� ������Ʈ�� �ڽ��� ���� �ʵ��� ����
    }
    private void LateUpdate()
    {
        if (!m_playerCore.m_photonView.IsMine) return;
        FollowCam();
    }

    private void FollowCam()
    {
        if (m_target == null) return;
        Vector3 desiredPosition = m_target.position;
        desiredPosition = Vector3.SmoothDamp(desiredPosition, m_target.position, ref m_currentVelocity, smoothSpeed); // ī�޶� ���� ��ġ
        transform.position = desiredPosition;
    }

    public void OnCameraRotation(Vector2 lookInput)
    {
        mouseX += lookInput.x * rotationSpeed * Time.deltaTime;
        mouseY -= lookInput.y * rotationSpeed * Time.deltaTime;

        mouseY = Mathf.Clamp(mouseY, -60f, 60f);
        // ī�޶� ȸ��

        Vector3 mouseDir = new Vector3(mouseY, mouseX, 0);
        transform.rotation = Quaternion.Euler(mouseDir);

        m_Dir = new Vector3(0, mouseX, 0);

    }
}
