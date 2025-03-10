using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HaltonSequence : MonoBehaviour
{
    public GameObject[] flowerSprites;
    public int flowerAmount = 100; // spawn 100 to fit 1 per area in spawn area
    public Vector2 area = new Vector2(20f, 5f); // 20 x 5 spawn area
    public Button resetButton;
    public TMP_InputField baseXInput; // UI Input for baseX
    public TMP_InputField baseYInput; // UI Input for baseY

    public List<GameObject> spawnedFlowers = new List<GameObject>(); // For reset

    void Start()
    {
        Generate();
    }

    void Generate()
    {
        for (int i = 1; i < flowerAmount; ++i) // Start at 1 so it doesnt divide by 0
        {
            float spreadOut = 2f;

            int baseX = int.TryParse(baseXInput.text, out int parsedBaseX) ? parsedBaseX : 2;
            int baseY = int.TryParse(baseYInput.text, out int parsedBaseY) ? parsedBaseY : 3;


            Vector2 point = haltonSequence2d(baseX, baseY, i);
            float x = (point.x * area.x * spreadOut) - (area.x / 2); //Dividin makes it fit/centers in area
            float y = (point.y * area.y * spreadOut) - (area.y / 2);

            Vector3 position = new Vector3(x,y,0); // instantiate only works with Vector 3

            GameObject randFlower = flowerSprites[Random.Range(0, 4)]; // 4 flower sprites
            // I feel there is a certain irony in using the random function in a demo about pseudorandom numbers
            GameObject flower = Instantiate(randFlower, position, Quaternion.identity);
            spawnedFlowers.Add(flower);
        }
    }

    public void Reset ()
    {
        foreach (GameObject flower in spawnedFlowers)
        {
            Destroy(flower);
        }
        spawnedFlowers.Clear();
        Generate();
    }


    // It seems like there is a built in Halton Sequence keyword in
    // UnityEngine.Rendering but I am trying to implement the 
    //pseudocode from Millington
    float haltonSequence1d(int fractBase, int index) //Return float, Millington didnt specify
    {
        float result = 0;
        float denominator = 1;

        while (index > 0)
        {
            denominator *= fractBase;
            result += ((index % fractBase) / denominator);
            index = (Mathf.FloorToInt(index / fractBase)); // Use floor to int instead of floor or else type error
        }

        return result;
    }

    Vector2 haltonSequence2d(int baseX, int baseY, int index) //Millington returns a tuple, can't do that
    {
        float x = haltonSequence1d(baseX, index);
        float y = haltonSequence1d(baseY, index);
        return new Vector2(x , y);
    }
}
