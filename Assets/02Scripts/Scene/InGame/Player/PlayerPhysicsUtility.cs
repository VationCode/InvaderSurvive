// ======================================== 250430
// ���� ���� ���� Ŭ����
// ========================================
using UnityEngine;

namespace DUS.Player {
    public static class PlayerPhysicsUtility
    {
        public static void SetVelocityXZ(Rigidbody rigid, Vector3 velocityXZ)
        {
            Vector3 currentVelocity = rigid.linearVelocity;
            rigid.linearVelocity = new Vector3(velocityXZ.x, currentVelocity.y, velocityXZ.z);
        }

        public static Vector3 ClampVelocityY(Rigidbody rigid, float minY, float maxY)
        {
            Vector3 v = rigid.linearVelocity;
            v.y = Mathf.Clamp(v.y, minY, maxY);
            rigid.linearVelocity = v;
            return v;
        }

        // ����
        public static void ApplyGravity(Rigidbody rigid)
        {
            // �������� ����Ǵ� �ڿ������� ������ ���
            rigid.AddForce(Physics.gravity * rigid.mass, ForceMode.Force);

            /*var velocityXZ = rigid.linearVelocity;
            var acceleration = Physics.gravity;
            velocityXZ += acceleration * Time.fixedDeltaTime;
            rigid.linearVelocity = velocityXZ;*/
        }
    }
}
