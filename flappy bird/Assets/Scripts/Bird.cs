using UnityEngine;

public class Bird : MonoBehaviour
{

    private Rigidbody2D _bird;
    private Vector2 _initPos;
    private int _gravityScale = 0;
    void Start()
    {
        _bird = GetComponent<Rigidbody2D>();
        _bird.gravityScale = _gravityScale;
        _initPos = _bird.transform.position;
    }

    void Update()
    {
        if (!GameManager.Instance.gameStarted && !GameManager.Instance.playerDead)
        {
            Idle();
            return;
        }
        if (!GameManager.Instance.playerDead && Input.GetMouseButtonDown(0))
        {
            RiseUp();
        }
        else
        {
            FallDown();
        }
    }

    void RiseUp()
    {
        if (_gravityScale == 0)
        {
            _gravityScale = 2;
            GetComponent<Rigidbody2D>().gravityScale = _gravityScale;
        }
        _bird.transform.rotation = Quaternion.Euler(0, 0, 30);
        _bird.velocity = Vector2.zero;
        _bird.AddForce(Vector2.up * 320);
    }

    void FallDown()
    {
        if (_bird.velocity.y <= -5)
        {
            _bird.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(30, -90, (_bird.velocity.y + 5) / -5));       
        }
    }

    void Idle()
    {
        float pos = Mathf.Sin(Time.time * 10) * 0.2f;
        _bird.transform.position = _initPos + (Vector2.down * pos);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameManager.Instance.CurrentScore++;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Finish"))
        {
            if (other.gameObject.name.Equals("Floor"))
            {
                gameObject.GetComponent<Animator>().speed = 0f;
            }
            else
            {
                other.gameObject.transform.GetComponent<PolygonCollider2D>().enabled = false;
            }

            if (!GameManager.Instance.playerDead)
            {
                EndGameAndHandleControl();            
            }
        }
    }

    void EndGameAndHandleControl()
    {
        GameManager.Instance.EndGame();
    }
}
