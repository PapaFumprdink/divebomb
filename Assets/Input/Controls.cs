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
                },
                {
                    ""name"": ""Cut Engines"",
                    ""type"": ""Button"",
                    ""id"": ""9d41d489-5fd2-4db1-b53f-5f68ebcc19ee"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire Primary"",
                    ""type"": ""Button"",
                    ""id"": ""b75caf00-0769-44c9-9e28-df95d11b6bd6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Repair"",
                    ""type"": ""Button"",
                    ""id"": ""2c29debc-84cc-42a0-a37b-041e028a22fb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TimeSlowDebug"",
                    ""type"": ""Button"",
                    ""id"": ""b74b6dd1-7b9e-44ee-b30d-9b096f902597"",
                    ""expectedControlType"": ""Button"",
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
                },
                {
                    ""name"": """",
                    ""id"": ""63bd6f6d-1ade-4bdc-b70d-5faf4e9821a8"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cut Engines"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f039cdce-9d09-4467-9315-f7dd50b64bf5"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire Primary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5b532262-6fcb-47b4-8f09-fe12233ed199"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Repair"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ff39da98-11af-4dac-a43f-06f26d448887"",
                    ""path"": ""<Keyboard>/capsLock"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TimeSlowDebug"",
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
        m_General_CutEngines = m_General.FindAction("Cut Engines", throwIfNotFound: true);
        m_General_FirePrimary = m_General.FindAction("Fire Primary", throwIfNotFound: true);
        m_General_Repair = m_General.FindAction("Repair", throwIfNotFound: true);
        m_General_TimeSlowDebug = m_General.FindAction("TimeSlowDebug", throwIfNotFound: true);
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
    private readonly InputAction m_General_CutEngines;
    private readonly InputAction m_General_FirePrimary;
    private readonly InputAction m_General_Repair;
    private readonly InputAction m_General_TimeSlowDebug;
    public struct GeneralActions
    {
        private @Controls m_Wrapper;
        public GeneralActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @TargetPosition => m_Wrapper.m_General_TargetPosition;
        public InputAction @TargetDirection => m_Wrapper.m_General_TargetDirection;
        public InputAction @CutEngines => m_Wrapper.m_General_CutEngines;
        public InputAction @FirePrimary => m_Wrapper.m_General_FirePrimary;
        public InputAction @Repair => m_Wrapper.m_General_Repair;
        public InputAction @TimeSlowDebug => m_Wrapper.m_General_TimeSlowDebug;
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
                @CutEngines.started -= m_Wrapper.m_GeneralActionsCallbackInterface.OnCutEngines;
                @CutEngines.performed -= m_Wrapper.m_GeneralActionsCallbackInterface.OnCutEngines;
                @CutEngines.canceled -= m_Wrapper.m_GeneralActionsCallbackInterface.OnCutEngines;
                @FirePrimary.started -= m_Wrapper.m_GeneralActionsCallbackInterface.OnFirePrimary;
                @FirePrimary.performed -= m_Wrapper.m_GeneralActionsCallbackInterface.OnFirePrimary;
                @FirePrimary.canceled -= m_Wrapper.m_GeneralActionsCallbackInterface.OnFirePrimary;
                @Repair.started -= m_Wrapper.m_GeneralActionsCallbackInterface.OnRepair;
                @Repair.performed -= m_Wrapper.m_GeneralActionsCallbackInterface.OnRepair;
                @Repair.canceled -= m_Wrapper.m_GeneralActionsCallbackInterface.OnRepair;
                @TimeSlowDebug.started -= m_Wrapper.m_GeneralActionsCallbackInterface.OnTimeSlowDebug;
                @TimeSlowDebug.performed -= m_Wrapper.m_GeneralActionsCallbackInterface.OnTimeSlowDebug;
                @TimeSlowDebug.canceled -= m_Wrapper.m_GeneralActionsCallbackInterface.OnTimeSlowDebug;
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
                @CutEngines.started += instance.OnCutEngines;
                @CutEngines.performed += instance.OnCutEngines;
                @CutEngines.canceled += instance.OnCutEngines;
                @FirePrimary.started += instance.OnFirePrimary;
                @FirePrimary.performed += instance.OnFirePrimary;
                @FirePrimary.canceled += instance.OnFirePrimary;
                @Repair.started += instance.OnRepair;
                @Repair.performed += instance.OnRepair;
                @Repair.canceled += instance.OnRepair;
                @TimeSlowDebug.started += instance.OnTimeSlowDebug;
                @TimeSlowDebug.performed += instance.OnTimeSlowDebug;
                @TimeSlowDebug.canceled += instance.OnTimeSlowDebug;
            }
        }
    }
    public GeneralActions @General => new GeneralActions(this);
    public interface IGeneralActions
    {
        void OnTargetPosition(InputAction.CallbackContext context);
        void OnTargetDirection(InputAction.CallbackContext context);
        void OnCutEngines(InputAction.CallbackContext context);
        void OnFirePrimary(InputAction.CallbackContext context);
        void OnRepair(InputAction.CallbackContext context);
        void OnTimeSlowDebug(InputAction.CallbackContext context);
    }
}
