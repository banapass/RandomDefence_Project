using UnityEngine;

public class TestCode : MonoBehaviour
{
    [SerializeField] float angle;
    [SerializeField] float count;
    public void LaunchProjectile()
    {

        Vector2 targetDirection = Vector2.right;
        float angleStep = angle / (count - 1);  // 각도 간격 계산
        float halfAngle = angle / 2;

        for (int i = 0; i < count; i++)
        {
            float currentAngle = -halfAngle + i * angleStep;
            Vector2 direction = Quaternion.Euler(0f, 0f, currentAngle) * targetDirection;

            DebugDrawLaunchDirection(Vector2.zero, direction);
        }
    }
    private void DebugDrawLaunchDirection(Vector2 startPos, Vector2 direction)
    {
        Debug.DrawRay(startPos, direction, Color.red, 1f);
    }
}