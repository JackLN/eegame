using UnityEngine;
using System.Collections;

namespace cctween
{
    public class CCAction
    {
        protected GameObject m_pTarget = null;
        protected float m_fDuration = 0;

        protected float m_elapsed;
        protected bool m_bFirstTick;

        public GameObject getTarget()
        {
            return m_pTarget;
        }

        void setTarget(GameObject pTarget)
        {
            m_pTarget = pTarget;
        }

        public float getDuration()
        {
            return m_fDuration;
        }

        public void setDuration(float duration)
        {
            m_fDuration = duration;
        }

        public float getElapsed()
        {
            return m_elapsed;
        }

        //--------------- 虚函数 ---------------

        public virtual bool isDone()
        {
            return m_elapsed >= m_fDuration;
        }

        public virtual void stop()
        {
            m_pTarget = null;
        }

        public virtual void update(float dt)
        {
            if (m_bFirstTick)
            {
                m_bFirstTick = false;
                m_elapsed = 0;
            }
            else
            {
                m_elapsed += dt;
            }

            this.step(Mathf.Max(0,                                  // needed for rewind. elapsed could be negative
                              Mathf.Min(1, m_elapsed /
                                  Mathf.Max(m_fDuration, Mathf.Epsilon)   // division by 0
                                  )
                              )
                         );
        }

        public virtual void step(float times)
        {
        }

        public virtual void startWithTarget(GameObject pTarget)
        {
            m_pTarget = pTarget;
            m_elapsed = 0.0f;
            m_bFirstTick = true;
        }

        // ---------- 内部函数 ----------
        protected bool initWithDuration(float d)
        {
            m_fDuration = d;

            // prevent division by 0
            // This comparison could be in step:, but it might decrease the performance
            // by 3% in heavy based action games.
            if (m_fDuration <= 0)
            {
                m_fDuration = Mathf.Epsilon;
            }

            m_elapsed = 0;
            m_bFirstTick = true;

            return true;
        }
    }
}