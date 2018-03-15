using UnityEngine;
using System.Collections;

public class HiveState : MonoBehaviour {

    public int pollen;
    private float beeCountStart;
    private float beeCount;

    private float beeRadius = 12.0f;

    public GameObject beePrefab;
	// Use this for initialization


    //Spawn Bee
    void spawnBee()
    {
        GameObject bee = GameObject.Instantiate<GameObject>(beePrefab);

        bee.GetComponent<BeeHaviour>().hive = this.gameObject;
        bee.GetComponent<BeeHaviour>().Alive = true;
        bee.GetComponent<BeeHaviour>().targetPos = beeNextLocationTarget();
        bee.GetComponent<BeeHaviour>().beeRadius = beeRadius;
        bee.GetComponent<BeeHaviour>().scale = Random.Range(0.1f, 0.3f);
        bee.GetComponent<BeeHaviour>().mass = Random.Range(0.1f, 0.3f);
        bee.GetComponent<BeeHaviour>().SeekPoint = true;
        bee.GetComponent<BeeHaviour>().PursueFlower = false;
        bee.GetComponent<BeeHaviour>().PursueHive = false;
        bee.GetComponent<BeeHaviour>().pollen = 0;
        bee.GetComponent<BeeHaviour>().pollenMax = 10;
        GameObject GameManager = GameObject.FindGameObjectWithTag("game");
        bee.GetComponent<BeeHaviour>().lifeSpan = GameManager.GetComponent<GameManager>().beeLife;
        bee.GetComponent<BeeHaviour>().transform.position = transform.position;
        bee.transform.localScale *= bee.GetComponent<BeeHaviour>().scale;
    }

    public Vector3 beeNextLocationTarget()
    {
        return transform.position + new Vector3(Random.Range(-beeRadius, beeRadius), -0.5f, Random.Range(-beeRadius, beeRadius));
    }
    void Start () {
        if( beeCountStart < 0)
        {
            Debug.Log("Can't be less than 0, Beecount = 5");
            beeCount = 5;
        }
	}
	
	// Update is called once per frame
	void Update () {
        //In the Hive
        beeCount -= Time.deltaTime;
        if (beeCount < 0)
        {
            if (pollen >= 5)
            {
                spawnBee();
                beeCount = beeCountStart;
                pollen -= 5;
            }
        }
	}
}
