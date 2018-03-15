using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {


    public GameObject hivePrefab;
    public GameObject beePrefab;
    public GameObject flowerPrefab;

    public GameObject hive;

    public int startingPollen;

    //Allow user to define where hive starts
    public float hiveStartX;
    public float hiveStartZ;
    public float flowerRadius;//Radius of flowers blooming from hive

    public int numFlowers;//This will allocate how many flowers will exist in the map.
    public float flowerLife;//How long they will stay dead before reblossoming
    public float beeLife;//How long the bee will live

    //The flower generator
    void generateFlowers()
    {
        if (flowerRadius > 0)
        {
            //No need to change
        }
        else
        {
            flowerRadius = 10f;
        }
        for (int i = 0; i < numFlowers; i++)
        {
            GameObject flower = GameObject.Instantiate<GameObject>(flowerPrefab);
            flower.transform.position = NextFlowerPos();
            flower.transform.parent = this.transform;
        }
    }

    public void generateFlower(Vector3 pos)
    {
        //Generates a single flower called when a bee dies
        GameObject flower = GameObject.Instantiate<GameObject>(flowerPrefab);
        flower.transform.position = pos;
        flower.transform.parent = this.transform;
    }
    Vector3 NextFlowerPos()
    {
        return transform.position + new Vector3(Random.Range(-flowerRadius, flowerRadius), -2.5f, Random.Range(-flowerRadius, flowerRadius));
    }

    //Hive spawner
    void generateHive()
    {
        GameObject hive = GameObject.Instantiate<GameObject>(hivePrefab);
        hive.transform.position = new Vector3(hiveStartX, 0.2f, hiveStartZ);
        hive.GetComponent<HiveState>().pollen = startingPollen;
    }

    // Use this for initialization
    void Start () {
        //Here we will set up our Hive location
        //hive =Instantiate(hivePrefab, new Vector3(hiveStartX, 0.2f, hiveStartZ), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f))
        generateHive();
        //Now we can check the number of flowers and begin generating them

        if(numFlowers > 0)
        {
            generateFlowers();
        }
        else
        {
            Debug.Log("Error you must have more than 0 flowers. Adding 5 Flowers");
            numFlowers = 5;
            generateFlowers();
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
