using NaughtyAttributes;
using Obvious.Soap;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    [Header("Dependencies")]
    [Header("CharacterPieces")]
    [Required][SerializeField] private CharacterPiecesSO _characterPieces;
    [Required][SerializeField] private ScriptableEventBaseItemSO _equipItemChannel;
    [Required][SerializeField] private ScriptableEventNoParam _updateArmourChannel;

    [Header("Renderers")]
    [Required][SerializeField] private SpriteRenderer _hoodSpriteRenderer;
    //[Required][SerializeField] private SpriteRenderer _hairSpriteRenderer;
    [Required][SerializeField] private SpriteRenderer _headSpriteRenderer;
    [Required][SerializeField] private SpriteRenderer _faceSpriteRenderer;
    [Required][SerializeField] private SpriteRenderer _shoulderLeftSpriteRenderer;
    [Required][SerializeField] private SpriteRenderer _shoulderRightSpriteRenderer;
    [Required][SerializeField] private SpriteRenderer _elbowLeftSpriteRenderer;
    [Required][SerializeField] private SpriteRenderer _elbowRightSpriteRenderer;
    [Required][SerializeField] private SpriteRenderer _wristLeftSpriteRenderer;
    [Required][SerializeField] private SpriteRenderer _wristRightSpriteRenderer;
    [Required][SerializeField] private SpriteRenderer _torsoSpriteRenderer;
    [Required][SerializeField] private SpriteRenderer _pelvisSpriteRenderer;
    [Required][SerializeField] private SpriteRenderer _legsLeftSpriteRenderer;
    [Required][SerializeField] private SpriteRenderer _legsRightSpriteRenderer;
    [Required][SerializeField] private SpriteRenderer _bootLeftSpriteRenderer;
    [Required][SerializeField] private SpriteRenderer _bootRightSpriteRenderer;
    [Required][SerializeField] private SpriteRenderer _weaponLeftSpriteRenderer;
    [Required][SerializeField] private SpriteRenderer _weaponRightSpriteRenderer;

    private Dictionary<BodyPartType, (SpriteRenderer, SpriteRenderer)> _spriteRendererMap;

    private void Awake()
    {
        BuildDictionary();
    }

    private void Start()
    {
        LoadArmour();
    }

    private void OnEnable()
    {
        _equipItemChannel.OnRaised += UpdateSprite;
        _updateArmourChannel.OnRaised += LoadArmour;
    }

    private void OnDisable()
    {
        _equipItemChannel.OnRaised -= UpdateSprite;
        _updateArmourChannel.OnRaised -= LoadArmour;
    }

    public void UpdateSprite(BaseItemSO newItem)
    {
        if (_spriteRendererMap.TryGetValue(newItem.BodyPartType, out var spriteRenderers))
        {
            if (spriteRenderers.Item2 == null)
            {
                SingleSpriteRenderer(spriteRenderers.Item1, newItem);
            }
            else
            {
                LeftRightSpriteRendererUpdate(spriteRenderers.Item1, spriteRenderers.Item2, newItem);
            }
        }
    }

    private void LoadArmour()
    {
        foreach(BaseItemSO baseItem in _characterPieces.GetAllCurrentItems())
        {
            UpdateSprite(baseItem);
        }
    }

    private void SingleSpriteRenderer(SpriteRenderer spriteRenderer, BaseItemSO baseItem)
    {
        spriteRenderer.sprite = baseItem.Sprite;
        UpdateIndexOfPiece(baseItem);
    }

    private void LeftRightSpriteRendererUpdate(SpriteRenderer left, SpriteRenderer right, BaseItemSO baseItem)
    {
        left.sprite = baseItem.Sprite;
        right.sprite = baseItem.Pair.Sprite;
        UpdateIndexOfPiece(baseItem);
    }

    private void UpdateIndexOfPiece(BaseItemSO baseItem)
    {
        _characterPieces.UpdateIndexOf(baseItem.BodyPartType, _characterPieces.GetListOf(baseItem.BodyPartType).IndexOf(baseItem));
    }

    private void BuildDictionary()
    {
        _spriteRendererMap = new Dictionary<BodyPartType, (SpriteRenderer, SpriteRenderer)>
        {
            { BodyPartType.Hood, (_hoodSpriteRenderer, null) },
            //{ BodyPartType.Hair, (_hairSpriteRenderer, null) },
            { BodyPartType.Head, (_headSpriteRenderer, null) },
            { BodyPartType.Face, (_faceSpriteRenderer, null) },
            { BodyPartType.Shoulder, (_shoulderLeftSpriteRenderer, _shoulderRightSpriteRenderer) },
            { BodyPartType.Elbow, (_elbowLeftSpriteRenderer, _elbowRightSpriteRenderer) },
            { BodyPartType.Wrist, (_wristLeftSpriteRenderer, _wristRightSpriteRenderer) },
            { BodyPartType.Torso, (_torsoSpriteRenderer, null) },
            { BodyPartType.Pelvis, (_pelvisSpriteRenderer, null) },
            { BodyPartType.Leg, (_legsLeftSpriteRenderer, _legsRightSpriteRenderer) },
            { BodyPartType.Boot, (_bootLeftSpriteRenderer, _bootRightSpriteRenderer) },
            { BodyPartType.Weapon, (_weaponLeftSpriteRenderer, _weaponRightSpriteRenderer) }
        };
    }


}
