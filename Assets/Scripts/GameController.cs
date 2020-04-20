using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject drawingwindow, comparisonwindow, playercounter, enemycounter, close,
        fightwindow, enemieswindow, startingwindow, camera, paintsurface;

    public Material shape1, shape2;

    List<int> enemies = new List<int>();
    int currentenemyid = 0;
    int accuracy = 0;
   int curenemypos=0;
    int howmanyenemies=2;

    void Start()
    {
        enemieswindow.SetActive(true);
        for (int i = 0; i < howmanyenemies; i++)
            enemies.Add(i);

        enemieswindow.SetActive(false);
    }

    System.Random rnd = new System.Random();
    public void FindOpponent()
    {
        curenemypos= rnd.Next(0, enemies.Count);
        currentenemyid = enemies[curenemypos];
        Fight();
    }

    public void Fight()
    {
        fightwindow.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Enemies/Enemy{currentenemyid}/Enemy");
        shape1.SetTexture("_MainTex", Resources.Load<Texture>($"Enemies/Enemy{currentenemyid}/Shape"));
        shape2.SetTexture("_MaskTex", Resources.Load<Texture>($"Enemies/Enemy{currentenemyid}/Shape"));
        
        StartCoroutine(ShowFightWindow());
    }


    public void ChangeBrush(int num)
    {
        switch(num){
            case 0:
                camera.GetComponent<Es.InkPainter.Sample.MousePainter>().brush.BrushTexture = Resources.Load<Texture>($"Drawing/brush_mini");
                break;
            case 1:
                camera.GetComponent<Es.InkPainter.Sample.MousePainter>().brush.BrushTexture = Resources.Load<Texture>($"Drawing/black circle");
                break;
            case 2:
                camera.GetComponent<Es.InkPainter.Sample.MousePainter>().brush.Color = Color.red;
                break;
            case 3:
                camera.GetComponent<Es.InkPainter.Sample.MousePainter>().brush.Color = Color.white;
                break;

        }
    }

    public void EndDrawing()
    {
        CheckPixels stat = paintsurface.GetComponentInChildren<CheckPixels>();
        int goodpixels = stat.GoodPixelCount,
            badpixels = stat.BadPixelCount,
            incomplete = stat.IncompletedPixelCount;

        //accuracy = (int)(100 * (float)goodpixels / (goodpixels + incomplete) - (float)badpixels / (goodpixels + incomplete + badpixels) / 3);
        accuracy = (int)(100 * Mathf.Clamp01(goodpixels / (goodpixels + incomplete * 0.8f)) * Mathf.Clamp01(((2.4f * goodpixels) - badpixels) / (goodpixels + 1)));

        camera.GetComponent<Es.InkPainter.Sample.MousePainter>().isEnabled = false;
        drawingwindow.SetActive(false);

        paintsurface.transform.localPosition += new Vector3(-100, 200, 0);
        paintsurface.transform.localScale = new Vector3(300, 300, 1);
        paintsurface.transform.GetChild(0).gameObject.SetActive(false);

        comparisonwindow.SetActive(true);

        playercounter.GetComponent<Counter>().maxvalue = accuracy;
        if (accuracy >= 60)
            enemycounter.GetComponent<Counter>().maxvalue = 60;
        else if (accuracy >= 50)
            enemycounter.GetComponent<Counter>().maxvalue = 50;
       else if (accuracy >= 40)
            enemycounter.GetComponent<Counter>().maxvalue = 40;
        else
            enemycounter.GetComponent<Counter>().maxvalue = 40;

        playercounter.GetComponent<Counter>().Activate();
        enemycounter.GetComponent<Counter>().Activate();

        comparisonwindow.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sprite =
            Resources.Load<Sprite>($"Enemies/Enemy{currentenemyid}/Drawing{enemycounter.GetComponent<Counter>().maxvalue}");

        StartCoroutine(ShowResult());
    }

    public void Returntoenemies()
    {
        paintsurface.transform.localPosition -= new Vector3(-100, 200, 0);
        paintsurface.transform.localScale = new Vector3(600, 600, 1);
        paintsurface.transform.GetChild(0).gameObject.SetActive(true);
        camera.GetComponent<Es.InkPainter.Sample.MousePainter>().Clear();
        paintsurface.SetActive(false);
        camera.GetComponent<Es.InkPainter.Sample.MousePainter>().isEnabled = true;
        comparisonwindow.SetActive(false);
        enemieswindow.SetActive(true);

        enemies.RemoveAt(curenemypos);
        if (enemies.Count==0)
            for (int i = 0; i < howmanyenemies; i++)
                enemies.Add(i);


    }

    public void StartMenuClose()
    {
        startingwindow.SetActive(false);
        enemieswindow.SetActive(true);
    }

    IEnumerator ShowResult()
    {
        close.SetActive(false);
        yield return new WaitForSeconds(enemycounter.GetComponent<Counter>().maxvalue * 0.05f + 1);
        if (enemycounter.GetComponent<Counter>().maxvalue <= playercounter.GetComponent<Counter>().maxvalue)
            close.GetComponentInChildren<Text>().text = "You won!";
        else
            close.GetComponentInChildren<Text>().text = "You lost!";
        close.SetActive(true);
    }

    IEnumerator ShowFightWindow()
    {
        enemieswindow.SetActive(false);
        fightwindow.SetActive(true);
        yield return new WaitForSeconds(2);
        fightwindow.SetActive(false);
        drawingwindow.SetActive(true);
        paintsurface.SetActive(true);
    }

}
