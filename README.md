# Team A - 2D Action Game

## 專案概述
這是一個使用Unity開發的2D動作遊戲，遊戲風格類似於吸血鬼倖存者(Vampire Survivors)。

## 技術規格

### 開發環境
- Unity Engine
- 使用新版輸入系統 (Input System Package)
- 使用Cinemachine相機系統
- TextMeshPro用於UI文字渲染

### 核心系統

#### 玩家系統
- 移動系統 (PlayerMovements)
- 玩家數值系統 (PlayerStats)
  - 生命值系統
  - 經驗值系統
  - 等級系統
- 攻擊系統 (PlayerAttackController)
  - 投射物攻擊
  - 可自定義攻擊範圍
  - 支持多重投射物

#### 敵人系統
- 敵人移動 (EnemyMovement)
  - 基於物理的移動系統
  - 追蹤玩家行為
- 敵人數值 (EnemyStats)
  - 可自定義敵人數值
  - 死亡系統

#### 戰鬥系統
- 物理基礎的碰撞檢測
- 傷害計算系統
- 經驗值掉落系統

#### 升級系統
- 等級提升機制
- 技能升級選項
- 自定義升級效果

#### UI系統
- 生命值顯示
- 經驗值進度條
- 等級顯示
- 升級選項介面

### 遊戲特色
1. 自動攻擊系統
2. 經驗值收集系統
3. 升級分支選擇
4. 物理基礎的移動系統

## 檔案結構
```
Assets/
├── Scripts/
│   ├── Player/
│   │   ├── PlayerMovements.cs    - 玩家移動控制
│   │   ├── PlayerStats.cs        - 玩家數值管理
│   │   └── PlayerAttackController.cs - 玩家攻擊系統
│   ├── Enemy/
│   │   ├── EnemyMovement.cs      - 敵人移動AI
│   │   └── EnemyStats.cs         - 敵人數值管理
│   ├── Pickups/
│   │   └── ExperienceOrb.cs      - 經驗值收集物件
│   └── UI/
│       ├── UIManager.cs          - UI總管理器
│       └── UpgradeButtonUI.cs    - 升級按鈕控制
├── Input Action/
│   └── PlayerControls.cs         - 玩家輸入控制
└── Scenes/                       - 遊戲場景
```

## 開發狀態
- [x] 基礎玩家控制系統
- [x] 基礎戰鬥系統
- [x] 經驗值系統
- [x] UI系統
- [x] 升級系統
- [x] 敵人AI系統