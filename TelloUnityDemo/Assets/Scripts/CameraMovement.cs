using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f; // ����t��

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // A, D ��
        float moveVertical = Input.GetAxis("Vertical"); // W, S ��
        float moveUpDown = 0f;

        // Z ��V�W���ʡAX ��V�U����
        if (Input.GetKey(KeyCode.Z))
        {
            moveUpDown = 1f; // �V�W
        }
        else if (Input.GetKey(KeyCode.X))
        {
            moveUpDown = -1f; // �V�U
        }

        Vector3 movement = new Vector3(moveHorizontal, moveUpDown, moveVertical) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        // C ��V������AV ��V�k����
        if (Input.GetKey(KeyCode.C))
        {
            transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0); // �V������
        }
        else if (Input.GetKey(KeyCode.V))
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0); // �V�k����
        }
    }
}