using UnityEngine;
using System.Collections;

namespace cctween
{
    public class CCRepeat : CCAction
    {
        protected uint m_uTimes;
        protected uint m_uTotal;
        protected float m_fNextDt;
        protected bool m_bActionInstant;
        protected CCAction m_pInnerAction;

        public static CCRepeat create(CCAction pAction, uint times)
        {
            CCRepeat pRepeat = new CCRepeat();
            pRepeat.initWithAction(pAction, times);
            return pRepeat;
        }

        public override void startWithTarget(GameObject pTarget)
        {
            m_uTotal = 0;
            m_fNextDt = m_pInnerAction.getDuration() / m_fDuration;
            base.startWithTarget(pTarget);
            m_pInnerAction.startWithTarget(pTarget);
        }

        public override void stop()
        {
            m_pInnerAction.stop();
            base.stop();
        }

        public override void step(float dt)
        {
            if (dt >= m_fNextDt)
            {
                while (dt > m_fNextDt && m_uTotal < m_uTimes)
                {
                    m_pInnerAction.step(1.0f);
                    m_uTotal++;

                    m_pInnerAction.stop();
                    m_pInnerAction.startWithTarget(m_pTarget);
                    m_fNextDt += m_pInnerAction.getDuration() / m_fDuration;
                }

                // fix for issue #1288, incorrect end value of repeat
                if (dt >= 1.0f && m_uTotal < m_uTimes)
                {
                    m_uTotal++;
                }

                // don't set an instant action back or update it, it has no use because it has no duration
                if (!m_bActionInstant)
                {
                    if (m_uTotal == m_uTimes)
                    {
                        m_pInnerAction.step(1);
                        m_pInnerAction.stop();
                    }
                    else
                    {
                        // issue #390 prevent jerk, use right update
                        m_pInnerAction.step(dt - (m_fNextDt - m_pInnerAction.getDuration() / m_fDuration));
                    }
                }
            }
            else
            {
                m_pInnerAction.step((dt * m_uTimes) % 1.0f);
            }
        }

        public override bool isDone()
        {
            return m_uTotal == m_uTimes;
        }

        public void setInnerAction(CCAction pAction)
        {
            if (m_pInnerAction != pAction)
            {
                m_pInnerAction = pAction;
            }
        }

        public CCAction getInnerAction()
        {
            return m_pInnerAction;
        }

        protected bool initWithAction(CCAction pAction, uint times)
        {
            float d = pAction.getDuration() * times;

            if (base.initWithDuration(d))
            {
                m_uTimes = times;
                m_pInnerAction = pAction;

                m_bActionInstant = (pAction != null) ? true : false;
                //an instant action needs to be executed one time less in the update method since it uses startWithTarget to execute the action
                if (m_bActionInstant)
                {
                    m_uTimes -= 1;
                }
                m_uTotal = 0;

                return true;
            }

            return false;
        }
    }
}
