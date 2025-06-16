// Scripts/UI/UpgradeButtonUI.cs
using UnityEngine;
using UnityEngine.UI; // 需要 Image
using TMPro; // 需要 TextMeshProUGUI

public class UpgradeButtonUI : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text nameText;
    public TMP_Text descriptionText;

    private Button buttonComponent;
    private UpgradeOptionData currentOptionData;
    private UIManager uiManager;

    void Awake()
    {
        buttonComponent = GetComponent<Button>();
        if (buttonComponent == null)
        {
            return;
        }
        buttonComponent.onClick.RemoveAllListeners();
        buttonComponent.onClick.AddListener(OnButtonClicked);
        
        if (UIManager.instance != null)
        {
            uiManager = UIManager.instance;
        }
    }
    
    public void Setup(UpgradeOptionData optionData, PlayerStats playerStats)
    {
        currentOptionData = optionData;

        if (nameText != null)
            nameText.SetText(optionData.upgradeName);
        if (descriptionText != null)
            descriptionText.SetText(optionData.description);
        if (iconImage != null)
        {
            if (optionData.icon != null)
            {
                iconImage.sprite = optionData.icon;
                iconImage.enabled = true;
            }
            else
            {
                iconImage.enabled = false;
            }
        }

        if (UIManager.instance != null)
        {
            uiManager = UIManager.instance;
        }
    }

    void OnButtonClicked()
    {
        if (currentOptionData != null)
        {
            Debug.Log($"{currentOptionData.upgradeName}");
            PlayerStats playerStats = FindObjectOfType<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.ApplyUpgrade(currentOptionData);
            }
            
            if (uiManager != null)
            {
                uiManager.HideUpgradePanel();
            }
            else
            {
            }
        }
    }
}