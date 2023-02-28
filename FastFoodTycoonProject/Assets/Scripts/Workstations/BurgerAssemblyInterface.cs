using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerAssemblyInterface : WorkStationInterface
{
    // VARIABLES
    [Tooltip("The point at which falling ingredients are spawned at")]
    public Transform startingPoint;

    public GameObject topBunPrefab;
    public GameObject bottomBunPrefab;
    public GameObject cookedPattyPrefab;
    public GameObject lettucePrefab;

    public AssembledBurger assembledBurger;

    // FUNCTIONS
    protected override void Awake()
    {
        base.Awake();

        NewBurger();
    }

    public void NewBurger()
    {
        DropBottomBun();
    }

    public void DropBottomBun()
    {
        StartCoroutine(DropIngredient(Instantiate(bottomBunPrefab, startingPoint.position, Quaternion.identity)));
    }

    private IEnumerator DropIngredient(GameObject ingredient)
    {
        float interval = 0.05f;
        
        while (ingredient.transform.parent == null)
        {
            ingredient.transform.position += new Vector3(0,0, interval);

            if (Physics.Raycast(ingredient.transform.position, Vector3.forward, out RaycastHit hit))
            {
                Debug.Log(hit.collider.gameObject.name + " below current ingredient, distance: " + hit.distance);

                /*
                 * 
                 * if (hit.distance <= someDistance)
                 * {
                 *      hit.collider.gameObject
                 * 
                 */
            }

            yield return new WaitForSeconds(interval);
        }
    }

    public class AssembledBurger
    {
        public Queue<GameObject> queue = new Queue<GameObject>();


    }
}
