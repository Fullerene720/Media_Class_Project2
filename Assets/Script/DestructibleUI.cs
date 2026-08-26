using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DestructibleUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TMP_Text hpText;

    [Header("Camera")]
    [SerializeField] private Camera playerCamera;

    /// <summary>
    /// HP UIを初期化する。
    /// </summary>
    public void Initialize(int maxHp)
    {
        if (hpSlider != null)
        {
            hpSlider.minValue = 0;
            hpSlider.maxValue = maxHp;
            hpSlider.value = maxHp;
        }

        if (hpText != null)
        {
            hpText.text = $"{maxHp} / {maxHp}";
        }

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        // 最初は非表示
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 現在HPに合わせてUIを更新する。
    /// </summary>
    public void UpdateHp(int currentHp, int maxHp)
    {
        gameObject.SetActive(true);

        if (hpSlider != null)
        {
            hpSlider.value = currentHp;
        }

        if (hpText != null)
        {
            hpText.text = $"{currentHp} / {maxHp}";
        }
    }

    private void LateUpdate()
    {
        if (playerCamera == null)
        {
            return;
        }

        // カメラと同じ方向を向かせる
        transform.rotation = Quaternion.LookRotation(
            playerCamera.transform.forward,
            playerCamera.transform.up
        );
    }
}