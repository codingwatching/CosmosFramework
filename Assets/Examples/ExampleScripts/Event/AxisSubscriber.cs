﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cosmos.Input;
using UnityEngine.UI;
using Cosmos;
using System;

public enum InputKey
{
    Vertical,
    Horizontal
}
public class AxisSubscriber : MonoBehaviour// ControllerBase<AxisSubscriber>
{
    [SerializeField]
    InputKey key;
    int sliderOffset;
    int SliderOffset { get { return Utility.Converter.Int(slider.maxValue / 2); } }
    Slider slider;
    Text text;
    IInputManager inputManager;

    [TickRefresh]
    void TickRefresh()
    {
        switch (key)
        {
            case InputKey.Vertical:
                slider.value = inputManager.GetAxis(InputAxisType._Vertical) * slider.maxValue + SliderOffset;
                break;
            case InputKey.Horizontal:
                slider.value = inputManager.GetAxis(InputAxisType._Horizontal) * slider.maxValue + SliderOffset;
                break;
        }
        float textValue = slider.value - SliderOffset;
        text.text = Utility.Converter.Int(textValue).ToString();
    }
    void Start()
    {
        slider = GetComponentInChildren<Slider>();
        text = GetComponentsInChildren<Text>()[1];
        inputManager = GameManager.GetModule<IInputManager>();
        GameManager.GetModule<IInputManager>().SetInputDevice(new StandardInputDevice());
    }
}
