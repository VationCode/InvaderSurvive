using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DUS.UI;

public class GoMenuUIBtn : MonoBehaviour
{
    [SerializeField] GoMenuUIType m_goMenuType;
    Button m_btn;

    private void Awake()
    {
        m_btn = GetComponent<Button>();
        m_btn.onClick.AddListener(OnClickMainButtons);
    }

    private void OnClickMainButtons()
    {
        // �ڱ� �ڽ��� ������ ������ �޴� ��Ȱ��ȭ
        LobbySceneManager.Instance.GoMenuUI(m_goMenuType);
    }
}
