using UnityEngine;
using System.Collections;

public class MovingObject : MonoBehaviour
{

    public float speed = 5f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0);
    }
}
