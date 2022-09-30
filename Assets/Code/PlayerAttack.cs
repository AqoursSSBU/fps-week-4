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

    public static bool gunActive = true;
    public string nextLevelName;
    public bool key = false;

    AudioSource audio_source;
    public AudioClip coin_sound;
    public AudioClip gun_sound;
    public AudioClip level_up;
    public AudioClip gun_arm;

    private void Start() {
        if(SceneManager.GetActiveScene().name=="L1"){
            gunActive=false;
        }

        audio_source = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && gunActive)
        {
            audio_source.PlayOneShot(gun_sound);

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
                    reticle.color = Color.red;
            }else{
                reticle.color = Color.white;
            }
        }else{
            reticle.enabled=false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        switch(other.tag)
        {
            case "Coin":
                audio_source.PlayOneShot(coin_sound);
                Destroy(other.gameObject);
                break;
            case "Gun":
                audio_source.PlayOneShot(gun_arm);
                Destroy(other.gameObject);
                gunActive = true;
                break;
            case "Key":
                audio_source.PlayOneShot(coin_sound);
                Destroy(other.gameObject);
                key = true;
                break;
            case "Door":
                print("touching door");
                audio_source.PlayOneShot(level_up);
                if(key){
                    SceneManager.LoadScene(nextLevelName);
                }
                break;
            default:
                break;
        }

        // if (other.CompareTag("Coin"))
        // {
        //     Destroy(other.gameObject);
        // }
        // if (other.CompareTag("Gun"))
        // {
        //     Destroy(other.gameObject);
        //     gunActive = true;
        // }
        // if (other.CompareTag("Key"))
        // {
        //     Destroy(other.gameObject);
        //     key = true;
        // }
        // if (other.CompareTag("Door"))
        // {
        //     SceneManager.LoadScene(nextLevelName);
        // }

         

    }
}
