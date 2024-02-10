using UnityEngine;
public class CameraScaler : MonoBehaviour
{
    private const float CameraOffset = -10f;
    [SerializeField] private Camera camera;

    public void SetCameraPosition(float x, float y)
    {
        transform.position = new Vector3((x-1) / 2, (y-1) / 2, CameraOffset);
        camera.orthographicSize = x >= y ? x : x + 1.5f;
    }
}