using UnityEngine;
using System.Collections;

public class FlowerState : MonoBehaviour {

    public float scale;
    public int pollen;
    public float lifeSpan;
    public bool found;

    public GameObject petalPrefab;

    private Vector3 grow;
	// Use this for initialization
	void Start () {

        grow = new Vector3(0, 0.1f, 0);
        scale = Random.Range(0.5f, 1f);//Scale the flower will be
        pollen = Random.Range(1, 10);
        found = false;
        //Now we need to know where in rotation this petal will appear


        //Now lets generate the petals
        for(int i = 0; i< pollen; i++)
        {
            //There is one petal for each pollen
            //GameObject[] petals = GameObject.FindGameObjectsWithTag("petal");
            GameObject petal = GameObject.Instantiate<GameObject>(petalPrefab);

           //THIS MAY BE SCRAPPED AS TIME IS A FACTOR
        }
	}
	
	// Update is called once per frame
	void Update () {

        if(transform.position.y<1.6f)
        {
            transform.position += grow;
        }
        if(pollen <= 0)
        {
            Destroy(this.gameObject);
        }
	}
}
