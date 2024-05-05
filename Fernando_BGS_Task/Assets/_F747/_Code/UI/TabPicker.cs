using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabPicker : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private List<GameObject> _inventoryPanelList;
    [SerializeField] private List<Button> _tabButtonList;
    [Required][SerializeField] private Sprite _activeButtonSprite;
    [Required][SerializeField] private Sprite _inactiveButtonSprite;

    private int _activeTabIndex = 0;

    private void OnEnable()
    {
        for (int i = 0; i < _tabButtonList.Count; i++)
        {
            int index = i;
            _tabButtonList[i].onClick.AddListener(() => ToggleTab(index));
        }
        ChangeButtonSprite(_tabButtonList[_activeTabIndex], _activeButtonSprite);
        ToggleTab(_activeTabIndex);
    }

    private void OnDisable()
    {
        for (int i = _tabButtonList.Count - 1; i >= 0; i--)
        {
            int index = i;
            _tabButtonList[i].onClick.RemoveListener(() => ToggleTab(index));
        }
    }

    private void ToggleTab(int selectedIndex)
    {
        foreach (GameObject go in _inventoryPanelList)
        {
            go.SetActive(false);
        }

        _inventoryPanelList[selectedIndex].SetActive(true);
        ChangeButtonSprite(_tabButtonList[_activeTabIndex], _inactiveButtonSprite);
        _activeTabIndex = selectedIndex;
        ChangeButtonSprite(_tabButtonList[_activeTabIndex], _activeButtonSprite);
    }

    private void ChangeButtonSprite(Button button, Sprite sprite)
    {
        Image image = button.GetComponent<Image>();
        if (image != null)
        {
            image.sprite = sprite;
        }
    }
}
