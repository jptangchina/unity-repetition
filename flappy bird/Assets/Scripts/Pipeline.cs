using UnityEngine;

public class Pipeline : MonoBehaviour
{
    public float speed = -2;

    void Update()
    {
        if (GameManager.Instance.gameStarted)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            if (transform.position.x < -5)
            {
                Destroy(gameObject);
            }
        }
    }
}
