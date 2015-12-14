using UnityEngine;
using System.Collections;

namespace cctween
{
    /// <summary>
    /// 并行多个CCAction
    /// </summary>
    public class CCSpawn : CCAction
    {
        protected CCAction m_pOne;
        protected CCAction m_pTwo;

        public static CCSpawn create(CCAction[] actionArray)
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

            return (CCSpawn)pPrev;
        }

        public static CCSpawn create(CCAction pAction1, params CCAction[] args)
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

            return (CCSpawn)pPrev;
        }

        public static CCSpawn createWithTwoActions(CCAction pAction1, CCAction pAction2)
        {
            CCSpawn pSpawn = new CCSpawn();
            pSpawn.initWithTwoActions(pAction1, pAction2);
            return pSpawn;
        }

        public override void startWithTarget(GameObject pTarget)
        {
            base.startWithTarget(pTarget);
            m_pOne.startWithTarget(pTarget);
            m_pTwo.startWithTarget(pTarget);
        }

        public override void stop()
        {
            m_pOne.stop();
            m_pTwo.stop();
            base.stop();
        }

        public override void step(float time)
        {
            if (m_pOne != null)
            {
                m_pOne.step(time);
            }
            if (m_pTwo != null)
            {
                m_pTwo.step(time);
            }
        }

        protected bool initWithTwoActions(CCAction pAction1, CCAction pAction2)
        {
            bool bRet = false;

            float d1 = pAction1.getDuration();
            float d2 = pAction2.getDuration();

            if (base.initWithDuration(Mathf.Max(d1, d2)))
            {
                m_pOne = pAction1;
                m_pTwo = pAction2;

                if (d1 > d2)
                {
                    m_pTwo = CCSequence.createWithTwoActions(pAction2, CCDelayTime.create(d1 - d2));
                }
                else if (d1 < d2)
                {
                    m_pOne = CCSequence.createWithTwoActions(pAction1, CCDelayTime.create(d2 - d1));
                }

                bRet = true;
            }

            return bRet;
        }
    }
}