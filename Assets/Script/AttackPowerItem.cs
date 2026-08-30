using UnityEngine;

public class AttackPowerItem : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] private int increaseAmount = 10;
    [SerializeField] private float rotateSpeed = 90f;

    private void Update()
    {
        transform.Rotate(Vector3.up,rotateSpeed * Time.deltaTime,Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerAttack playerAttack = other.GetComponentInParent<PlayerAttack>();

        if (playerAttack == null)
        {
            return;
        }

        playerAttack.AddAttackPower(increaseAmount);

        Destroy(gameObject);
    }
}