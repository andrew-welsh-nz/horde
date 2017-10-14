using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine.UI;

public class ZombieController : MonoBehaviour {

    [SerializeField]
    Rigidbody head;

    public GameObject TargetObject;
    NavMeshAgent agent;

    [SerializeField]
    float health = 50.0f;

    public EnemySpawnControl controller;

    Animator anim;

    Vector3 lastArrow;
    bool headImpulseDone = false;
    bool IsDead = false;

    private float StoppingDistance = 2.5f;
    private int StuckArrows = 0;

    private bool IsInAttackRange = false;
    private bool IsCoolingDown = false;
    private float TimeSinceLastAttack = 0.0f;

    public float speed;

    //Arrow drop chance
    public float ArrowChance = 0.1f;
    bool GivenArrow = false;

    // Use this for initialization
    void Start () {
        if (TargetObject == null) {
            TargetObject = GameObject.FindGameObjectWithTag("Player");
        }

        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        
        //Get zombie's animator
        anim = gameObject.transform.GetChild(0).GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update (){

        if (health <= 0.0f)
        {
            if (!IsDead)
            {
                TargetObject.GetComponent<PlayerController>().CurrentScore += 5.0f;
                controller.TotalDead++;
                controller.WaveDead++;
                IsDead = true;
            }
            
            //Collect Arrows
            TargetObject.GetComponent<shooting>().ArrowCount += StuckArrows;
            StuckArrows = 0;

            //Flash Arrow Counter
            TargetObject.GetComponent<shooting>().ArrowCountFlash();

            //Play death animation
            anim.SetTrigger("death");

            //Trigger delay
            StartCoroutine(death());

            //Stop walking
            agent.enabled = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;

            //Stop Attacking
            IsInAttackRange = false;

            if (!headImpulseDone)
            {
                head.AddForce(lastArrow * -100f, ForceMode.Impulse);
                headImpulseDone = true;
            }
        }
        else
        {
            //If not dead, stop walking
            anim.SetFloat("Speed", 1);
            agent.destination = TargetObject.transform.position;
            //If its not within the stopping distance, continue foward.
            if (Vector3.SqrMagnitude(gameObject.transform.position - TargetObject.transform.position) <= StoppingDistance * StoppingDistance) {
                agent.speed = 0.0f;
                IsInAttackRange = true;
            }
        }

        

        //Attacking
        if (IsInAttackRange) {
            if (!IsCoolingDown){
                //Attack
                IsCoolingDown = true;
                anim.SetTrigger("attack");
                if (TargetObject.GetComponent<shooting>().ArrowCount > 0){
                    TargetObject.GetComponent<shooting>().ArrowCount--;
                    //GameObject.Find("Player/ArrowBreak").GetComponent<Animator>().SetTrigger("break");
                }
                else
                {
                    //Game Over
                    TargetObject.GetComponent<PlayerController>().StartCoroutine("GameOver");
                }

            }
            else {
                TimeSinceLastAttack += Time.deltaTime;
            }

            if (TimeSinceLastAttack >= 1.5f) {
                TimeSinceLastAttack = 0.0f;
                IsCoolingDown = false;
            }
        }
    }

    IEnumerator death()
    {
        //Enabling zombie head
        gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
        gameObject.transform.GetChild(1).GetComponent<Rigidbody>().isKinematic = false;
        gameObject.transform.GetChild(1).GetComponent<BoxCollider>().enabled = true;

        //Disabling collision and letting the zombie fall through earth
        GetComponent<CapsuleCollider>().isTrigger = true;
        GetComponent<Rigidbody>().AddForce(transform.up * -15);

        //If random number within chance, give arrow
        if (GivenArrow == false)
        {
            GivenArrow = true;

            if (Random.value <= (ArrowChance))
            {
                TargetObject.GetComponent<shooting>().ArrowCount += 1;
               // Debug.Log("Arrow Drop!");
            }
            else
            {
                //Debug.Log("No Drop!");
            }
        }
        

        //Destroying object after 5 seconds
        yield return new WaitForSeconds(5);

        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
       // Debug.Log("Is colliding with: "+ collision.gameObject.name);

        if(collision.gameObject.tag == "Arrow")
        {
            lastArrow = gameObject.transform.forward;

            // Remove the charge of the arrow from the health of the zombie
            health -= collision.gameObject.GetComponent<Arrow>().charge;
            gameObject.transform.GetChild(1).GetComponent<Rigidbody>().AddForce(agent.transform.forward * -20.0f);

            //If Its not a peircing arrow, stick to the enemy and make it the arrows parent (so Destroying works with the models)
            if (collision.gameObject.GetComponent<Arrow>().charge < 100) {
                StuckArrows++;

                //Stops the arrow from moving / Dealing damage
                collision.gameObject.GetComponent<Arrow>().charge = 0;

                collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                collision.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
                
                //Making arrow attach to head
                collision.gameObject.transform.SetParent(this.transform.GetChild(1).transform);
                collision.gameObject.transform.localPosition = new Vector3(0, 0, 2);
                collision.gameObject.transform.LookAt(this.transform.GetChild(1).transform);

                // collision.transform.position += new Vector3(0.0f, 1.8f, 0.0f);
                collision.transform.position += collision.gameObject.transform.forward * 1.0f;
            }

            //Emit particle
            var emitParams = new ParticleSystem.EmitParams();
            emitParams.startLifetime = 1.5f;
            this.transform.Find("PS_Enemy").GetComponent<ParticleSystem>().Emit(emitParams, Random.Range(3, 7));
        }
    }
}
