using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour
{
    public string IgnoreTag = null;
    public GameObject DestroyedPrefab;
    
    void DoCollision(GameObject collider)
    {
        if (collider.tag.Equals(IgnoreTag))
            return;

        DestroyObject(this.gameObject);
        if (DestroyedPrefab != null)
            Instantiate(DestroyedPrefab, transform.position, transform.rotation);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        DoCollision(col.collider.gameObject);
    }

    void OnCollisionEnter(Collision col)
    {
        DoCollision(col.collider.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(string.Format("Trigger: {0}", other.gameObject));
        DoCollision(other.gameObject);
    }

}
