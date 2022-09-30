using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    public int xSpeed = 0;
    public int ySpeed = 0;
    public int zSpeed = 0;
    void Update()
    {
        transform.Rotate(xSpeed*Time.deltaTime, ySpeed * Time.deltaTime, zSpeed * Time.deltaTime, Space.World);
    }
}
