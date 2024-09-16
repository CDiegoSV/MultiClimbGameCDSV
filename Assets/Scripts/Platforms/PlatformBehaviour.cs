using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBehaviour : MonoBehaviour
{
    #region Knobs

    [SerializeField] float platformSpeed;

    #endregion

    #region References

    Rigidbody2D rb2D;

    #endregion

    #region Unity Methods

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        Destroy(gameObject, 11f);
    }

    private void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + platformSpeed * Time.deltaTime * Vector2.down);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(transform, true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(null, true);
        }
    }
    #endregion
}
