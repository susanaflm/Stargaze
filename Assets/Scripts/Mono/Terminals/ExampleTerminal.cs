using UnityEngine;
using UnityEngine.UI;

namespace Stargaze.Mono.Terminals
{
    public class ExampleTerminal : Terminal
    {
        public void OnHelloButtonPressed()
        {
            Debug.Log("Hello");
        }
        
        public void OnHiButtonPressed()
        {
            Debug.Log("Hi");
        }
        
        public void OnBeyButtonPressed()
        {
            Debug.Log("Bey");
        }
    }
}