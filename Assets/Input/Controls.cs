// GENERATED AUTOMATICALLY FROM 'Assets/Input/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""General"",
            ""id"": ""a79c35cf-33e1-47c5-8df5-8ee6dca44411"",
            ""actions"": [
                {
                    ""name"": ""Target Position"",
                    ""type"": ""Value"",
                    ""id"": ""0d3715b4-2459-44eb-853d-7bda815b5c3b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Target Direction"",
                    ""type"": ""Value"",
                    ""id"": ""a8f86a2a-ba8f-4e04-9afd-2017cdbd8589"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""fe1161d7-211c-40bd-8550-e5ab7eaaf536"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Target Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1baeba8f-b12f-4f52-8816-3b8fabb821a3"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Target Direction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // General
        m_General = asset.FindActionMap("General", throwIfNotFound: true);
        m_General_TargetPosition = m_General.FindAction("Target Position", throwIfNotFound: true);
        m_General_TargetDirection = m_General.FindAction("Target Direction", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // General
    private readonly InputActionMap m_General;
    private IGeneralActions m_GeneralActionsCallbackInterface;
    private readonly InputAction m_General_TargetPosition;
    private readonly InputAction m_General_TargetDirection;
    public struct GeneralActions
    {
        private @Controls m_Wrapper;
        public GeneralActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @TargetPosition => m_Wrapper.m_General_TargetPosition;
        public InputAction @TargetDirection => m_Wrapper.m_General_TargetDirection;
        public InputActionMap Get() { return m_Wrapper.m_General; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GeneralActions set) { return set.Get(); }
        public void SetCallbacks(IGeneralActions instance)
        {
            if (m_Wrapper.m_GeneralActionsCallbackInterface != null)
            {
                @TargetPosition.started -= m_Wrapper.m_GeneralActionsCallbackInterface.OnTargetPosition;
                @TargetPosition.performed -= m_Wrapper.m_GeneralActionsCallbackInterface.OnTargetPosition;
                @TargetPosition.canceled -= m_Wrapper.m_GeneralActionsCallbackInterface.OnTargetPosition;
                @TargetDirection.started -= m_Wrapper.m_GeneralActionsCallbackInterface.OnTargetDirection;
                @TargetDirection.performed -= m_Wrapper.m_GeneralActionsCallbackInterface.OnTargetDirection;
                @TargetDirection.canceled -= m_Wrapper.m_GeneralActionsCallbackInterface.OnTargetDirection;
            }
            m_Wrapper.m_GeneralActionsCallbackInterface = instance;
            if (instance != null)
            {
                @TargetPosition.started += instance.OnTargetPosition;
                @TargetPosition.performed += instance.OnTargetPosition;
                @TargetPosition.canceled += instance.OnTargetPosition;
                @TargetDirection.started += instance.OnTargetDirection;
                @TargetDirection.performed += instance.OnTargetDirection;
                @TargetDirection.canceled += instance.OnTargetDirection;
            }
        }
    }
    public GeneralActions @General => new GeneralActions(this);
    public interface IGeneralActions
    {
        void OnTargetPosition(InputAction.CallbackContext context);
        void OnTargetDirection(InputAction.CallbackContext context);
    }
}
