using UnityEngine;
using System;

public class OrderManager : MonoBehaviour
{
    private float _currentTimer;
    public float spawnTimer;
    public AudioClip[] monkeySounds;
    public AudioClip correctOrder;
    public AudioClip wrongOrder;
    public GameObject[] orderingMonkeys;
    public GameObject[] foodIcons;
    private GameObject[] spawnedIcons = {null, null, null, null};
    private string[] orderNames = {"monkeybread", "mudcake", "muffin"};
    private string[] orders = {"null", "null", "null", "null"};
    public int currentNumOrders = 0;
    public GameObject loveBar;
    private FillProgressBar barScript;

    void Start()
    {
        MakeOrder();
        barScript = loveBar.GetComponent<FillProgressBar>();
    }
    
    void Update ()
    {
        _currentTimer += Time.deltaTime;
        if(_currentTimer >= spawnTimer && currentNumOrders < 5)
        {
            MakeOrder();
            _currentTimer = 0;
        }
    }

    public void FulfillOrder(string foodGiven, string monkeyGiven, GameObject heldItem)
    {
        int monkeyIndex;
        Int32.TryParse(monkeyGiven[^1].ToString(), out monkeyIndex);
        if(orders[monkeyIndex - 1] == foodGiven)
        {
            orderingMonkeys[monkeyIndex - 1].GetComponent<AudioSource>().PlayOneShot(correctOrder);
            barScript.IncreaseProgress();
        }
        else
        {
            orderingMonkeys[monkeyIndex - 1].GetComponent<AudioSource>().PlayOneShot(wrongOrder);
        }
        orders[monkeyIndex - 1] = "null";
        Destroy(spawnedIcons[monkeyIndex - 1]);
        Destroy(heldItem);
        currentNumOrders -= 1;
    }

    public void MakeOrder()
    {
        // Choosing the food ordered and playing monkey sounds
        int chosenFood = UnityEngine.Random.Range(0, orderNames.Length);
        int chosenMonkey = UnityEngine.Random.Range(0, orderingMonkeys.Length);
        while(orders[chosenMonkey] != "null")
        {
            chosenMonkey = UnityEngine.Random.Range(0, orderingMonkeys.Length);
        }

        AudioSource _monkeyAudioSource = orderingMonkeys[chosenMonkey].GetComponent<AudioSource>();
        _monkeyAudioSource.PlayOneShot(monkeySounds[chosenFood]);
        orders[chosenMonkey] = orderNames[chosenFood];

        // Spawning the chat bubble for the monkey order
        Vector3 orderBubblePosition = orderingMonkeys[chosenMonkey].transform.position;
        orderBubblePosition.x -= 1;
        orderBubblePosition.y += 1;
        spawnedIcons[chosenMonkey] = Instantiate(foodIcons[chosenFood], orderBubblePosition, Quaternion.identity);
        currentNumOrders += 1;
    }
}