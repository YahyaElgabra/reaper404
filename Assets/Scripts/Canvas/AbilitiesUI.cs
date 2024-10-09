using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AbilitiesUI : MonoBehaviour
{
    public GameObject iconPrefab;
    public Sprite[] icons;
    int[] abilities;
    private int offset = 120;
    List<GameObject> iconObjects;

    void Start()
    {
        abilities = GameObject.FindGameObjectWithTag("Finish").GetComponent<portal>().passes;
        iconObjects = new List<GameObject>();
        RectTransform rect = GetComponent<RectTransform>();
        foreach (int ability in abilities)
        {
            GameObject icon = Instantiate(iconPrefab, gameObject.transform);
            icon.GetComponent<Image>().sprite = icons[ability];
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.y + offset);
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y - offset / 2);
            iconObjects.Add(icon);
        }
        updateIcons(0);
    }

    public void updateIcons(int current)
    {
        for (int i = 0; i < iconObjects.Count; i++)
        {
            Image iconImage = iconObjects[i].GetComponent<Image>();

            if (i == current)
            {
                iconImage.color = new Color(1, 1, 1, 1);
            }
            else
            {
                iconImage.color = new Color(1, 1, 1, 0.5f);
            }
        }
    }
}
