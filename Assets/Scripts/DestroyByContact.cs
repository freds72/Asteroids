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

        DestroyObject(this.gameObject);
        if ( DestroyedPrefab != null)
            Instantiate(DestroyedPrefab, transform.position, transform.rotation);
    }
}
