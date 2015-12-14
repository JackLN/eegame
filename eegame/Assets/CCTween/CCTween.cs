using UnityEngine;
using System.Collections;


namespace cctween
{
    public class CCTween : MonoBehaviour
    {
        private CCAction m_currentAction = null;

        public static void runAction(GameObject go, CCAction action)
        {
            if (go == null || action == null)
            {
                return;
            }

            CCTween tween = go.AddMissingComponent<CCTween>();
            if (tween.m_currentAction != null)
            {
                tween.m_currentAction.stop();
                tween.m_currentAction = null;
            }

            tween.enabled = true;
            tween.m_currentAction = action;
            tween.m_currentAction.startWithTarget(tween.gameObject);
        }

        public static void stopAction(GameObject go)
        {
            if (go == null)
            {
                return;
            }

            CCTween tween = go.GetComponent<CCTween>();
            if (tween != null)
            {
                if (tween.m_currentAction != null)
                {
                    tween.m_currentAction.stop();
                    tween.m_currentAction = null;
                }
                tween.enabled = false;
            } 
        }

        void Update()
        {
            if (m_currentAction != null)
            {
                m_currentAction.update(Time.deltaTime);

                if (m_currentAction.isDone())
                {
                    m_currentAction.stop();
                    m_currentAction = null;
                    this.enabled = false;
                }
            }
        }
    }
}