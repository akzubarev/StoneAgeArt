using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{

    public int maxvalue, middlevalue;

    int count = 0;


    public void Activate()
    {
        count = 0;
        gameObject.GetComponent<Text>().text = count + "%";
        StartCoroutine(WaitAndPrint());
    }


    IEnumerator WaitAndPrint()
    {
        while (true)
        {
            yield return new WaitForSeconds(Waitingtimefromvalue(count));
            if (count < maxvalue)
            {
                count++;
                gameObject.GetComponent<Text>().text = count + "%";
            }
            else
            { 
                GameObject.Find("GameController").GetComponent<GameController>().Showwinlose();
                break;
            } 
        }
    }

    float Waitingtimefromvalue(int value)
    {
        if (value <= middlevalue / 3)
            return 0.05f - 0.0005f * value;
        else if (value <= middlevalue)
            return (0.05f - 0.0005f * middlevalue/2) + 0.01f* (value- middlevalue/2);
        else
            return (0.05f - 0.0005f * middlevalue / 2 + 0.01f*middlevalue / 2) - 0.02f * (value - middlevalue);
    }

}
