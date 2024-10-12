using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AbilitiesUI : MonoBehaviour
{
    public GameObject iconPrefab;
    public Sprite[] icons;
    int[] abilities;
    List<GameObject> iconObjects;

    void Start()
    {
        abilities = GameObject.FindGameObjectWithTag("Finish").GetComponent<portal>().passes;
        iconObjects = new List<GameObject>();
        RectTransform rect = GetComponent<RectTransform>();
        VerticalLayoutGroup layout = GetComponent<VerticalLayoutGroup>();
        float offset = iconPrefab.GetComponent<RectTransform>().rect.height + layout.spacing;
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.y + offset * abilities.Length + layout.padding.top + layout.padding.bottom);

        foreach (int ability in abilities)
        {
            GameObject icon = Instantiate(iconPrefab, gameObject.transform);
            icon.GetComponent<Image>().sprite = icons[ability];
            iconObjects.Add(icon);
        }
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.y + GetComponent<VerticalLayoutGroup>().spacing);
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
