using UnityEngine;
using System.Collections;

namespace cctween
{
    public class CCRepeatForever : CCAction
    {
        protected CCAction m_pInnerAction;

        public static CCRepeatForever create(CCAction pAction)
        {
            CCRepeatForever pRet = new CCRepeatForever();
            pRet.initWithAction(pAction);
            return pRet;
        }

        public override void startWithTarget(GameObject pTarget)
        {
            base.startWithTarget(pTarget);
            m_pInnerAction.startWithTarget(pTarget);
        }

        public override void update(float dt)
        {
            m_pInnerAction.update(dt);
            if (m_pInnerAction.isDone())
            {
                float diff = m_pInnerAction.getElapsed() - m_pInnerAction.getDuration();
                m_pInnerAction.startWithTarget(m_pTarget);
                // to prevent jerk. issue #390, 1247
                m_pInnerAction.update(0.0f);
                m_pInnerAction.update(diff);
            }
        }

        public override bool isDone()
        {
            return false;
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

        protected bool initWithAction(CCAction pAction)
        {
            m_pInnerAction = pAction;
            return true;
        }
    }
}