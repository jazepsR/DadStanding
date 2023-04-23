using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    public float moveSpeed = -0.3f;
    public AnimationCurve yCurve;
    public float yMultiplier;
    private float startY;
    public float rotationSpeed = 10;
    // Start is called before the first frame update
    void Start()
    {
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameState == GameState.Playing)
        {
            transform.position += Vector3.right * moveSpeed;
            transform.position = new Vector3(transform.position.x, startY + yCurve.Evaluate(1 - Mathf.Abs(transform.position.x / 10f)) * yMultiplier, transform.position.z);
            transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
        }
        if (GameManager.gameState == GameState.Fail || GameManager.gameState == GameState.Win)
            Destroy(gameObject);
    }
}
