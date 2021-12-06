using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedCollectible : MonoBehaviour
{
    public AudioClip collectedClip;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            if(controller.speed < 3.0f)
            {
                controller.speed = 3.0f;
                Destroy(gameObject);
                controller.PlaySound(collectedClip);
            }
        }
    }
}
