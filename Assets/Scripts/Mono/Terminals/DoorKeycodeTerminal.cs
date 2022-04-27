using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Stargaze.Mono.Terminals
{
    public class DoorKeycodeTerminal : Terminal
    {
        [SyncVar(hook = nameof(OnInputCodeChanged))]
        private string _currentInput;
        
        [Header("Security")]
        [SerializeField] private string code;
        [SerializeField] private TMP_Text codeDisplay;

        [Header("Events")]
        [SerializeField] private UnityEvent onCodeValidated;

        public override void OnStartServer()
        {
            _currentInput = "";
        }

        public void OnNumberInput(int value)
        {
            CmdNumberInput(value);
        }

        [Command(requiresAuthority = false)]
        private void CmdNumberInput(int number)
        {
            _currentInput += number.ToString();
        }

        public void OnClearButtonPressed()
        {
            CmdClearInput();
        }

        [Command(requiresAuthority = false)]
        private void CmdClearInput()
        {
            _currentInput = "";
        }

        public void OnOkButtonPressed()
        {
            CmdValidateInput();
        }

        [Command(requiresAuthority = false)]
        private void CmdValidateInput()
        {
            if (_currentInput != code)
                return;
            
            onCodeValidated.Invoke();
        }

        private void OnInputCodeChanged(string oldValue, string newValue)
        {
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            codeDisplay.text = _currentInput;
        }
    }
}