using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerAttack : MonoBehaviour
{
    private float raycastDist = 50;
    public LayerMask enemyLayer;
    public Transform camTrans;
    public Image reticle;
    public RawImage keyImg;

    public static bool gunActive;
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
    public TextMeshProUGUI coinsCollected;
    public TextMeshProUGUI gameMode;
    
    public TMPro.TextMeshProUGUI before;
    public TMPro.TextMeshProUGUI after;
    public double time;
    public double currentTime;
    public bool triggered = false;
    public float health = 100;
    public float maxHealth = 100;
    public HP healthBar;
    int hp_hit_count = 0;

    public static string levelName;

    private void Start() {
        if(SceneManager.GetActiveScene().name=="L1"){
            gunActive=false;
            coinTotal=0;
            levelName="L1";
        }

        if(SceneManager.GetActiveScene().name == "EndScreen")
        {
            Endscreen();
        }else{

            audio_source = GetComponent<AudioSource>();
            coinLevelTotal = GameObject.FindGameObjectsWithTag("Coin").GetLength(0);
            after.text = coinLevelTotal.ToString();
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name != "EndScreen"){
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

            if (Input.GetKey(KeyCode.Backspace))
            {
                Application.Quit();
            }

            before.text = coins.ToString();
            print(coinTotal+ " coins");
            currentTime+=Time.deltaTime;
            if(levelName!=SceneManager.GetActiveScene().name){
                if(SceneManager.GetActiveScene().name!="L1"){
                    audio_source.PlayOneShot(level_up);
                }
                levelName=SceneManager.GetActiveScene().name;
            }
        }
    }

    private void FixedUpdate()
    {
        if(SceneManager.GetActiveScene().name != "EndScreen"){
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
                time=currentTime;
                if(key){
                    SceneManager.LoadScene(nextLevelName);
                    coinTotal+=coins;
                }
                break;
            case "Monster":
                
                hp_hit_count += 1;
                print("hit a zombie");
                print(hp_hit_count);

                health -= 25;
                healthBar.UpdateHealthBar();

                if (hp_hit_count == 4)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    coins=0;
                }
                if(SceneManager.GetActiveScene().name=="L1"){
                    gunActive=false;
                }
                break;
            default:
                break;
        }

    }

    void Endscreen()
    {
        coinsCollected.text = "Coins Collected: " + coinTotal+"/"+"31";
        if (gunActive)
        {
            gameMode.text = "Gun Run";
        }
        else
        {
            gameMode.text = "No Gun Run";
        }
    }
}
