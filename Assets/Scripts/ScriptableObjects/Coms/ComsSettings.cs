using UnityEngine;

namespace Stargaze.ScriptableObjects.Coms
{
    [CreateAssetMenu(fileName = "Coms Settings", menuName = "Coms Settings", order = 0)]
    public class ComsSettings : ScriptableObject
    {
        public ushort StartingFrequency = 1234;

        [Space]
        
        public ushort EmergencyFrequency = 4321;
        public ushort ErrorMargin = 2;
    }
}