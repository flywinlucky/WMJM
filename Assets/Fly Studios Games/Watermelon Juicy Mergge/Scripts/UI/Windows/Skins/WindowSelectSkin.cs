﻿using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public partial class WindowSelectSkin : UIMonoBehaviour
{
    public const string SELECTED_SKIN_KEY = "selected_skin_id";

    [SerializeField] private GameObject _contentPanel;
    [SerializeField] private TMP_Text _unlockText;
    [SerializeField] private List<SkinItem> _skinItems;

    private int _firstSkin = 0;
    private int _currentSelectedSkinsIndex = 0;

    private void Start()
    {
        _contentPanel.SetActive(false);
        _unlockText.gameObject.SetActive(false);

        RemoteConfigManager.Instance.OnInitialized(() =>
        {
            Initialize();
            UpdateContent();

            _contentPanel.SetActive(true);
        });
    }

    private void Initialize()
    {
        _firstSkin = RemoteConfigManager.Instance.Get<int>("first_skin_id", 0);
        _currentSelectedSkinsIndex = PlayerPrefs.GetInt(SELECTED_SKIN_KEY, _firstSkin);
    }

    private void UpdateContent()
    {
        _skinItems = _skinItems.OrderBy(skin => skin.Id).ToList();
        _skinItems.ForEach(skin =>
        {
            skin.SetState(SkinItem.State.UNLOCKED);
            skin.OnClicked += (skin) => SetSkinIndex(skin.Id); 
        });

        // change skins order
        _skinItems[_firstSkin].transform.SetAsFirstSibling();

        // set selected
        _skinItems[_currentSelectedSkinsIndex].SetState(SkinItem.State.SELECTED);

        // handle locked
        if (PlayerPrefs.HasKey("unlockedLastItem"))
            return;

        int lockedId = _firstSkin == 0 ? 1 : 0;
        _skinItems[lockedId].SetState(SkinItem.State.LOCKED);

        _unlockText.gameObject.SetActive(true);
        _unlockText.text = $"Merge {_skinItems[lockedId].ConditionSphereName} to unlock";

    }

    public void SetSkinIndex(int skinIndex)
    {
        for (int i = 0; i < _skinItems.Count; i++)
        {
            if (i == _currentSelectedSkinsIndex)
                _skinItems[i].SetState(SkinItem.State.UNLOCKED);

            if (i == skinIndex)
                _skinItems[i].SetState(SkinItem.State.SELECTED);
        }

        _currentSelectedSkinsIndex = skinIndex;
        PlayerPrefs.SetInt(SELECTED_SKIN_KEY, skinIndex);
    }
}