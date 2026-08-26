using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private int attackPower = 10;
    [SerializeField] private float projectileSpeed = 20f;

    [Header("References")]
    [SerializeField] private BallProjectile ballPrefab;
    [SerializeField] private Transform firePoint;

    private void Update()
    {
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (ballPrefab == null || firePoint == null)
        {
            Debug.LogWarning(
                "BallPrefabまたはFirePointが設定されていません。"
            );

            return;
        }

        BallProjectile ball = Instantiate(
            ballPrefab,
            firePoint.position,
            firePoint.rotation
        );

        ball.Initialize(attackPower);

        Rigidbody rb = ball.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity =
                firePoint.forward * projectileSpeed;
        }
    }
}