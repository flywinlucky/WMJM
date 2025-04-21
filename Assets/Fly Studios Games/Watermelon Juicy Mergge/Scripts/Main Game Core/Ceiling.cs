using DG.Tweening;
using UnityEngine;

namespace WatermelonGameClone
{
    public class Ceiling : MonoBehaviour
    {
        public bool gameOverTrigger;
        public Animator alertAnimator;

        private float _stayTime;
        public float s_timeLimit;

        private bool _invulnerable = false;

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Sphere") && !_invulnerable)
            {
                _stayTime += Time.deltaTime;
        
                if (_stayTime > s_timeLimit)
                {
                    if (gameOverTrigger)
                    {
                        GameManager.Instance.GameEvent.Execute(GameManager.GameState.GameOver);
                    }

                    if (alertAnimator)
                    {
                        alertAnimator.SetBool("AlertPlay", true);
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Sphere"))
            {
                _stayTime = 0;

                if (alertAnimator)
                {
                    alertAnimator.SetBool("AlertPlay", false);
                }
            }
        }

        public void GetInvulnerability()
        {
            _stayTime = 0f;

            _invulnerable = true;
            DOVirtual.DelayedCall(5f, () => _invulnerable = false);
        }
    }
}