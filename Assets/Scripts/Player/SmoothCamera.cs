using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    [SerializeField] private float _followSpeed;
    [SerializeField] private Transform _target;
    
    private void FixedUpdate()
    {
        Vector3 updatedPosition = new Vector3(_target.position.x, _target.position.y, -10f);
        transform.position = Vector3.Slerp(transform.position, updatedPosition, _followSpeed * Time.deltaTime);
    }
}
