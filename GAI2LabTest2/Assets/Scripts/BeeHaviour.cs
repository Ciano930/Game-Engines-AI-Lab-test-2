using UnityEngine;
using System.Collections;

public class BeeHaviour : MonoBehaviour {

    public int pollen;
    public int pollenMax;
    public float waitForPollen;
    public float beeRadius;
    public float pollenCollect;
    public float pollenDeposit;
    public float scale;
    public float mass;
    public float speedMax;
    public float lifeSpan;
    public GameObject targetFlower;
    public GameObject hive;
    public Vector3 targetPos;
    public Vector3 velocity;
    public Vector3 force;

    public bool SeekPoint;
    public bool PursueFlower;
    public bool PursueHive;
    public bool Alive;
    public bool GatherPollen;

	// Use this for initialization
	void Start () {
        pollen = 0;
        pollenMax = 5;
        pollenCollect = 1f;
        pollenDeposit = 0.2f;
        speedMax = 2;
        mass = 1;//starts at one but will change depending on size
        waitForPollen = 20f;
        beeRadius = 12;

    }

    Vector3 beeNextLocationTarget()
    {
        return transform.position + new Vector3(Random.Range(-beeRadius, beeRadius), -0.5f, Random.Range(-beeRadius, beeRadius));
    }

    //Beehaviors
    void Seek(Vector3 targetPos, float step)
    {
        Vector3 newPos = Vector3.MoveTowards(transform.position, targetPos, step);
        //Now we can rotate the bee to face the flower

        Vector3 targetDir = targetPos - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);

        //Now we make the bee rotate
        transform.rotation = Quaternion.LookRotation(newDir);
        //And finally we move toward the point
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
    }


    //The searching script for finding flowers
    void findFlowers()
    {
        GameObject[] flowers = GameObject.FindGameObjectsWithTag("flower");
        for( int i = 0; i < flowers.Length; i++)
        {
            Vector3 flowTarget = flowers[i].GetComponent<FlowerState>().transform.position - transform.position;
            if (flowTarget.magnitude < 20 && !flowers[i].GetComponent<FlowerState>().found)
            {
                //Now we have a flower target in range!
                targetFlower = flowers[i];
                targetFlower.GetComponent<FlowerState>().found = true;
                if (targetFlower.GetComponent<FlowerState>().pollen > 0)//Checks if the flower has pollen to collect
                {
                    SeekPoint = false;
                    PursueFlower = true;
                }
            }
            waitForPollen -= Time.deltaTime;//count down how long the bee will wait
        }
        if(waitForPollen < 0)
        {
            targetPos = hive.GetComponent<HiveState>().beeNextLocationTarget();
        }
    }

    void depositPollen()
    {
        GameObject hive = GameObject.FindGameObjectWithTag("hive");
        Vector3 hiveTarget = targetPos - transform.position;
        if(hiveTarget.magnitude < 2)
        {
            pollenDeposit -= Time.deltaTime;
            if (pollenDeposit < 0)
            {
                pollen -= 1;// remove pollen from the bee
                hive.GetComponent<HiveState>().pollen += 1;//add pollen to the hive
                pollenDeposit = 0.5f;//resets to a second
            }
        }
        if(pollen < 1)
        {
            SeekPoint = true;
        }
    }

    void collectPollen()
    {
        Vector3 flowTarget = targetFlower.transform.position - transform.position;
        if (flowTarget.magnitude < 1)//checks if within flower range
        {
            pollenCollect -= Time.deltaTime;
            if (pollenCollect < 0)
            {
                pollen += 1;
                targetFlower.GetComponent<FlowerState>().pollen -= 1;//The bees take the pollen
                pollenCollect = 1;//resets to a second
            }
            if (targetFlower.GetComponent<FlowerState>().pollen == 0)
            {
                //sets our state to find hive
                SeekPoint = false;
                PursueFlower = false;
                PursueHive = true;
            }
        }
    }

    void die()
    {
        Debug.Log("I'm dead!");
        //Kills the bee
        //When a bee with pollen dies a flower blooms in its place
        Vector3 flowerPos = new Vector3(transform.position.x, -2.5f, transform.position.z);
        GameObject GameManager = GameObject.FindGameObjectWithTag("game");
        GameManager.GetComponent<GameManager>().generateFlower(flowerPos);
        Destroy(this.gameObject);
    }

	// Update is called once per frame
	void Update () {
        float step = speedMax * Time.deltaTime;
        if(SeekPoint)
        {
            Seek(targetPos, step);
            findFlowers();//The beehaviour to find flower will be called while seeking
        }
        if(PursueFlower)
        {
            Seek(targetFlower.transform.position, step);//As flowers don't move we do not need to check their velocity So a seek will work fine
            collectPollen();
        }
        if(PursueHive)
        {
            Seek(hive.transform.position, step);
            depositPollen();
        }
        if(Alive)
        {
            //Check lifespan vs time
            lifeSpan -= Time.deltaTime;
        }
        if(lifeSpan < 0)
        {
            Alive = false;
            die();
        }
        if(pollen>pollenMax)
        {
            //If the bee has collected as much pollen as it can it will attempt to fly home
            PursueFlower = false;
            SeekPoint = false;
            PursueHive = true;
        }

	}
}
