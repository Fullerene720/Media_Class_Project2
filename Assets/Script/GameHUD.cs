using System.Collections;
using TMPro;
using UnityEngine;

public class GameHUD : MonoBehaviour
{
    [Header("Main HUD")]
    [SerializeField] private TMP_Text damageCostText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text attackPowerText;
    [SerializeField] private TMP_Text attackPowerMessageText;
    [SerializeField] private TMP_Text autoFireMessageText;

    

    [Header("Countdown")]
    [SerializeField] private TMP_Text countdownText;

    [Header("Result")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text resultDamageCostText;

    private Coroutine attackPowerMessageCoroutine;

    private Coroutine autoFireMessageCoroutine;

    public void UpdateDamageCost(int damageCost)
    {
        if (damageCostText == null)
        {
            return;
        }

        damageCostText.text = $"¥{damageCost:N0}";
    }

    public void UpdateTimer(float remainingTime)
    {
        if (timerText == null)
        {
            return;
        }

        int totalSeconds =
            Mathf.CeilToInt(remainingTime);

        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        timerText.text =
            $"{minutes:00}:{seconds:00}";
    }

    public void UpdateAttackPower(int attackPower)
    {
        if (attackPowerText == null)
        {
            return;
        }

        attackPowerText.text =$"攻撃力：{attackPower}";
    }

    public void ShowAttackPowerIncrease(int amount)
    {
        if (attackPowerMessageText == null)
        {
            return;
        }

        if (attackPowerMessageCoroutine != null)
        {
            StopCoroutine(attackPowerMessageCoroutine );
        }

        attackPowerMessageCoroutine = StartCoroutine( ShowAttackPowerMessageCoroutine(amount) );
    }

    private IEnumerator ShowAttackPowerMessageCoroutine(
        int amount)
    {
        attackPowerMessageText.gameObject.SetActive(true);

        attackPowerMessageText.text =
            $"攻撃力が{amount}上がった！";

        yield return new WaitForSeconds(2f);

        attackPowerMessageText.gameObject.SetActive(false);

        attackPowerMessageCoroutine = null;
    }

    public void ShowAutoFireUnlocked()
    {
        if (autoFireMessageText == null)
        {
            return;
        }

        if (autoFireMessageCoroutine != null)
        {
            StopCoroutine(autoFireMessageCoroutine);
        }

        autoFireMessageCoroutine =
            StartCoroutine(ShowAutoFireMessageCoroutine());
    }

    private IEnumerator ShowAutoFireMessageCoroutine()
    {
        autoFireMessageText.gameObject.SetActive(true);

        autoFireMessageText.text =
            "連射モード解禁！\n左クリック長押しで連続攻撃！";

        yield return new WaitForSeconds(3f);

        autoFireMessageText.gameObject.SetActive(false);

        autoFireMessageCoroutine = null;
    }

    public void ShowCountdown(string text)
    {
        if (countdownText == null)
        {
            return;
        }

        countdownText.gameObject.SetActive(true);
        countdownText.text = text;
    }

    public void HideCountdown()
    {
        if (countdownText == null)
        {
            return;
        }

        countdownText.gameObject.SetActive(false);
    }

    public void ShowResult(int totalDamageCost)
    {
        if (resultPanel != null)
        {
            resultPanel.SetActive(true);
        }

        if (resultDamageCostText != null)
        {
            resultDamageCostText.text =  $"最終損害賠償額\n¥{totalDamageCost:N0}";
        }
    }

    public void HideResult()
    {
        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }
    }
}