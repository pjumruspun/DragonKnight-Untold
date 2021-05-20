using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject perkMenuObject;

    [SerializeField]
    private Transform perkDisplayParent;

    [SerializeField]
    private GameObject perkDisplayPrefab;

    private List<PerkDisplay> perkDisplays = new List<PerkDisplay>();

    private void Start()
    {
        perkMenuObject.SetActive(false);
        perkDisplays = new List<PerkDisplay>();
    }

    private void Update()
    {
        ListenToPerkMenuInput();
    }

    private void ListenToPerkMenuInput()
    {
        if (InputManager.PerkMenu)
        {
            bool shouldActive = !perkMenuObject.activeInHierarchy;
            perkMenuObject.SetActive(shouldActive);
            UpdatePerkDisplays();
        }
    }

    private void UpdatePerkDisplays()
    {
        EnsurePerkDisplayCapacity();
        ShowPerkDisplays();

        int index = 0;
        foreach (var perk in PerkStatic.perks)
        {
            perkDisplays[index].SetPerk(perk);
            ++index;
        }
    }

    private void ShowPerkDisplays()
    {
        int count = PerkStatic.perks.Count;
        for (int i = 0; i < perkDisplays.Count; ++i)
        {
            if (i < count)
            {
                perkDisplays[i].gameObject.SetActive(true);
            }
            else
            {
                perkDisplays[i].gameObject.SetActive(false);
            }
        }
    }

    private void EnsurePerkDisplayCapacity()
    {
        int count = PerkStatic.perks.Count;
        if (perkDisplays.Count < count)
        {
            int difference = count - perkDisplays.Count;
            for (int i = 0; i < difference; ++i)
            {
                GameObject instantiatedPerkDisplay = Instantiate(perkDisplayPrefab);
                instantiatedPerkDisplay.transform.SetParent(perkDisplayParent);
                if (instantiatedPerkDisplay.TryGetComponent<PerkDisplay>(out var perkDisplay))
                {
                    perkDisplays.Add(perkDisplay);
                }
                else
                {
                    throw new System.Exception($"Error in EnsurePerkDisplayCapacity: PerkDisplay component not found in instantiated perkDisplayPrefab GameObject");
                }
            }
        }
    }
}
