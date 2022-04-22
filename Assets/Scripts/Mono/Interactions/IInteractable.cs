namespace Stargaze.Mono.Interactions
{
    public interface IInteractable
    {
        bool Switchable { get; }
        
        bool IsInteractable { get; }

        public void OnInteractionStart();
        
        public void OnInteractionEnd();
    }
}
