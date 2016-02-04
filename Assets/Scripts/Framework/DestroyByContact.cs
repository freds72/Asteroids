using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour
{
    public string IgnoreTag = null;
    public GameObject DestroyedPrefab;
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag.Equals(IgnoreTag))
            return;

        Debug.Log(string.Format("Collider: {0}", col.collider.tag));
        DestroyObject(this.gameObject);
        if ( DestroyedPrefab != null)
            Instantiate(DestroyedPrefab, transform.position, transform.rotation);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag.Equals(IgnoreTag))
            return;

        Debug.Log(string.Format("Collider: {0}", col.collider.tag));
        Destroy(gameObject);
        if (DestroyedPrefab != null)
            Instantiate(DestroyedPrefab, transform.position, transform.rotation);
    }
}
