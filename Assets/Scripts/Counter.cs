using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{

    public int maxvalue;
    int count = 0;


    public void Activate()
    {
        count=0;
        gameObject.GetComponent<Text>().text = count + "%";
        StartCoroutine(WaitAndPrint());
    }


    IEnumerator WaitAndPrint()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);
            if (count < maxvalue)
            {
                count++;
                gameObject.GetComponent<Text>().text = count + "%";
            }
            else
                break;
        }
    }
}
