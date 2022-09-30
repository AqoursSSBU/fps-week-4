using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathBorder : MonoBehaviour
{
    public float deathLevel = -5;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y < deathLevel)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
