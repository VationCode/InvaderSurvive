using UnityEngine;


namespace DUS.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

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

        void Start()
        {
            // TODO : ���� �� �ʿ��� �ֿ� ���ҽ����� �̸� �ٿ�

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
