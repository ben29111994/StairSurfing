using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance;
    public static bool isChange;
    public static bool isFirstChange;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (isFirstChange == false)
        {
            isFirstChange = true;
            ChangeColor();
        }

        if (isChange)
        {
            isChange = false;

            if (GameManager.Instance.levelGame <= 4)
            {
                ColorIndex = 0;
            }
            else if (GameManager.Instance.levelGame == 5)
            {
                ColorIndex = 1;
            }
            else if (GameManager.Instance.levelGame < 9)
            {
                ColorIndex = Random.Range(0, 2);
            }
            else if (GameManager.Instance.levelGame == 10)
            {
                ColorIndex = 2;
            }
            else
            {
                ColorIndex = Random.Range(0, 3);
            }
        }

        Camera.main.backgroundColor = colors[ColorIndex];
        RenderSettings.fogColor = colors[ColorIndex];
        maps[ColorIndex].SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeColor();
            SceneManager.LoadScene(0);
        }
    }

    public Color[] colors;
    public GameObject[] maps;

    private int ColorIndex
    {
        get
        {
            return PlayerPrefs.GetInt("ColorIndex");
        }
        set
        {
            PlayerPrefs.SetInt("ColorIndex", value);
        }
    }

    public void ChangeColor()
    {
        int r = Random.Range(0, colors.Length);

        for(int i = 0; i < 1; i++)
        {
            if(r == ColorIndex)
            {
                r = Random.Range(0, colors.Length);
                i--;
            }
        }

        ColorIndex = r;

        isChange = true;
    }
}
