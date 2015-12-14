using UnityEngine;
using System.Collections;

namespace cctween
{
    public class CCMoveBy : CCAction
    {
        protected Vector3 m_positionDelta;
        protected Vector3 m_startPosition;

        public static CCMoveBy create(float duration, Vector3 deltaPosition)
        {
            CCMoveBy pRet = new CCMoveBy();
            pRet.initWithDuration(duration, deltaPosition);
            return pRet;
        }

        public override void startWithTarget(GameObject pTarget)
        {
            base.startWithTarget(pTarget);
            m_startPosition = pTarget.transform.localPosition;
        }

        public override void step(float t)
        {
            if (m_pTarget)
            {
                m_pTarget.transform.localPosition = m_startPosition + m_positionDelta * t;
            }
        }

        protected bool initWithDuration(float duration, Vector3 deltaPosition)
        {
            if (base.initWithDuration(duration))
            {
                m_positionDelta = deltaPosition;
                return true;
            }
            return false;
        }
    }


    public class CCMoveTo : CCMoveBy
    {
        public static new CCMoveTo create(float duration, Vector3 position)
        {
            CCMoveTo pRet = new CCMoveTo();
            pRet.initWithDuration(duration, position);
            return pRet;
        }

        public override void startWithTarget(GameObject pTarget)
        {
            base.startWithTarget(pTarget);
            m_positionDelta = m_positionDelta - m_startPosition;
        }
    }
}