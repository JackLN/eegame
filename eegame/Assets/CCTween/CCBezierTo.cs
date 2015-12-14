using UnityEngine;
using System.Collections;

namespace cctween
{
    public struct ccBezierConfig
    {
        //! end position of the bezier
        public Vector3 endPosition;
        //! Bezier control point 1
        public Vector3 controlPoint_1;
        //! Bezier control point 2
        public Vector3 controlPoint_2;
    }

    public class CCBezierBy : CCAction
    {
        protected ccBezierConfig m_sConfig;
        protected Vector3 m_startPosition;
        protected Vector3 m_previousPosition;

        public static CCBezierBy create(float t, ccBezierConfig c)
        {
            CCBezierBy pBezierBy = new CCBezierBy();
            pBezierBy.initWithDuration(t, c);
            return pBezierBy;
        }

        public override void startWithTarget(GameObject pTarget)
        {
            base.startWithTarget(pTarget);
            m_previousPosition = m_startPosition = pTarget.transform.localPosition;
        }

        public override void step(float time)
        {
            if (m_pTarget == null)
                return;

            float xa = 0;
            float xb = m_sConfig.controlPoint_1.x;
            float xc = m_sConfig.controlPoint_2.x;
            float xd = m_sConfig.endPosition.x;

            float ya = 0;
            float yb = m_sConfig.controlPoint_1.y;
            float yc = m_sConfig.controlPoint_2.y;
            float yd = m_sConfig.endPosition.y;

            float za = 0;
            float zb = m_sConfig.controlPoint_1.z;
            float zc = m_sConfig.controlPoint_2.z;
            float zd = m_sConfig.endPosition.z;

            float x = bezierat(xa, xb, xc, xd, time);
            float y = bezierat(ya, yb, yc, yd, time);
            float z = bezierat(za, zb, zc, zd, time);

            m_pTarget.transform.localPosition = m_startPosition + new Vector3(x, y, z);
        }

        protected bool initWithDuration(float t, ccBezierConfig c)
        {
            if (base.initWithDuration(t))
            {
                m_sConfig = c;
                return true;
            }
            return false;
        }

        // Bezier cubic formula:
        //    ((1 - t) + t)3 = 1 
        // Expands to… 
        //   (1 - t)3 + 3t(1-t)2 + 3t2(1 - t) + t3 = 1 
        private float bezierat(float a, float b, float c, float d, float t)
        {
            return (Mathf.Pow(1 - t, 3) * a +
                    3 * t * (Mathf.Pow(1 - t, 2)) * b +
                    3 * Mathf.Pow(t, 2) * (1 - t) * c +
                    Mathf.Pow(t, 3) * d);
        }
    }

    public class CCBezierTo : CCBezierBy
    {
        protected ccBezierConfig m_sToConfig;

        public static new CCBezierTo create(float t, ccBezierConfig c)
        {
            CCBezierTo pBezierTo = new CCBezierTo();
            pBezierTo.initWithDuration(t, c);
            return pBezierTo;
        }

        public override void startWithTarget(GameObject pTarget)
        {
            base.startWithTarget(pTarget);
            m_sConfig.controlPoint_1 = m_sToConfig.controlPoint_1 - m_startPosition;
            m_sConfig.controlPoint_2 = m_sToConfig.controlPoint_2 - m_startPosition;
            m_sConfig.endPosition = m_sToConfig.endPosition - m_startPosition;
        }

        protected new bool initWithDuration(float t, ccBezierConfig c)
        {
            if (base.initWithDuration(t))
            {
                m_sToConfig = c;
                return true;
            }    
            return false;
        }
    }
}
