using NaughtyAttributes;
using Obvious.Soap;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Pieces List", menuName = "ShoopingSpree/Character Pieces List")]
public class CharacterPiecesSO : ScriptableObject
{
    [Header("Dependencies")]
    [Required][SerializeField] private ScriptableEventNoParam _armourUpdatedChannel;

    public List<BaseItemSO> HoodPieces;
    //public List<BaseItemSO> HairPieces;
    public List<BaseItemSO> FacePieces;
    public List<BaseItemSO> HeadPieces;
    public List<BaseItemSO> ShoulderPieces;
    public List<BaseItemSO> ElbowPieces;
    public List<BaseItemSO> TorsoPieces;
    public List<BaseItemSO> WristPieces;
    public List<BaseItemSO> PelvisPieces;
    public List<BaseItemSO> LegsPieces;
    public List<BaseItemSO> BootPieces;
    public List<BaseItemSO> WeaponPieces;

    public int HoodCurrentIndex = 0;
    //public int HairCurrentIndex = 0;
    public int FaceCurrentIndex = 0;
    public int HeadCurrentIndex = 0;
    public int ShoulderCurrentIndex = 0;
    public int ElbowCurrentIndex = 0;
    public int TorsoCurrentIndex = 0;
    public int WristsCurrentIndex = 0;
    public int PelvisCurrentIndex = 0;
    public int LegsCurrentIndex = 0;
    public int BootCurrentIndex = 0;
    public int WeaponCurrentIndex = 0;

    private Dictionary<BodyPartType, List<BaseItemSO>> _piecesDictionary = new Dictionary<BodyPartType, List<BaseItemSO>>();
    private Dictionary<BodyPartType, int> _indexDictionary = new Dictionary<BodyPartType, int>();

    public List<BaseItemSO> GetListOf(BodyPartType partType)
    {
        if (_piecesDictionary.Count == 0)
        {
            BuildPiecesDictionary();
        }
        List<BaseItemSO> responseList;
        _piecesDictionary.TryGetValue(partType, out responseList);
        return responseList;
    }   

    public void UpdateIndexOf(BodyPartType bodyPart, int newIndex)
    {
        if(_piecesDictionary.Count == 0)
        {
            BuildIndexDictionary();
        }
        _indexDictionary[bodyPart] = newIndex;
        SaveInfo(bodyPart, newIndex);
    }

    public void SaveInfo(BodyPartType piece, int index)
    {
        _armourUpdatedChannel.Raise();
        PlayerPrefs.SetInt(piece.ToString(), index);
    }

    [Button("Load Info")]
    public void LoadInfo()
    {
        HoodCurrentIndex = PlayerPrefs.GetInt(BodyPartType.Hood.ToString());
        //HairCurrentIndex = PlayerPrefs.GetInt(BodyPartType.Hair.ToString());
        FaceCurrentIndex = PlayerPrefs.GetInt(BodyPartType.Face.ToString());
        HeadCurrentIndex = PlayerPrefs.GetInt(BodyPartType.Head.ToString());
        ShoulderCurrentIndex = PlayerPrefs.GetInt(BodyPartType.Shoulder.ToString());
        ElbowCurrentIndex = PlayerPrefs.GetInt(BodyPartType.Elbow.ToString());
        TorsoCurrentIndex = PlayerPrefs.GetInt(BodyPartType.Torso.ToString());
        WristsCurrentIndex = PlayerPrefs.GetInt(BodyPartType.Wrist.ToString());
        PelvisCurrentIndex = PlayerPrefs.GetInt(BodyPartType.Pelvis.ToString());
        LegsCurrentIndex = PlayerPrefs.GetInt(BodyPartType.Leg.ToString());
        BootCurrentIndex = PlayerPrefs.GetInt(BodyPartType.Boot.ToString());
        WeaponCurrentIndex = PlayerPrefs.GetInt(BodyPartType.Weapon.ToString());

        foreach (BodyPartType bodyPart in (BodyPartType[])Enum.GetValues(typeof(BodyPartType)))
        {
            if (bodyPart == BodyPartType.Hair || bodyPart == BodyPartType.Head) continue;
            foreach (BaseItemSO baseItem in GetListOf(bodyPart))
            {
                baseItem.LoadInfo();
            }
        }
    }

    public List<BaseItemSO> GetAllCurrentItems()
    {
        LoadInfo();
        List<BaseItemSO> currentItems = new List<BaseItemSO>();

        currentItems.Add(HoodPieces[HoodCurrentIndex]);
        //currentItems.Add(HairPieces[HairCurrentIndex]);
        currentItems.Add(FacePieces[FaceCurrentIndex]);
        currentItems.Add(HeadPieces[HeadCurrentIndex]);
        currentItems.Add(ShoulderPieces[ShoulderCurrentIndex]);
        currentItems.Add(ElbowPieces[ElbowCurrentIndex]);
        currentItems.Add(TorsoPieces[TorsoCurrentIndex]);
        currentItems.Add(WristPieces[WristsCurrentIndex]);
        currentItems.Add(PelvisPieces[PelvisCurrentIndex]);
        currentItems.Add(LegsPieces[LegsCurrentIndex]);
        currentItems.Add(BootPieces[BootCurrentIndex]);
        currentItems.Add(WeaponPieces[WeaponCurrentIndex]);

        return currentItems;
    }

    private void BuildPiecesDictionary()
    {
        _piecesDictionary = new Dictionary<BodyPartType, List<BaseItemSO>>
        {
            { BodyPartType.Hood, HoodPieces },
            //{ BodyPartType.Hair, HairPieces },
            { BodyPartType.Face, FacePieces },
            { BodyPartType.Head, HeadPieces },
            { BodyPartType.Shoulder, ShoulderPieces },
            { BodyPartType.Elbow, ElbowPieces },
            { BodyPartType.Torso, TorsoPieces },
            { BodyPartType.Wrist, WristPieces },
            { BodyPartType.Pelvis, PelvisPieces },
            { BodyPartType.Leg, LegsPieces },
            { BodyPartType.Boot, BootPieces },
            { BodyPartType.Weapon, WeaponPieces }
        };
    }

    private void BuildIndexDictionary()
    {
        _indexDictionary = new Dictionary<BodyPartType, int>
        {
            { BodyPartType.Hood, HoodCurrentIndex },
           //{ BodyPartType.Hair, HairCurrentIndex },
            { BodyPartType.Face, FaceCurrentIndex },
            { BodyPartType.Head, HeadCurrentIndex },
            { BodyPartType.Shoulder, ShoulderCurrentIndex },
            { BodyPartType.Elbow, ElbowCurrentIndex },
            { BodyPartType.Torso, TorsoCurrentIndex },
            { BodyPartType.Wrist, WristsCurrentIndex },
            { BodyPartType.Pelvis, PelvisCurrentIndex },
            { BodyPartType.Leg, LegsCurrentIndex },
            { BodyPartType.Boot, BootCurrentIndex },
            { BodyPartType.Weapon, WeaponCurrentIndex }
        };
    }
    private void LoadPurchased(List<BaseItemSO> itemList)
    {
        foreach (BaseItemSO item in itemList)
        {
            item.LoadInfo();
        }
    }

    [Button("First Save")]
    public void FirstSave()
    {
        if (PlayerPrefs.HasKey(BodyPartType.Hood.ToString())) return;

        SaveInfo(BodyPartType.Hood, HoodCurrentIndex);
        //SaveInfo(BodyPartType.Hair, HairCurrentIndex);
        SaveInfo(BodyPartType.Face, FaceCurrentIndex);       
        SaveInfo(BodyPartType.Head, HeadCurrentIndex);
        SaveInfo(BodyPartType.Shoulder, ShoulderCurrentIndex);
        SaveInfo(BodyPartType.Elbow, ElbowCurrentIndex);
        SaveInfo(BodyPartType.Torso, TorsoCurrentIndex);
        SaveInfo(BodyPartType.Wrist, WristsCurrentIndex);
        SaveInfo(BodyPartType.Pelvis, PelvisCurrentIndex);
        SaveInfo(BodyPartType.Leg, LegsCurrentIndex);
        SaveInfo(BodyPartType.Boot, BootCurrentIndex);
        SaveInfo(BodyPartType.Weapon, WeaponCurrentIndex);
    }

    [Button("Hard Reset")]
    public void HardReset()
    {
        foreach (BodyPartType bodyPart in (BodyPartType[])Enum.GetValues(typeof(BodyPartType)))
        {
            if (bodyPart == BodyPartType.Hair) continue;

            List<BaseItemSO> itemList = GetListOf(bodyPart);
            for(int i = 0; i < itemList.Count; i++)
            {
                SaveInfo(bodyPart, 0);

                if (i == 0) continue;
                itemList[i].SaveInfo(false);
            }
        }
    }

}
