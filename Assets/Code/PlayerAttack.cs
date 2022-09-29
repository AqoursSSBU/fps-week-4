using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerAttack : MonoBehaviour
{
    private float raycastDist = 50;
    public LayerMask enemyLayer;
    public Transform camTrans;
    public Image reticle;
    private bool reticleTarget = false;
    public bool gunActive= true;


    private void Start() {
        if(SceneManager.GetActiveScene().name=="L1"){
            gunActive=false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && gunActive)
        {
            RaycastHit hit;
            if (Physics.Raycast(camTrans.position, camTrans.forward, out hit, raycastDist, enemyLayer))
            {
                GameObject enemy = hit.collider.gameObject;
                //destroy
                if(enemy.CompareTag("Monster"))
                {
                    Destroy(enemy);
                }  
                //push back
                else if(enemy.CompareTag("Target"))
                {
                    Rigidbody enemyRB = enemy.GetComponent<Rigidbody>();
                    enemyRB.AddForce(transform.forward * 800 + Vector3.up * 200);
                    enemyRB.AddTorque(new Vector3(Random.Range(-50,50), Random.Range(-50,50), Random.Range(-50,50)));
                }
                else{
                    print(enemy.tag);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        print("gun " + gunActive);
        if(gunActive){
            reticle.enabled=true;
            RaycastHit hit;
            if (Physics.Raycast(camTrans.position, camTrans.forward, out hit, raycastDist) &&
                (hit.collider.CompareTag("Target") || hit.collider.CompareTag("Monster")))
            {
                if (!reticleTarget)
                {
                    reticle.color = Color.red;
                    reticleTarget = true;
                }
                else if (reticleTarget)
                {
                    reticle.color = Color.white;
                    reticleTarget = false;
                }
            }
        }else{
            reticle.enabled=true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Gun"))
        {
            Destroy(other.gameObject);
            gunActive=true;
        }
    }
}
