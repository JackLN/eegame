using UnityEngine;
using System.Collections;

namespace cctween
{
    public class CCJumpBy : CCAction
    {
        protected Vector3 m_startPosition;
        protected Vector3 m_delta;
        protected float m_height;
        protected uint m_nJumps;

        public static CCJumpBy create(float duration, Vector3 position, float height, uint jumps)
        {
            CCJumpBy pJumpBy = new CCJumpBy();
            pJumpBy.initWithDuration(duration, position, height, jumps);
            return pJumpBy;
        }

        public override void startWithTarget(GameObject pTarget)
        {
            base.startWithTarget(pTarget);
            m_startPosition = pTarget.transform.localPosition;
        }

        public override void step(float time)
        {
            if (m_pTarget)
            {
                float frac = (time * m_nJumps) % 1.0f;
                float y = m_height * 4 * frac * (1 - frac);
                y += m_delta.y * time;

                float x = m_delta.x * time;
                float z = m_delta.z * time;

                m_pTarget.transform.localPosition = m_startPosition + new Vector3(x, y, z);
            }
        }

        protected bool initWithDuration(float duration, Vector3 position, float height, uint jumps)
        {
            if (base.initWithDuration(duration))
            {
                m_delta = position;
                m_height = height;
                m_nJumps = jumps;
                return true;
            }
            return false;
        }
    }


    public class CCJumpTo : CCJumpBy
    {
        public static new CCJumpTo create(float duration, Vector3 position, float height, uint jumps)
        {
            CCJumpTo pJumpTo = new CCJumpTo();
            pJumpTo.initWithDuration(duration, position, height, jumps);
            return pJumpTo;
        }

        public override void startWithTarget(GameObject pTarget)
        {
            base.startWithTarget(pTarget);
            m_delta = m_delta - m_startPosition;
        }
    }

}