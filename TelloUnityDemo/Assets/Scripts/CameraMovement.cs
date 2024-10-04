using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f; // 旋轉速度

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // A, D 鍵
        float moveVertical = Input.GetAxis("Vertical"); // W, S 鍵
        float moveUpDown = 0f;

        // Z 鍵向上移動，X 鍵向下移動
        if (Input.GetKey(KeyCode.Z))
        {
            moveUpDown = 1f; // 向上
        }
        else if (Input.GetKey(KeyCode.X))
        {
            moveUpDown = -1f; // 向下
        }

        Vector3 movement = new Vector3(moveHorizontal, moveUpDown, moveVertical) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        // C 鍵向左旋轉，V 鍵向右旋轉
        if (Input.GetKey(KeyCode.C))
        {
            transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0); // 向左旋轉
        }
        else if (Input.GetKey(KeyCode.V))
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0); // 向右旋轉
        }
    }
}