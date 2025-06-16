// Scripts/Pickups/PowerUpType.cs
public enum PowerUpType
{
    // 臨時 Buffs
    AttackSpeedBuff,  // 攻擊速度提升 (減少攻擊間隔)
    AttackDamageBuff, // 攻擊力提升
    MovementSpeedBuff,// 移動速度提升

    // 立即生效類
    ScreenClearBomb,  // 清場炸彈
    Heal,             // (未來可加) 生命恢復
    Magnet,           // (未來可加) 磁鐵，吸附所有經驗球

    None
}