using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DestructibleUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TMP_Text hpText;

    private Camera playerCamera;

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

        // シーン上のMainCameraを自動取得
        playerCamera = Camera.main;

        // 最初は非表示
        gameObject.SetActive(false);
    }

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
        // 何らかの理由で取得できていなければ再取得
        if (playerCamera == null)
        {
            playerCamera = Camera.main;

            if (playerCamera == null)
            {
                return;
            }
        }

        transform.rotation = Quaternion.LookRotation( playerCamera.transform.forward, playerCamera.transform.up );
    }
}