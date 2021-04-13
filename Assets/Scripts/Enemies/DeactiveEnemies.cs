using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactiveEnemies : MonoBehaviour
{

    private int distanceFromPlayer;

    private GameObject player;

    private GameObject manager;

    private CheckPosition checkPosition;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");

        manager = GameObject.Find("Main Camera");

        checkPosition = manager.GetComponent<CheckPosition>();

        distanceFromPlayer = checkPosition.distanceFromPlayer;
    }


    // Update is called once per frame
    void Update()
    {
        if (checkPosition == null)
        {
            checkPosition = manager.GetComponent<CheckPosition>();
        }
        else
        {
            if (player == null)
            {
                player = GameObject.FindWithTag("Player");
            }
            else
            {

                if (Vector3.Distance(player.transform.position, gameObject.transform.position) > distanceFromPlayer)
                {
                    checkPosition.AddToList(this.gameObject);
                    gameObject.SetActive(false);
                }

            }
        }
    }
}

