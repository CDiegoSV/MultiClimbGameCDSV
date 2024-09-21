using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    #region Unity Methods

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            VSGameManager.instance.DecreaseAlivePlayersForAllPlayers();
        }
    }

    #endregion
}
