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
    public RawImage keyImg;

    public static bool gunActive =true;
    public string nextLevelName;
    public bool key = false;

    AudioSource audio_source;
    public AudioClip coin_sound;
    public AudioClip gun_sound;
    public AudioClip level_up;
    public AudioClip gun_arm;
    public Color colorChange;
    public int coins=0;
    public static int coinTotal;
    public int coinLevelTotal;
    
    public TMPro.TextMeshProUGUI before;
    public TMPro.TextMeshProUGUI after;

    public bool triggered = false;
    int hp_hit_count = 0;

    private void Start() {
        if(SceneManager.GetActiveScene().name=="L1"){
            gunActive=false;
            coinTotal=0;
        }

        audio_source = GetComponent<AudioSource>();
        coinLevelTotal = GameObject.FindGameObjectsWithTag("Coin").GetLength(0);
        after.text = coinLevelTotal.ToString();
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
        if (key){
            keyImg.CrossFadeAlpha(1,0.2f,false);
        }
        else{
            keyImg.CrossFadeAlpha(0.3f,0.2f,false);
        }
        if(Input.GetKey(KeyCode.Escape)){
            coins=0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            if(SceneManager.GetActiveScene().name=="L1"){
                gunActive=false;
            }
        }
        before.text = coins.ToString();
        
    }

    private void FixedUpdate()
    {
        
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
        triggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //prevents trigger activating twice in a row
        if(triggered){
            return;
        }
        triggered = true;
        switch(other.tag)
        {
            case "Coin":
                audio_source.PlayOneShot(coin_sound);
                Destroy(other.gameObject);
                coins+=1;
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
                audio_source.PlayOneShot(level_up);
                if(key){
                    SceneManager.LoadScene(nextLevelName);
                    coinTotal+=coins;
                }
                break;
            case "Monster":
                coins=0;
                hp_hit_count += 1;
                print("hit a zombie");
                print(hp_hit_count);
                if (hp_hit_count == 4)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                if(SceneManager.GetActiveScene().name=="L1"){
                    gunActive=false;
                }
                break;
            default:
                break;
        }

    }
}
