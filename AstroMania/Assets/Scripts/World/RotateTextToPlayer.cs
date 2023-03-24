using UnityEngine;

public class RotateTextToPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject _lookAt;



    void Update()
    {
        transform.LookAt(_lookAt.transform);
        transform.rotation = Quaternion.LookRotation(FindAnyObjectByType<PlayerCamera>().gameObject.transform.forward);
    }
}
