using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace cctween
{
    /// <summary>
    /// 串行多个CCAction
    /// </summary>
    public class CCSequence : CCAction
    {
        protected CCAction[] m_pActions = new CCAction[2];
        protected float m_split;
        protected int m_last;

        public static CCSequence create(CCAction[] actionArray)
        {
            if (actionArray == null || actionArray.Length == 0)
            {
                Debug.LogError("action array is empty !");
                return null;
            }

            CCAction pNow;
            CCAction pPrev = actionArray[0];

            if (actionArray.Length > 1)
            {
                for (int i = 1; i < actionArray.Length; i++)
                {
                    pNow = actionArray[i];
                    if (pNow != null)
                    {
                        pPrev = createWithTwoActions(pPrev, pNow);
                    }
                }
            }
            else
            {
                pPrev = createWithTwoActions(pPrev, new CCAction());
            }

            return (CCSequence)pPrev;
        }

        public static CCSequence create(CCAction pAction1, params CCAction[] args)
        {
            CCAction pNow;
            CCAction pPrev = pAction1;

            if (args != null && args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    pNow = args[i];
                    if (pNow != null)
                    {
                        pPrev = createWithTwoActions(pPrev, pNow);
                    }
                }
            }
            else
            {
                pPrev = createWithTwoActions(pPrev, new CCAction());
            }

            return (CCSequence)pPrev;
        }

        public static CCSequence createWithTwoActions(CCAction pActionOne, CCAction pActionTwo)
        {
            CCSequence pSequence = new CCSequence();
            pSequence.initWithTwoActions(pActionOne, pActionTwo);
            return pSequence;
        }

        public override void startWithTarget(GameObject pTarget)
        {
            base.startWithTarget(pTarget);
            m_split = m_pActions[0].getDuration() / m_fDuration;
            m_last = -1;
        }

        public override void stop()
        {
            if (m_last != -1)
            {
                m_pActions[m_last].stop();
            }

            base.stop();
        }

        public override void step(float t)
        {
            int found = 0;
            float new_t = 0.0f;

            if (t < m_split)
            {
                // action[0]
                found = 0;
                if (m_split != 0)
                    new_t = t / m_split;
                else
                    new_t = 1;

            }
            else
            {
                // action[1]
                found = 1;
                if (m_split == 1)
                    new_t = 1;
                else
                    new_t = (t - m_split) / (1 - m_split);
            }

            if (found == 1)
            {
                if (m_last == -1)
                {
                    // action[0] was skipped, execute it.
                    m_pActions[0].startWithTarget(m_pTarget);
                    m_pActions[0].step(1.0f);
                    m_pActions[0].stop();
                }
                else if (m_last == 0)
                {
                    // switching to action 1. stop action 0.
                    m_pActions[0].step(1.0f);
                    m_pActions[0].stop();
                }
            }
            else if (found == 0 && m_last == 1)
            {
                // Reverse mode ?
                // XXX: Bug. this case doesn't contemplate when _last==-1, found=0 and in "reverse mode"
                // since it will require a hack to know if an action is on reverse mode or not.
                // "step" should be overriden, and the "reverseMode" value propagated to inner Sequences.
                m_pActions[1].step(0);
                m_pActions[1].stop();
            }
            // Last action found and it is done.
            if (found == m_last && m_pActions[found].isDone())
            {
                return;
            }

            // Last action found and it is done
            if (found != m_last)
            {
                m_pActions[found].startWithTarget(m_pTarget);
            }

            m_pActions[found].step(new_t);
            m_last = found;
        }

        protected bool initWithTwoActions(CCAction pActionOne, CCAction pActionTwo)
        {
            float d = pActionOne.getDuration() + pActionTwo.getDuration();
            base.initWithDuration(d);

            m_pActions[0] = pActionOne;
            m_pActions[1] = pActionTwo;

            return true;
        }
    }
}