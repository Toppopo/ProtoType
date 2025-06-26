using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerCam : MonoBehaviour
{
    [Header("ƒZƒ“ƒV")]
    [SerializeField] float sensX;
    [SerializeField] float sensY;

    [SerializeField] Transform orientation;

    [SerializeField] private GameObject cObj;

    private Vector3 dir;

    float xRotation;
    public float XRotation
    {
        get { return xRotation; }
        set { xRotation = value; }
    }
    float yRotation;
    public float YRotation
    {
        get { return yRotation; }
        set { yRotation = value; }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        dir = cObj.transform.position - transform.position;

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -30, 90);

        Ray ray = new Ray(transform.position, dir.normalized);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 5))
        {
            cObj.transform.position = hit.point;
        }
        else
        {
            cObj.transform.position = transform.position + dir.normalized * 5;
        }
        Debug.DrawRay(transform.position, dir.normalized * 5 , Color.red);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
