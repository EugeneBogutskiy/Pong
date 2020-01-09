using System.Collections.Generic;
using UnityEngine;

public class Brain2 : MonoBehaviour
{
    // There are 6 Inputs
    //Ball X
    //Ball Y
    //Ball Velocity X
    //Ball Velocity Y
    //Paddle X
    //Paddle Y

    public GameObject paddle;
    public GameObject ball;
    Rigidbody2D brb; //ball rigid body
    float yvel; // output from NN
    float paddleMinY = -3.6f; // how far we want to be padddle to travel
    float paddleMaxY = 3.6f;
    float paddleMaxSpeed = 5;
    public float numSaved = 0;
    public float numMissed = 0;

    ANN ann;

    // Use this for initialization
    void Start()
    {
        ann = new ANN(6, 1, 1, 4, 0.11);
        brb = ball.GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// /
    /// </summary>
    /// <param name="bx">Ball X</param>
    /// <param name="by">Ball Y</param>
    /// <param name="bvx">Ball Velocity X</param>
    /// <param name="bvy">Ball Velocity Y</param>
    /// <param name="px">Paddle X</param>
    /// <param name="py">Paddle Y</param>
    /// <param name="pv">Paddle velocity</param>
    /// <param name="train or not">will train or not</param>
    /// <returns></returns>
    List<double> Run(double bx, double by, double bvx, double bvy, double px, double py, double pv, bool train)
    {
        List<double> inputs = new List<double>();
        List<double> outputs = new List<double>();
        inputs.Add(bx);
        inputs.Add(by);
        inputs.Add(bvx);
        inputs.Add(bvy);
        inputs.Add(px);
        inputs.Add(py);
        outputs.Add(pv);
        if (train)
            return (ann.Train(inputs, outputs));
        else
            return (ann.CalcOutput(inputs, outputs));
    }

    // Update is called once per frame
    void Update()
    {
        float posy = Mathf.Clamp(paddle.transform.position.y + (yvel * Time.deltaTime * paddleMaxSpeed),
                                 paddleMinY, paddleMaxY); // calculate Y for Paddle
        paddle.transform.position = new Vector3(paddle.transform.position.x, posy, paddle.transform.position.z); // here we move the Paddle

        List<double> output = new List<double>();
        int layerMask = 1 << 9; // here we are getting layer #9 from layer list ib Unity and then we use it in hit
        RaycastHit2D hit = Physics2D.Raycast(ball.transform.position, brb.velocity, 1000, layerMask);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "tops") //reflect off top
            {
                Vector3 reflection = Vector3.Reflect(brb.velocity, hit.normal);
                hit = Physics2D.Raycast(hit.point, reflection, 1000, layerMask);
            }

            if (hit.collider != null && hit.collider.gameObject.tag == "backwall")
            {
                float dy = (hit.point.y - paddle.transform.position.y); // delta Y between hit coordinate and paddle coordinate

                output = Run(ball.transform.position.x,
                                    ball.transform.position.y,
                                    brb.velocity.x, brb.velocity.y,
                                    paddle.transform.position.x,
                                    paddle.transform.position.y,
                                    dy, true); //true if we want to train our NN
                yvel = (float)output[0];
            }

        }
        else
            yvel = 0;
    }
}
