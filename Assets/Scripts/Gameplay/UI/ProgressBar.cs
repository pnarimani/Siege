using FastSpring;
using UnityEngine;

namespace Siege.Gameplay.UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] TransformSpring _bumpTarget;

        [SerializeField] ImageSpring _increase, _decrease, _foreground;
        [SerializeField] float _increaseDelay = 0.5f;
        [SerializeField] float _decreaseDelay = 0.5f;

        [SerializeField] float _increaseScaleBump = 2f;
        [SerializeField] float _decreaseScaleBump = 2f;

        float _updateTimer;
        float _targetFill;

        public void Update01(float normalized)
        {
            if (Mathf.Approximately(_targetFill, normalized))
                return;
            _targetFill = normalized;
            if (_foreground.FillAmount.Current > normalized)
            {
                _foreground.FillAmount.MoveInstantly(normalized);
                _updateTimer = _decreaseDelay;

                if (_bumpTarget)
                    _bumpTarget.BumpScale(_decreaseScaleBump);
            }
            else
            {
                _increase.FillAmount.MoveInstantly(normalized);
                _updateTimer = _increaseDelay;
                if (_bumpTarget)
                    _bumpTarget.BumpScale(_increaseScaleBump);
            }
        }

        void Update()
        {
            if (_updateTimer <= 0)
                return;

            _updateTimer -= Time.deltaTime;

            if (_updateTimer <= 0)
            {
                _foreground.FillAmount.Target = _targetFill;
                _increase.FillAmount.Target = _targetFill;
                _decrease.FillAmount.Target = _targetFill;
            }
        }

        public void Set01(float normalized)
        {
            _foreground.FillAmount.MoveInstantly(normalized);
            _increase.FillAmount.MoveInstantly(normalized);
            _decrease.FillAmount.MoveInstantly(normalized);
        }
    }
}