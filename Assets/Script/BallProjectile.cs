using UnityEngine;

public class BallProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float lifeTime = 5f;

    private int attackPower;
    private bool hasHit;

    /// <summary>
    /// 球を初期化する。
    /// </summary>
    public void Initialize(int damage)
    {
        attackPower = damage;

        // どこにも当たらなかった球が残り続けるのを防ぐ
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit)
        {
            return;
        }

        hasHit = true;

        IDamageable damageable =
            collision.collider.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(attackPower);
        }

        // 何かに当たったら球を消す
        Destroy(gameObject);
    }
}