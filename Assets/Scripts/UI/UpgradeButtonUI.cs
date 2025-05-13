// Scripts/UI/UpgradeButtonUI.cs
using UnityEngine;
using UnityEngine.UI; // �ݭn Image
using TMPro; // �ݭn TextMeshProUGUI

public class UpgradeButtonUI : MonoBehaviour
{
    public Image iconImage; // ���s�W���ϥ� (�i��)
    public TMP_Text nameText;    // ��ܤɯŦW��
    public TMP_Text descriptionText; // ��ܤɯŴy�z

    private Button buttonComponent;
    private UpgradeOptionData currentOptionData;
    private UIManager uiManager; // �Ψӳq���������O

    void Awake()
    {
        buttonComponent = GetComponent<Button>();
        if (buttonComponent == null)
        {
            Debug.LogError("UpgradeButtonUI: �䤣�� Button �ե�I");
            return;
        }
        // �����Ҧ��s�边�ɴ��K�[����ť�� (�H���U�@)
        buttonComponent.onClick.RemoveAllListeners();
        // �K�[�{���X�����ť��
        buttonComponent.onClick.AddListener(OnButtonClicked);

        // �d�� UIManager (�]�i�H�b Setup �ɶǤJ)
        if (UIManager.instance != null)
        {
            uiManager = UIManager.instance;
        }
    }

    // �� UIManager �I�s�ӳ]�w���s��ܪ����e�M�I������檺�ﶵ
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
                iconImage.enabled = false; // �p�G�S���ϥܫh���� Image �ե�
            }
        }

        if (UIManager.instance != null)
        {
            uiManager = UIManager.instance;
        }
        // (���n) �N�I���ƥ�P���Τɯ��޿����p�_��
        // �o�̧ڭ̤��b AddListener ���������ΡA�ӬO�����s�Q�I���ɡA
        // �I�s UIManager ����k�A�ç��ܪ��ƾڶǦ^�h�A�� UIManager �q�� PlayerStats�C
        // �Ϊ̧󪽱��G�� PlayerStats ���S���Τ�k�A�ѳo�̩I�s�C
        // ���F�t�ܡA�o�̧ڭ̥����]�I���᪽���I�s PlayerStats �� ApplyUpgrade ��k
    }

    void OnButtonClicked()
    {
        if (currentOptionData != null)
        {
            Debug.Log($"��ܤF�ɯ�: {currentOptionData.upgradeName}");
            // ��� PlayerStats (�o�̰��] PlayerStats ���@���R�A��ҩγq�L GameManager ���)
            PlayerStats playerStats = FindObjectOfType<PlayerStats>(); // ��ڶ��ؤ����u�Ʀ��d��
            if (playerStats != null)
            {
                playerStats.ApplyUpgrade(currentOptionData); // <--- PlayerStats �ݭn�o�Ӥ�k
            }

            // �q�� UIManager �������O (�p�G UIManager �Q���)
            if (uiManager != null)
            {
                uiManager.HideUpgradePanel();
            }
            else
            {
                Debug.LogWarning("UpgradeButtonUI: ����� UIManager�A�L�k�۰������ɯŭ��O�C");
                // �p�G�O�o�ر��p�A�i�H�Ҽ{���ɯŭ��O�ۤv��ť�Y�Өƥ������
            }
        }
    }
}