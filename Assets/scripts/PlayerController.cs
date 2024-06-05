using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 2f;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI winText;
    public float maxSpeed = 4f;
    public float victoryScore = 60;



    private uint score = 0;
    private Rigidbody rb;
    private float moveX;
    private float moveY;
    private Vector3 forceDir;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        winText.gameObject.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movement = movementValue.Get<Vector2>();
        moveX = movement.x;
        moveY = movement.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        forceDir.x = moveX;
        forceDir.z = moveY;
        Debug.LogFormat("player speed {0}", rb.velocity);

        // 限制速度，在各个轴速度不能超过maxSpeed
        Vector3 desiredVelocity = rb.velocity;
        desiredVelocity.x = Mathf.Clamp(desiredVelocity.x, -maxSpeed, maxSpeed);
        desiredVelocity.z = Mathf.Clamp(desiredVelocity.z, -maxSpeed, maxSpeed);
        rb.velocity = desiredVelocity;

        // 如果速度太小，就用冲量的方式提高起始加速度
        if (rb.velocity.magnitude < 2f)
        {
            rb.AddForce(forceDir * speed, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(forceDir * speed);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Gem"))
    //    {
    //        Destroy(collision.gameObject);
    //        Debug.LogFormat("collision gem, destroy it");
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gem"))
        {
            Destroy(other.gameObject);
            score += 10;
            scoreText.text = $"Score: {score}";
            if (score >= 60)
            {
                //winText.gameObject.SetActive(true);
                SceneManager.LoadScene("Finish");
            }
            Debug.LogFormat("collision gem, destroy it");
        }
    }
}
