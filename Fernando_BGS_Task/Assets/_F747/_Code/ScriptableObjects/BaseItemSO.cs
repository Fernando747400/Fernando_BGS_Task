using NaughtyAttributes;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Base Item", menuName = "ShoopingSpree/Base Item")]
public class BaseItemSO : ScriptableObject
{
    public int ID;
    public string Name;
    [ShowAssetPreview(256,256)]
    public Sprite Sprite;
    public BodyPartType BodyPartType;
    public int Price;
    public bool Purchased;
    public bool HasPair;
    [ShowIf("HasPair")]
    public BaseItemSO Pair;
    [ShowIf("HasPair")]
    public bool BaseOfPair;

    public bool GrantsExtraStats;
    [ShowIf("GrantsExtraStats")]
    public int Health;
    [ShowIf("GrantsExtraStats")]
    public int Damage;
    [ShowIf("GrantsExtraStats")]
    public int Speed;
    [ShowAssetPreview(256,256)]
    public Sprite StoreIcon;


    public void LoadInfo()
    {
        if (!PlayerPrefs.HasKey(Name)) return;
        Purchased = Convert.ToBoolean(PlayerPrefs.GetInt(Name));
    }

    public void SaveInfo(bool purchased)
    {
        Purchased = purchased;
        PlayerPrefs.SetInt(Name, purchased ? 1 : 0);
    }
}
