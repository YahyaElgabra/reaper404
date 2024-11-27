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
    public int[] charges;
    List<GameObject> iconObjects;
    List<GameObject> containers;
    public int current;
    int chargeIndex = 0;
    public AudioSource audioSource;

    void Awake()
    {
        abilities = GameObject.FindGameObjectWithTag("Finish").GetComponent<portal>().passes;
        charges = GameObject.FindGameObjectWithTag("Finish").GetComponent<portal>().charges;
        iconObjects = new List<GameObject>();
        containers = new List<GameObject>();

        RectTransform rect = GetComponent<RectTransform>();
        HorizontalLayoutGroup layout = GetComponent<HorizontalLayoutGroup>();
        float offset = containerPrefab.GetComponent<RectTransform>().rect.width + layout.spacing;
        rect.sizeDelta = new Vector2(rect.sizeDelta.x + offset * abilities.Length + layout.padding.left + layout.padding.right, rect.sizeDelta.y);

        for (int i = 0; i < abilities.Length; i++)
        {
            GameObject container = Instantiate(containerPrefab, gameObject.transform);
            TextMeshProUGUI run = container.transform.Find("run").GetComponent<TextMeshProUGUI>();
            run.text = "Run " + (i + 1);

            TextMeshProUGUI abilityName = container.transform.Find("ability name").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI textmesh = container.transform.Find("description/Input").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI text;

            switch (abilities[i])
            {
                case 0:
                    abilityName.text = "Walk";
                    textmesh.text = "Use WASD/LJoy to walk\n\nPress Space/X to jump";
                    break;
                case 1:
                    abilityName.text = "Run & Walljump";
                    textmesh.text = "Hold Shift/R1 to run\n\nPress Space/X to walljump";
                    break;
                case 2:
                    abilityName.text = "Teleport";
                    textmesh.text = "Hold C/L1 to aim, Release to cancel\n\nPress Space/X to throw\n\nMovement blocked";
                    text = container.transform.Find("charges").GetComponent<TextMeshProUGUI>();
                    if (chargeIndex < charges.Length)
                    {
                        text.text = "Charges: " + charges[chargeIndex];
                        chargeIndex++;
                    }
                    break;
                case 3:
                    abilityName.text = "Gravity";
                    textmesh.text = "Press Z/Square to rotate left\n\nPress X/Circle to rotate right";
                    text = container.transform.Find("charges").GetComponent<TextMeshProUGUI>();
                    if (chargeIndex < charges.Length)
                    {
                        text.text = "Charges: " + charges[chargeIndex];
                        chargeIndex++;
                    }
                    break;
                case 4:
                    abilityName.text = "Fly";
                    textmesh.text = "Hold Shift/R1 to boost\n\nHold Ctrl/L1 to brake";
                    break;
                default:
                    abilityName.text = "";
                    break;
            }

            textmesh.gameObject.SetActive(false);
            GameObject icon = container.transform.Find("Icon").gameObject;
            icon.GetComponent<Image>().sprite = icons[abilities[i]];
            iconObjects.Add(icon);
            containers.Add(container);
        }

        rect.sizeDelta = new Vector2(rect.sizeDelta.x + layout.spacing, rect.sizeDelta.y);
        updateIcons(0);
    }

    public void updateIcons(int next)
{
    current = next;
    for (int i = 0; i < iconObjects.Count; i++)
    {
        Image iconImage = iconObjects[i].GetComponent<Image>();
        Image containerImage = containers[i].GetComponent<Image>();
        Image frameImage = containers[i].transform.Find("frame")?.GetComponent<Image>(); // Assume "Frame" is the frame's GameObject

        if (i == current)
        {
            // Make icon opaque
            iconImage.color = new Color(1, 1, 1, 1);

            // Set container background to white
            if (containerImage != null)
            {
                containerImage.color = new Color(0f, 0f, 0f, 0.5f);
            }

            // Show frame if it exists
            if (frameImage != null)
            {
                frameImage.enabled = true; // Show the frame
            }

            // Show description
            containers[i].transform.Find("description/Input").GetComponent<TextMeshProUGUI>().gameObject.SetActive(true);
        }
        else
        {
            // Fade icon
            iconImage.color = new Color(1, 1, 1, 0.08f);

            // Set container background to gray
            if (containerImage != null)
            {
                containerImage.color = new Color(1f, 1f, 1f, 0.1f);
            }

            // Hide frame if it exists
            if (frameImage != null)
            {
                frameImage.enabled = false; // Hide the frame
            }

            // Hide description
            containers[i].transform.Find("description/Input").GetComponent<TextMeshProUGUI>().gameObject.SetActive(false);
        }
    }
}


    public void updateCharges(int charges)
    {
        if (current < iconObjects.Count && (abilities[current] == 2 || abilities[current] == 3))
        {
            TextMeshProUGUI text = containers[current].transform.Find("charges").GetComponent<TextMeshProUGUI>();
            text.text = "Charges: " + charges;
        }
    }

    public void newPassAudio()
    {
        audioSource.Play();
    }
}
