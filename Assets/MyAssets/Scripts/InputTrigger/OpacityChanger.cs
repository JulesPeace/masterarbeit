using UnityEngine;
public class OpacityChanger : MonoBehaviour
{
    public Material target;
    public Transform transform;
    public void UpdateOpacity(float alphaValue)
    {
        Vector3 trans = transform.position;
        trans.x += alphaValue / 4;
        trans.y += alphaValue / 4;
        trans.z += alphaValue / 4;
        transform.position = trans;

    }
}