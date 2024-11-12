using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class AbilitiesUI : MonoBehaviour
{
    public GameObject iconPrefab;
    public GameObject containerPrefab;
    public Sprite[] icons;
    public int[] abilities;
    List<GameObject> iconObjects;
    List<GameObject> containers;
    public int current;
    void Awake()
    {
        abilities = GameObject.FindGameObjectWithTag("Finish").GetComponent<portal>().passes;
        iconObjects = new List<GameObject>();
        containers = new List<GameObject>();
        RectTransform rect = GetComponent<RectTransform>();
        VerticalLayoutGroup layout = GetComponent<VerticalLayoutGroup>();
        float offset = containerPrefab.GetComponent<RectTransform>().rect.height + layout.spacing;
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.y + offset * abilities.Length + layout.padding.top + layout.padding.bottom);

        /*foreach (int ability in abilities)
        {
            GameObject icon = Instantiate(iconPrefab, gameObject.transform);
            icon.GetComponent<Image>().sprite = icons[ability];
            iconObjects.Add(icon);
        }*/
        for (int i = 0; i < abilities.Length; i++)
        {
            GameObject container = Instantiate(containerPrefab, gameObject.transform);
            container.transform.SetParent(gameObject.transform);
            TextMeshProUGUI run = container.transform.Find("run").GetComponent<TextMeshProUGUI>();
            run.text = "Run " + (i + 1);
            TextMeshProUGUI abilityName = container.transform.Find("ability name").GetComponent<TextMeshProUGUI>();
            switch (abilities[i])
            {
                case 0:
                    abilityName.text = "WALK";
                    break;
                case 1:
                    abilityName.text = "WALLJUMP & RUN";
                    break;
                case 2:
                    abilityName.text = "TELEPORT";
                    break;
                case 3:
                    abilityName.text = "GRAVITY";
                    break;
                case 4:
                    abilityName.text = "FLY";
                    break;
                default:
                    abilityName.text = "";
                    break;
            }
            GameObject icon = container.transform.Find("Icon").gameObject;
            icon.GetComponent<Image>().sprite = icons[abilities[i]];
            iconObjects.Add(icon);
            containers.Add(container);
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
            }
            else
            {
                iconImage.color = new Color(1, 1, 1, 0.25f);
            }
        }
    }

    public void updateCharges(int charges)
    {
        if (current < iconObjects.Count && abilities[current] == 2 || abilities[current] == 3)
        {
            TextMeshProUGUI text = containers[current].transform.Find("charges").GetComponent<TextMeshProUGUI>();
            text.text = "Charges: " + charges;
        }
    }
}
