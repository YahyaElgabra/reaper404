using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class AbilitiesUI : MonoBehaviour
{
    public GameObject iconPrefab;
    public Sprite[] icons;
    int[] abilities;
    List<GameObject> iconObjects;
    int current;

    void Awake()
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

    public void updateIcons(int next)
    {
        current = next;
        for (int i = 0; i < iconObjects.Count; i++)
        {
            Image iconImage = iconObjects[i].GetComponent<Image>();

            if (i == current)
            {
                iconImage.color = new Color(1, 1, 1, 1);
                if (abilities[current] == 2 || abilities[current] == 3)
                {
                    iconImage.transform.GetChild(0).gameObject.SetActive(true);
                }
            }
            else
            {
                iconImage.color = new Color(1, 1, 1, 0.5f);
                iconImage.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void updateCharges(int charges)
    {
        Transform textMesh = iconObjects[current].transform.GetChild(0);
        textMesh.GetComponent<TextMeshProUGUI>().text = charges.ToString();
    }
}
