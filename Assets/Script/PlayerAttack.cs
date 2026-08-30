using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private int attackPower = 10;
    [SerializeField] private float projectileSpeed = 20f;

    [SerializeField] private Camera playerCamera;

    [Header("Auto Fire Settings")]
    [SerializeField] private int autoFireThreshold = 100;
    [SerializeField] private float autoFireInterval = 0.15f;

    [Header("Aim Settings")]
    [SerializeField] private LayerMask aimLayerMask;

    [Header("References")]
    [SerializeField] private BallProjectile ballPrefab;
    [SerializeField] private Transform firePoint;

    public int AttackPower => attackPower;
    public bool IsAutoFireUnlocked => attackPower > autoFireThreshold;

    private float nextFireTime;
    private bool autoFireUnlocked;

    private void Awake()
    {
        autoFireUnlocked = IsAutoFireUnlocked;
    }

    private void Update()
    {
        if (DestructionGameManager.Instance != null &&
            !DestructionGameManager.Instance.IsPlaying)
        {
            return;
        }

        if (Mouse.current == null)
        {
            return;
        }

        if (autoFireUnlocked)
        {
            HandleAutoFire();
        }
        else
        {
            HandleSingleFire();
        }
    }

    private void HandleSingleFire()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Attack();
        }
    }

    private void HandleAutoFire()
    {
        if (!Mouse.current.leftButton.isPressed)
        {
            return;
        }

        if (Time.time < nextFireTime)
        {
            return;
        }

        Attack();

        nextFireTime = Time.time + autoFireInterval;
    }

    private void Attack()
    {
        if (ballPrefab == null ||  firePoint == null || playerCamera == null)
        {
            return;
        }

        // カメラ中央からRayを飛ばす
        Ray aimRay = new Ray( playerCamera.transform.position,  playerCamera.transform.forward );

        Vector3 targetPoint;

        // 照準の先にオブジェクトがあれば、その位置を狙う
        if (Physics.Raycast(aimRay,out RaycastHit hit,100f,aimLayerMask))
        {
            targetPoint = hit.point;
        }
        else
        {
            // 何もなければ前方を狙う
            targetPoint = playerCamera.transform.position +  playerCamera.transform.forward * 100f;
        }

        // FirePointから照準位置への方向
        Vector3 shootDirection = (targetPoint - firePoint.position).normalized;

        BallProjectile ball = Instantiate( ballPrefab,  firePoint.position,   Quaternion.LookRotation(shootDirection) );

        ball.Initialize( attackPower, autoFireUnlocked  );

        Rigidbody rb = ball.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity =  shootDirection * projectileSpeed;
        }
    }

    public void AddAttackPower(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        bool wasAutoFireUnlocked = autoFireUnlocked;

        attackPower += amount;

        autoFireUnlocked = IsAutoFireUnlocked;

        if (DestructionGameManager.Instance != null)
        {
            DestructionGameManager.Instance.OnAttackPowerIncreased( amount,  attackPower);

            // 初めて連射可能になった瞬間だけ通知
            if (!wasAutoFireUnlocked && autoFireUnlocked)
            {
                DestructionGameManager.Instance.OnAutoFireUnlocked();
            }
        }
    }
}