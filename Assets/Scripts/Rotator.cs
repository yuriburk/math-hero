using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(new Vector3(0, 155, 0) * Time.deltaTime);
    }
}
