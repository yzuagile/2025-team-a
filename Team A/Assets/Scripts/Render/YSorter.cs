using UnityEngine;

// 附加到所有需要進行 Y 軸動態排序的物件上
[RequireComponent(typeof(Renderer))] // 確保有渲染組件
public class YSorter : MonoBehaviour
{
    // Y 軸乘數，拉大 Order 間距，避免 Y 值相近導致排序錯誤。可調。
    public int sortingOrderMultiplier = 10;
    // 可選基礎偏移，整體調整 Order 範圍。
    public int sortingOrderOffset = 0;
    // 關鍵：如果物件視覺底部非 transform.position.y (Pivot不在底部)，設置此垂直偏移。
    public float pivotOffsetY = 0f;

    private Renderer objectRenderer;

    void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer == null)
        {
            Debug.LogError("YSorter requires a Renderer component!", this);
            enabled = false;
        }
        // 可以在這裡強制設置 Sorting Layer 名稱，確保所有物件都在同一層
        // objectRenderer.sortingLayerName = "YourDynamicSortLayerName";
    }

    // 在 LateUpdate 中執行，確保位置已更新
    void LateUpdate()
    {
        if (objectRenderer == null) return;

        // 計算排序依據的 Y 座標
        float sortY = transform.position.y + pivotOffsetY;

        // 核心計算：Y 越小 -> Order 越大 -> 越靠前渲染
        objectRenderer.sortingOrder = -(Mathf.RoundToInt(sortY * sortingOrderMultiplier)) + sortingOrderOffset;
    }
}