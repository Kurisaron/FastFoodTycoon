using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerAssemblyInterface : WorkStationInterface
{
    // VARIABLES
    [Tooltip("The point at which falling ingredients go to")]
    public Transform endPoint;

    public GameObject topBunPrefab;
    public GameObject bottomBunPrefab;
    public GameObject cookedPattyPrefab;
    public GameObject lettucePrefab;
    public GameObject burgerPrefab;

    private bool isAssembling; // is the player in the middle of assembling a burger (bottom bun placed, no top bun)
    private AssembledBurger assembledBurger;
    public GameObject lastIngredient;

    // PROPERTIES
    private Vector3 StartPosition
    {
        get
        {
            return endPoint.position - (endPoint.forward * 3.0f);
        }
    }

    // UNITY FUNCTIONS
    protected override void Awake()
    {
        base.Awake();

        isAssembling = false;
        assembledBurger = new AssembledBurger(this);
    }

    // BUN BUTTON EVENTS
    public void BunButtonEvent()
    {
        if (!isAssembling)
        {
            if (!assembledBurger.CheckForIngredients())
            {
                PlaceBottomBun();
            }
            else
            {
                Debug.Log("Your current burger is not done!");
            }
            
        }
        else
        {
            DropTopBun();
        }
        
        isAssembling = !isAssembling;
    }

    private void PlaceBottomBun()
    {
        GameObject bun = Instantiate(bottomBunPrefab, transform);
        bun.transform.parent = null;
        bun.transform.position = endPoint.position;

        bun.transform.Rotate(new Vector3(-90.0f, 0.0f, 0.0f));

        if (assembledBurger == null) assembledBurger = new AssembledBurger(this);

        assembledBurger.AddIngredient(bun);
    }

    private void DropTopBun()
    {
        DropIngredient(topBunPrefab);
    }

    // PATTY BUTTON EVENTS
    public void PattyButtonEvent()
    {
        if (isAssembling)
        {
            DropPatty();
        }
        else
        {
            NoPatty();
        }
    }

    private void DropPatty()
    {
        DropIngredient(cookedPattyPrefab);
    }

    private void NoPatty()
    {
        Debug.Log("You can't stack a patty without a bun!");
    }

    // LETTUCE BUTTON EVENTS
    public void LettuceButtonEvent()
    {
        if (isAssembling)
        {
            DropLettuce();
        }
        else
        {
            NoLettuce();
        }
    }

    private void DropLettuce()
    {
        DropIngredient(lettucePrefab);
    }

    private void NoLettuce()
    {
        Debug.Log("You can't stack lettuce without a bun!");
    }

    // MISC FUNCTIONS
    private void DropIngredient(GameObject ingredient)
    {
        StartCoroutine(DropRoutine(Instantiate(ingredient, transform)));
    }

    private IEnumerator DropRoutine(GameObject ingredient)
    {
        ingredient.transform.parent = null;
        ingredient.transform.position = StartPosition;
        ingredient.transform.Rotate(new Vector3(-90.0f, 0.0f, 0.0f));
        
        float interval = 0.05f;
        
        while (ingredient.transform.parent == null)
        {
            yield return new WaitForSeconds(interval);

            ingredient.transform.position += -ingredient.transform.up * interval * 2.0f;

            if (lastIngredient == null)
            {
                Destroy(ingredient);
                yield break;
            }

            if (Vector3.Distance(ingredient.transform.position, lastIngredient.transform.position) <= 0.2f)
            {
                ingredient.transform.parent = lastIngredient.transform;
                assembledBurger.AddIngredient(ingredient);
            }

        }
    }


    public class AssembledBurger
    {
        private BurgerAssemblyInterface assemblyInterface;
        private List<GameObject> ingredients = new List<GameObject>();

        public AssembledBurger(BurgerAssemblyInterface burgerAssembly)
        {
            assemblyInterface = burgerAssembly;
        }

        public void AddIngredient(GameObject ingredient)
        {
            ingredients.Add(ingredient);
            assemblyInterface.lastIngredient = ingredient;

            CheckBurger();
        }

        public bool CheckForIngredients()
        {
            if (ingredients != null && ingredients.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void CheckBurger()
        {
            if (ingredients.Count >= 7 && !ingredients.Exists(ingredient => ingredient.name.Contains("Top")))
            {
                TrashBurger();
                return;
            }

            if (ingredients.Exists(ingredient => ingredient.name.Contains("Top")))
            {
                if (ingredients.Exists(ingredient => ingredient.name.Contains("Patty")) && ingredients.Exists(ingredient => ingredient.name.Contains("Lettuce")))
                {
                    FinishBurger();
                }
                else
                {
                    TrashBurger();
                }
                return;
            }
        }

        private void FinishBurger()
        {
            // TO-DO: Visuals for passing burger
            if (WorkStation.orderBuilding.DepositIngredient(Ingredient.CompleteBurger, 1))
            {
                Debug.Log("Burger Assembly passed a complete burger to the Order Building Station");
            }
            else
            {
                Debug.LogError("Burger Assembly could not pass a complete burger to the Order Building Station");
            }

            UseIngredients();
        }

        private void TrashBurger()
        {
            // TO-DO: Visuals for trashing burgers

            UseIngredients();
        }

        private void UseIngredients()
        {
            List<GameObject> ingredientsToClear = new List<GameObject>();
            foreach (GameObject ingredient in ingredients)
            {
                if (ingredient.name.Contains("Bottom"))
                {
                    // Use Buns
                }

                if (ingredient.name.Contains("Patty"))
                {
                    // Use Patty
                }

                if (ingredient.name.Contains("Lettuce"))
                {
                    // Use Lettuce
                }

                ingredientsToClear.Add(ingredient);
            }

            ingredients = new List<GameObject>();
            foreach(GameObject ingredient in ingredientsToClear)
            {
                Destroy(ingredient);
            }
        }
    }
}
