using FastSpring;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Siege.Gameplay.UI
{
    public class ButtonFeedbacks : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private TransformSpring _target;
        [SerializeField] private AudioClip _clickClip, _enterClip, _exitClip;

        private void Awake()
        {
            _target = _target != null ? _target : GetComponent<TransformSpring>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_target.IsAtEquilibrium)
                return;
            _target.BumpRotation(Random.Range(1, 2f) * RandomSign())
                .BumpScale(5f);

            // if (_enterClip != null)
            //     SfxPlayer.Play(_enterClip, 0.5f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_target.IsAtEquilibrium)
                return;
            
            _target.BumpScale(2f);

            // if (_exitClip != null)
            //     SfxPlayer.Play(_exitClip, 0.5f);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _target.BumpRotation(Random.Range(3, 7f) * RandomSign())
                .BumpScale(10);

            // if (_clickClip != null)
            //     SfxPlayer.Play(_clickClip);
        }

        private static int RandomSign()
        {
            return Random.value > 0.5f ? 1 : -1;
        }
    }
}