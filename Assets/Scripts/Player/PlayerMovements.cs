using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovements : MonoBehaviour
{
    public float movementSpeed = 5f;

    public float minX = -25;
    public float maxX = 25;
    public float minY = -20;
    public float maxY = 30;

    private Rigidbody2D rb;
    private Vector2 movementInput;
    private Animator animator;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

        if (movementInput.x != 0)
        {
            // 設定角色的 Y 軸旋轉為 180 或 0 度（取決於左右移動）
            float rotationY = movementInput.x > 0 ? 0 : 180;  // 向右時 rotationY = 0，向左時 rotationY = 180
            transform.rotation = Quaternion.Euler(0, rotationY, 0); // 設定新的旋轉
        }



        Vector2 displacement = movementInput.normalized * movementSpeed * Time.fixedDeltaTime;
        Vector2 targetPosition = rb.position + displacement;
        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
        animator.SetFloat("Speed", movementInput.sqrMagnitude);
        rb.MovePosition(targetPosition);
    }

    void OnMove(InputValue value)
    {
        // Debug.Log("onMove");
        movementInput = value.Get<Vector2>();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green; // 設定 Gizmo 顏色
        // 計算邊界框的中心點和大小
        float width = maxX - minX + 1;
        float height = maxY - minY + 1;
        Vector3 center = new Vector3(minX + width / 2, minY + height / 2, 0);
        Vector3 size = new Vector3(width, height, 0);
        // 繪製線框
        Gizmos.DrawWireCube(center, size);
    }
}
