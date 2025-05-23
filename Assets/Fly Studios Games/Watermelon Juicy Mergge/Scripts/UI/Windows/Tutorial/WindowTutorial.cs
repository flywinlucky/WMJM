﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WatermelonGameClone;

public class WindowTutorial : UIMonoBehaviour
{
    [SerializeField] private Transform _layout;

    private List<SetSpriteUi> _spheres;

    private void OnEnable()
    {
        StartCoroutine(ScaleAllWithDelay());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void SetSpheres()
    {
        _spheres = new();

        for (int i = 1; i < _layout.childCount; i++) // Începem de la 1, ignorăm primul copil
        {
            _spheres.Add(_layout.GetChild(i).GetComponent<SetSpriteUi>());
        }
    }


    public void Initialize(int skinId, List<GameManager.SpheresCategory> sphereSkins)
    {
        SetSpheres();

        if (skinId >= 0 && skinId < sphereSkins.Count)
        {
            List<Sphere> categorySpheres = sphereSkins[skinId].spheresCategory.Select(s => s.sphere).ToList();

            for (int i = 0; i < Mathf.Min(categorySpheres.Count, _spheres.Count); i++)
            {
                _spheres[i].SetImage(categorySpheres[i].itemSpriteRenderer.sprite);
            }

        }
    }

    private IEnumerator ScaleAllWithDelay()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < _spheres.Count; i++)
        {
            if (_spheres[i].TryGetComponent(out TwenScaling twenScaling))
            {
                twenScaling.TwenObjectScaling();
            }

            yield return new WaitForSeconds(.11f);
        }

        yield return new WaitForSeconds(3f);

        StartCoroutine(ScaleAllWithDelay());
    }
}
