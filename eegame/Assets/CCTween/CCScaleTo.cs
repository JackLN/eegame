using UnityEngine;
using System.Collections;

namespace cctween
{
    public class CCScaleTo : CCAction
    {
        protected Vector3 m_startScale;
        protected Vector3 m_endScale;
        protected Vector3 m_delta;

        public static CCScaleTo create(float duration, float s)
        {
            CCScaleTo pScaleTo = new CCScaleTo();
            pScaleTo.initWithDuration(duration, s);
            return pScaleTo;
        }

        public static CCScaleTo create(float duration, Vector3 scale)
        {
            CCScaleTo pScaleTo = new CCScaleTo();
            pScaleTo.initWithDuration(duration, scale);
            return pScaleTo;
        }

        public override void startWithTarget(GameObject pTarget)
        {
            base.startWithTarget(pTarget);
            m_startScale = pTarget.transform.localScale;
            m_delta = m_endScale - m_startScale;
        }

        public override void step(float time)
        {
            if (m_pTarget)
            {
                Vector3 scale = m_startScale + m_delta * time;
                m_pTarget.transform.localScale = scale;
            }
        }

        protected bool initWithDuration(float duration, float s)
        {
            if (base.initWithDuration(duration))
            {
                m_endScale.x = s;
                m_endScale.y = s;
                m_endScale.z = s;
                return true;
            }
            return false;
        }

        protected bool initWithDuration(float duration, Vector3 scale)
        {
            if (base.initWithDuration(duration))
            {
                m_endScale = scale;
                return true;
            }
            return false;
        }
    }


    public class CCScaleBy : CCScaleTo
    {
        public static new CCScaleBy create(float duration, float s)
        {
            CCScaleBy pScaleBy = new CCScaleBy();
            pScaleBy.initWithDuration(duration, s);
            return pScaleBy;
        }

        public static new CCScaleBy create(float duration, Vector3 scale)
        {
            CCScaleBy pScaleBy = new CCScaleBy();
            pScaleBy.initWithDuration(duration, scale);
            return pScaleBy;
        }

        public override void startWithTarget(GameObject pTarget)
        {
            base.startWithTarget(pTarget);
            Vector3 finalScale = new Vector3(m_endScale.x * m_startScale.x, m_endScale.y * m_startScale.y, m_endScale.z * m_startScale.z);
            m_delta = finalScale - m_startScale;
        }
    }
}