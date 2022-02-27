
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float speed;
    public float rotation;
    public LayerMask raycastMask;
    private float[] input = new float[5];
    public NeuralNetwork network;

    public int position;
    public bool collided;

    void FixedUpdate()
    {
        if (!collided)
        {
            for (int i=0; i<5; i++)
            {
                Vector3 newVector = Quaternion.AngleAxis(i * 45 - 90, new Vector3(0, 1, 0)) * transform.right;
                RaycastHit hit;
                Ray Ray = new Ray(transform.position, newVector);

                if (Physics.Raycast(Ray, out hit, 1, raycastMask))
                {

                    UnityEngine.Debug.Log(hit.distance);
                    UnityEngine.Debug.DrawLine(Ray.origin, hit.point, Color.red);
                    input[i] = (10 - hit.distance) / 10;
                    
                }
                else
                {
                    input[i] = 0;
                }
            }

            float[] output = network.FeedForward(input);
            transform.Rotate(0, output[0] * rotation, 0, Space.World);
            transform.position += this.transform.right * output[1] * speed;

        }
    }


    void OnCollisionEnter(Collision collision)
    {
        transform.Rotate(0, 180, 0, Space.World);
    }
}
