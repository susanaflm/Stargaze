using System;
using Stargaze.Mono.Player;
using UnityEngine;
using Color = UnityEngine.Color;
using Image = UnityEngine.UI.Image;

namespace Stargaze.Mono.UI
{
    public class CrosshairColor : MonoBehaviour
    {
        private Image _crosshair;
        
        [SerializeField] private Color onHoverColor;
        [SerializeField] private Color defaultColor;
        
        void Start()
        {
            _crosshair = GetComponent<Image>();
        }
        
        private void SetHover()
        {
            _crosshair.color = onHoverColor;
        }

        private void SetDefault()
        {
            _crosshair.color = defaultColor;
        }

        private void OnEnable()
        {
            PlayerInteraction.OnHoverInteractable += SetHover;
            PlayerInteraction.OnHoverInteractableExit += SetDefault;
        }

        private void OnDisable()
        {
            PlayerInteraction.OnHoverInteractable -= SetHover;
            PlayerInteraction.OnHoverInteractableExit -= SetDefault;
        }
    }
}
