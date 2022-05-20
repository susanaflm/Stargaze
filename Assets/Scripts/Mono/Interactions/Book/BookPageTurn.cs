using UnityEngine;
using UnityEngine.UI;

namespace Stargaze.Mono.Interactions.Book
{
    public class BookPageTurn : MonoBehaviour
    {

        private Animator _animator;

        private bool isPageTurned = false;

        // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponent<Animator>();
        }

        void TurnPageFront()
        {
            if (!isPageTurned)
            {
                _animator.SetTrigger("TurnPageForward");
                isPageTurned = true;
                Debug.Log("Turned Page");
            }
        }

        void TurnPageBack()
        {
            if (isPageTurned)
            {
                _animator.SetTrigger("TurnPageBack");
                isPageTurned = false;
                Debug.Log("Turned Page Back");
            }
        }

        public void AssignButtons(Button back, Button front)
        {
            back.onClick.AddListener(TurnPageBack);
            front.onClick.AddListener(TurnPageFront);
        }
    }
}
