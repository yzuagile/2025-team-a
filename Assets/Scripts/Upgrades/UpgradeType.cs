// Scripts/Upgrades/UpgradeType.cs (���������|)
public enum UpgradeType
{
    // ���a��¦�ݩ�
    IncreaseMaxHealth,
    IncreaseMovementSpeed,
    IncreasePickupRadius,
    IncreaseExperienceGain, // �W�[�g������ʤ���
    IncreaseArmor, // �W�[�@��/���

    // �Z���ݩ� (�i�H��Ӥ��A�Ҧp�w��S�w�Z��)
    IncreaseAttackDamage,
    IncreaseProjectileSpeed,
    DecreaseAttackInterval, // �W�[�����t�� (��֧������j)
    IncreaseProjectilesPerShot,
    IncreaseAttackRange,    // �W�[�Z������/���Ľd��
    IncreaseProjectilePierce, // �W�[�u����z����
    IncreaseCriticalHitChance,
    IncreaseCriticalHitDamage,
    IncreaseAreaOfEffect, // �W�[�ĪG�d�� (�Ҧp�Y�ǽd��Z��)

    // ��L�S���ĪG
    GainNewWeapon, // ����s�Z��
    EvolveWeapon, // �i�ƪZ��
    SpecialAbility_Dash,
    SpecialAbility_Shield,

    // (��h�A�Q�n������...)
    None // �N���L�Υ��w�q
}