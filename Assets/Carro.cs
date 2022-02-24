using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carro : MonoBehaviour
{
    [Range(-1,1)]
    public float Axis;
    public float Sensibility = 1;
    public float maxAngle = 30;
    public float SidewaysSpeed = 1;
    public float maxZMovement = 0.32f;

    public LayerMask deathLayer;

    public bool morto = false;

    private Rigidbody rb;
    public bool TouchInputEnabled;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!morto)
        {
            bool turnLeft = Input.GetKey(KeyCode.LeftArrow);
            bool turnRight = Input.GetKey(KeyCode.RightArrow);

            if (Input.GetMouseButton(0) && TouchInputEnabled)
            {
                if(Input.mousePosition.x < Screen.width / 2)
                {
                    turnLeft = true;
                }
                else
                {
                    turnRight = true;
                }
            }


            if (turnLeft && !turnRight && transform.position.z-0.01f >-maxZMovement)
            {
                Axis = Mathf.Lerp(Axis, -1, Sensibility * Time.deltaTime);
            }
            else if (turnRight && !turnLeft && transform.position.z+ 0.01f < maxZMovement)
            {
                Axis = Mathf.Lerp(Axis, 1, Sensibility * Time.deltaTime);
            }
            else
            {
                Axis = Mathf.Lerp(Axis, 0, Sensibility * Time.deltaTime);
            }

            //rotação
            float anguloAtual = Axis * maxAngle;
            Vector3 rot = transform.rotation.eulerAngles;
            rot.y = anguloAtual;
            transform.rotation = Quaternion.Euler(rot);

            //posição
            Vector3 newPos = transform.position + new Vector3(0, 0, anguloAtual * SidewaysSpeed * Time.deltaTime);

            if (newPos.z > maxZMovement)
            {
                newPos.z = maxZMovement;
            }
            else if (newPos.z < -maxZMovement)
            {
                newPos.z = -maxZMovement;
            }

            transform.position = newPos;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.tag == "Obstaculo" && !morto)
        {
            Debug.Log("Morri");
            morto = true;
            FindObjectOfType<SceneRunner>().Parar();

            //rb.velocity = new Vector3(0.1f,0,0);

            rb.AddForce(( transform.up) * 60f );
            rb.AddTorque(new Vector3(0, 0, 60f));
   //         Debug.Log("Apply force");
            //GetComponent<Rigidbody>().useGravity = false;
            //GetComponent<Rigidbody>().AddExplosionForce(100f, collision.GetContact(0).point, 10f);

        }
    }

    public void EnableTouchInput()
    {
        TouchInputEnabled = true;
    }

}
