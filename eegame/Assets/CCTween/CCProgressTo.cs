using UnityEngine;
using System.Collections;

namespace cctween
{
    public class CCProgressTo : CCAction
    {
        protected float m_fTo;
        protected float m_fFrom;
        protected float m_fProgress;

        private System.Action<float> m_durationCallback;

        public static CCProgressTo create(float duration, float fFromPercentage, float fToPercentage, System.Action<float> durationCallback)
        {
            CCProgressTo pAction = new CCProgressTo();
            pAction.initWithDuration(duration, fFromPercentage, fToPercentage, durationCallback);
            return pAction;
        }

        public bool initWithDuration(float duration, float fFromPercentage, float fToPercentage, System.Action<float> durationCallback)
        {
            if (base.initWithDuration(duration))
            {
                m_fTo = fToPercentage;
                m_fFrom = fFromPercentage;
                m_fProgress = fFromPercentage;
                m_durationCallback = durationCallback;
                return true;
            }
            return false;
        }

        public override void step(float t)
        {
            m_fProgress = m_fFrom + (m_fTo - m_fFrom) * t;

            if (m_durationCallback != null)
            {
                m_durationCallback(m_fProgress);
            }
        }
    }
}
