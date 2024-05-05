using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class TabInventoryManager : MonoBehaviour
{
    [Header("Dependencies")]
    [Required][SerializeField] private CharacterPiecesSO _characterPieces;

    [Header("Settings")]
    [SerializeField] private bool _hideEmptyPanels = true;
    [SerializeField] private BodyPartType _bodyPartType;

    private List<InventoryPanelManager> _inventoryPanelList  = new List<InventoryPanelManager>();

    private void Awake()
    {
        GetInventoryPanelList();
    }

    private void OnEnable()
    {
        Enableall();
        LoadPanels();
        if(_hideEmptyPanels) UpdateActives(); //Change this to hide or unhide panels that dont have an item.
    }

    private void LoadPanels()
    {
        List<BaseItemSO> itemsList = _characterPieces.GetListOf(_bodyPartType);
       for(int i = 0; i < itemsList.Count; i++)
        {
            _inventoryPanelList[i].BaseItem = itemsList[i];
        }
    }

    private void GetInventoryPanelList()
    {
        foreach (Transform child in transform)
        {
            InventoryPanelManager inventoryPanel = child.GetComponent<InventoryPanelManager>();
            if (inventoryPanel != null)
            {
                _inventoryPanelList.Add(inventoryPanel);
            }
        }
    }

    private void UpdateActives()
    {
        foreach (InventoryPanelManager panel in _inventoryPanelList)
        {
            if (panel.BaseItem == null)
            {
                panel.gameObject.SetActive(false);
            }
            else
            {
                panel.gameObject.SetActive(true);
            }
        }
    }

    private void Enableall()
    {
        foreach (InventoryPanelManager panel in _inventoryPanelList)
        {
            panel.gameObject.SetActive(true);
        }
    }
}
