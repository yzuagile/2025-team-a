// Scripts/Upgrades/UpgradeOptionData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "New UpgradeOption", menuName = "Upgrades/Upgrade Option Data", order = 52)]
public class UpgradeOptionData : ScriptableObject
{
    [Header("��¦��T")]
    public string upgradeName = "�s�ɯ�"; // ��ܦb���s�W���W��
    [TextArea(3, 5)] // ���y�z���b Inspector ���i�H�h���J
    public string description = "�ɯŮĪG�y�z�C"; // ��ԲӪ��y�z
    public Sprite icon; // (�i��) �ɯſﶵ���ϥ�

    [Header("�ɯŮĪG")]
    public UpgradeType upgradeType = UpgradeType.None; // �ϥΤW�����T�|
    public float value1; // �D�n���ƭ� (�Ҧp�G�ˮ`�W�[�q�B�t�׼W�[�q)
    // public float value2; // (�i��) ���n�ƭ� (�Ҧp�G����ɶ��B�B�~�ĪG���v)
    // public GameObject associatedPrefab; // (�i��) �p�G�O����s�Z��/�ޯ�A�i��ݭn Prefab
    // public int requiredPlayerLevel; // (�i��) ���ɯſﶵ�X�{�һݪ��̧C���a����

    [Header("�v�� (�Ω��H�����)")]
    [Range(1, 100)]
    public int selectionWeight = 10; // �V�j�V�e���Q�襤

    // (�i��) �S�w��ɯ����������[�ƾ�
    // public WeaponData newWeaponToGrant; // �p�G�O UpgradeType.GainNewWeapon
}