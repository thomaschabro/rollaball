using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
 // Rigidbody of the player.
 private Rigidbody rb;
 private int count;
 private bool win;

 // Movement along X and Y axes.
 private float movementX;
 private float movementY;

 // Speed at which the player moves.
 public float speed = 0;
 public TextMeshProUGUI countText;
 public GameObject winTextObject;
 public GameObject winTimerTextObject;

 // Star images
   public GameObject emptyStar1;
   public GameObject emptyStar2;
   public GameObject emptyStar3;
   public GameObject fullStar1;
   public GameObject fullStar2;
   public GameObject fullStar3;

 // Timer variables
 [SerializeField] TextMeshProUGUI timerText;
 float elapsedTime ;
 public int minutes;
 public int seconds;
 public int milliseconds;

 // Sound effects
 public AudioSource audioSource, victorySource, damageSource;
   public AudioClip pickUpSound, victorySound, damageSound;

   // HealthManager
   public Image healthBar;
   public float health = 100f;

 // Start is called before the first frame update.
 void Start()
    {
 // Get and store the Rigidbody component attached to the player.
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
        winTimerTextObject.SetActive(false);
        win = false;

        // Set all stars to active false
         emptyStar1.SetActive(false);
         emptyStar2.SetActive(false);
         emptyStar3.SetActive(false);
         fullStar1.SetActive(false);
         fullStar2.SetActive(false);
         fullStar3.SetActive(false);
    }

 void Update()
    {
      if (!win) {
        elapsedTime += Time.deltaTime;
        minutes = Mathf.FloorToInt(elapsedTime / 60);
        seconds = Mathf.FloorToInt(elapsedTime - minutes * 60);
        milliseconds = Mathf.FloorToInt((elapsedTime - minutes * 60 - seconds) * 1000);
        timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
      }
    }

 // This function is called when a move input is detected.
 void OnMove(InputValue movementValue)
    {
 // Convert the input value into a Vector2 for movement.
        Vector2 movementVector = movementValue.Get<Vector2>();

 // Store the X and Y components of the movement.
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

 void SetCountText()
   {
       countText.text =  "Count: " + count.ToString() + "/7";
   }

 // FixedUpdate is called once per fixed frame-rate frame.
 private void FixedUpdate()
    {
 // Create a 3D movement vector using the X and Y inputs.
        Vector3 movement = new Vector3 (movementX, 0.0f, movementY);

 // Apply force to the Rigidbody to move the player.
        rb.AddForce(movement * speed);
    }

   void changeScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

   // If hit the wall
   void OnCollisionEnter(Collision collision)
   {
      if (collision.gameObject.CompareTag("Wall") && !win)
      {
         damageSource.clip = damageSound;
         damageSource.Play();

         health -= 20f;
         healthBar.fillAmount = health / 100f;

         if (health <= 0)
         {
            win = true;
            winTextObject.GetComponent<TextMeshProUGUI>().text = "Game Over!";
            winTextObject.GetComponent<TextMeshProUGUI>().color = Color.red;
            winTextObject.SetActive(true);
            countText.gameObject.SetActive(false);
            timerText.gameObject.SetActive(false);
            winTimerTextObject.GetComponent<TextMeshProUGUI>().text = "Your Time: " + string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
            winTimerTextObject.SetActive(true);

            Invoke("changeScene", 3f);
         }
      }
   }

   void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.CompareTag("PickUp"))
      {
         other.gameObject.SetActive(false);
         count = count + 1;
         SetCountText();

         if (count >= 7 && !win)
         {
            win = true;
            winTextObject.SetActive(true);
            countText.gameObject.SetActive(false);
            timerText.gameObject.SetActive(false);
            winTimerTextObject.GetComponent<TextMeshProUGUI>().text = "Your Time: " + string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
            winTimerTextObject.SetActive(true);

            // Set stars
            if ((int)seconds < 15)
            {
               fullStar1.SetActive(true);
               fullStar2.SetActive(true);
               fullStar3.SetActive(true);
            }
            else if ((int)seconds >= 15 && (int)seconds < 20)
            {
               fullStar1.SetActive(true);
               fullStar2.SetActive(true);
               emptyStar3.SetActive(true);
            }
            else if ((int)seconds >= 20 && (int)seconds < 25)
            {
               fullStar1.SetActive(true);
               emptyStar2.SetActive(true);
               emptyStar3.SetActive(true);
            }
            else
            {
               emptyStar1.SetActive(true);
               emptyStar2.SetActive(true);
               emptyStar3.SetActive(true);
            }

            victorySource.clip = victorySound;
            victorySource.Play();

            Invoke("changeScene", 3f);
         }
         else
         {
            audioSource.clip = pickUpSound;
            audioSource.Play();
         }
      }
   }
}