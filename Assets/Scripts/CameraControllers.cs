using UnityEngine;

public class CameraControllers : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 pos;

    private void Awake()
    {
        if (!player)
        {
            player = FindObjectOfType<UnitRoot>().transform;
        }
    }

    private void Update()
    {
        pos = player.position;
        pos.z = -10f;
        transform.position= Vector3.Lerp(transform.position, pos, Time.deltaTime);
    }



}
