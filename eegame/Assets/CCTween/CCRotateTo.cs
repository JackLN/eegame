using UnityEngine;
using System.Collections;

namespace cctween
{
    public class CCRotateTo : CCAction
    {
        protected Vector3 m_dstAngle;
        protected Vector3 m_startAngle;
        protected Vector3 m_diffAngle;

        public static CCRotateTo create(float fDuration, Vector3 angle)
        {
            CCRotateTo pRotateTo = new CCRotateTo();
            pRotateTo.initWithDuration(fDuration, angle);
            return pRotateTo;
        }

        public bool initWithDuration(float fDuration, Vector3 angle)
        {
            if (base.initWithDuration(fDuration))
            {
                m_dstAngle = angle;
                return true;
            }
            return false;
        }

        public override void startWithTarget(GameObject pTarget)
        {
            base.startWithTarget(pTarget);

            m_startAngle = pTarget.transform.localEulerAngles;
            m_diffAngle = m_dstAngle - m_startAngle;

            m_diffAngle.x = m_diffAngle.x % 360;
            m_diffAngle.y = m_diffAngle.y % 360;
            m_diffAngle.z = m_diffAngle.z % 360;
        }

        public override void step(float time)
        {
            if (m_pTarget)
            {
                Vector3 angle = m_startAngle + m_diffAngle * time;
                m_pTarget.transform.localEulerAngles = angle;
            }
        }
    }

    public class CCRotateBy : CCAction
    {
        protected Vector3 m_dstAngle;
        protected Vector3 m_startAngle;
        protected Vector3 m_diffAngle;

        public static CCRotateBy create(float fDuration, Vector3 angle)
        {
            CCRotateBy pRotateTo = new CCRotateBy();
            pRotateTo.initWithDuration(fDuration, angle);
            return pRotateTo;
        }

        public bool initWithDuration(float fDuration, Vector3 angle)
        {
            if (base.initWithDuration(fDuration))
            {
                m_diffAngle = angle;
                return true;
            }
            return false;
        }

        public override void startWithTarget(GameObject pTarget)
        {
            base.startWithTarget(pTarget);

            m_startAngle = pTarget.transform.localEulerAngles;
            m_dstAngle = m_startAngle + m_diffAngle;
        }

        public override void step(float time)
        {
            if (m_pTarget)
            {
                Vector3 angle = m_startAngle + m_diffAngle * time;
                m_pTarget.transform.localEulerAngles = angle;
            }
        }
    }
}
