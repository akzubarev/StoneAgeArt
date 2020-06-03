using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject drawingwindow, comparisonwindow, playercounter, enemycounter, close,
        fightwindow, enemieswindow, camera, paintsurface;

    public Material shape1, shape2;

    List<int> enemies = new List<int>();
    int currentenemyid = 0;
    int accuracy = 0;
    int enemyaccuracy = 0;
    int curenemypos = 0;
    public int howmanyenemies = 2, howmanycharacters=2;

    System.Random rnd = new System.Random();


    void Start()
    {
        for (int i = 0; i < howmanyenemies; i++)
            enemies.Add(i);

        for (int i = 0; i < 8; i++)
            comparisonwindow.transform.GetChild(i).gameObject.SetActive(false);

        for (int i = 0; i < 4; i++)
            fightwindow.transform.GetChild(i).gameObject.SetActive(false);
    }

    public void FindOpponent()
    {
        curenemypos = rnd.Next(0, enemies.Count);
        currentenemyid = enemies[curenemypos];
        Fight();
    }

    public void Fight()
    {
        int currentenemycharacter = rnd.Next(howmanycharacters);
        fightwindow.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sprite = 
            Resources.Load<Sprite>($"Enemies/characters/{currentenemycharacter}_right");
        comparisonwindow.transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().sprite =
                Resources.Load<Sprite>($"Enemies/characters/{currentenemycharacter}_right");



        shape1.SetTexture("_MainTex", Resources.Load<Texture>($"Enemies/shapes_t/{currentenemyid}"));
        shape2.SetTexture("_MaskTex", Resources.Load<Texture>($"Enemies/shapes/{currentenemyid}"));

        StartCoroutine(ShowFightWindow());
    }

    public void ChangeBrush(int num)
    {
        switch (num)
        {
            /*
            case 0:
                camera.GetComponent<Es.InkPainter.Sample.MousePainter>().brush.BrushTexture = Resources.Load<Texture>($"Drawing/brush_mini");
                break;
            case 1:
                camera.GetComponent<Es.InkPainter.Sample.MousePainter>().brush.BrushTexture = Resources.Load<Texture>($"Drawing/black circle");
                break;
           */
            case 0:
                camera.GetComponent<Es.InkPainter.Sample.MousePainter>().brush.Color = Color.black;
                GameObject.Find("Brush").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Drawing/Tools/U");
                break;
            case 1:
                camera.GetComponent<Es.InkPainter.Sample.MousePainter>().brush.Color = Color.white;
                GameObject.Find("Brush").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Drawing/Tools/W");
                break;
            case 2:
                camera.GetComponent<Es.InkPainter.Sample.MousePainter>().brush.Color = Color.blue;
                GameObject.Find("Brush").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Drawing/Tools/B");
                break;

        }
    }

    public void EndDrawing()
    {
        CheckPixels stat = paintsurface.GetComponentInChildren<CheckPixels>();
        int goodpixels = stat.GoodPixelCount,
            badpixels = stat.BadPixelCount,
            incomplete = stat.IncompletedPixelCount;

        accuracy = (int)System.Math.Round(100 * Mathf.Clamp01(goodpixels / (goodpixels + incomplete * 0.8f)) * Mathf.Clamp01(((3.5f * goodpixels) - badpixels) / (goodpixels + 1f)));

        camera.GetComponent<Es.InkPainter.Sample.MousePainter>().isEnabled = false;
        drawingwindow.SetActive(false);

        paintsurface.transform.localPosition += new Vector3(-100, 175, 0);
        paintsurface.transform.localScale = new Vector3(180, 270, 1);
        paintsurface.transform.GetChild(0).gameObject.SetActive(false);

        comparisonwindow.SetActive(true);

        playercounter.GetComponent<Counter>().maxvalue = accuracy;
        if (accuracy >= 60)
            enemyaccuracy = 60;
        else if (accuracy >= 50)
            enemyaccuracy = 50;
        else if (accuracy >= 40)
            enemyaccuracy = 40;
        else
            enemyaccuracy = 40;

        enemycounter.GetComponent<Counter>().maxvalue = enemyaccuracy;

        StartCoroutine(ShowResult());
    }

    public void Returntoenemies()
    {
        for (int i = 0; i < 3; i++)
            fightwindow.transform.GetChild(i).gameObject.SetActive(false);

        paintsurface.transform.localPosition -= new Vector3(-100, 175, 0);
        paintsurface.transform.localScale = new Vector3(400, 600, 1);
        paintsurface.transform.GetChild(0).gameObject.SetActive(true);
        camera.GetComponent<Es.InkPainter.Sample.MousePainter>().Clear();
        paintsurface.SetActive(false);
        camera.GetComponent<Es.InkPainter.Sample.MousePainter>().isEnabled = true;
        accuracy=0;
        enemyaccuracy=0;
        
        comparisonwindow.SetActive(false);
        enemieswindow.SetActive(true);
        playercounter.SetActive(false);
        enemycounter.SetActive(false);
        close.SetActive(false);
        for (int i = 0; i < 5; i++)
            comparisonwindow.transform.GetChild(i).gameObject.SetActive(false);

        enemies.RemoveAt(curenemypos);
        if (enemies.Count == 0)
            for (int i = 0; i < howmanyenemies; i++)
                enemies.Add(i);
    }

    IEnumerator ShowResult()
    {
        comparisonwindow.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sprite =
                Resources.Load<Sprite>($"Enemies/Drawings/{currentenemyid}_{enemyaccuracy}");
        
        paintsurface.SetActive(false);

        yield return new WaitForSeconds(0.3f);
        comparisonwindow.transform.GetChild(0).gameObject.SetActive(true);//lines
        yield return new WaitForSeconds(0.3f);
        comparisonwindow.transform.GetChild(1).gameObject.SetActive(true);//lightning
        yield return new WaitForSeconds(0.3f);
        paintsurface.SetActive(true);//player stone
        comparisonwindow.transform.GetChild(2).gameObject.SetActive(true);//enemy stone
        comparisonwindow.transform.GetChild(3).gameObject.SetActive(true);//player
        comparisonwindow.transform.GetChild(4).gameObject.SetActive(true);//enemy
        playercounter.SetActive(true);
        enemycounter.SetActive(true);


        playercounter.GetComponent<Counter>().Activate();
        enemycounter.GetComponent<Counter>().Activate();

        yield return new WaitForSeconds(enemyaccuracy * 0.05f + 1);
        if (enemyaccuracy <= accuracy)
            close.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Buttons/win");
        else
            close.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Buttons/lose");
        close.SetActive(true);
    }

    IEnumerator ShowFightWindow()
    {
        enemieswindow.SetActive(false);
        fightwindow.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        fightwindow.transform.GetChild(0).gameObject.SetActive(true);//lines
        yield return new WaitForSeconds(0.3f);
        fightwindow.transform.GetChild(1).gameObject.SetActive(true);//lightning
        yield return new WaitForSeconds(0.3f);
        fightwindow.transform.GetChild(2).gameObject.SetActive(true);//player
        fightwindow.transform.GetChild(3).gameObject.SetActive(true);//enemy
        yield return new WaitForSeconds(1f);
        fightwindow.SetActive(false);
        for (int i = 0; i < 4; i++)
            fightwindow.transform.GetChild(i).gameObject.SetActive(false);

        drawingwindow.SetActive(true);
        paintsurface.SetActive(true);
    }

}
