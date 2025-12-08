using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    Rigidbody rb;
    public Rigidbody Rb
    {
        get {
            if(rb == null)
                rb = GetComponent<Rigidbody>();
            return rb;
        }
    }
    public void Shoot(Vector3 direction)
    {
        Rb.linearVelocity = direction * speed;
        Destroy(gameObject, 5f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
