using UnityEngine;
using System.Collections;

namespace cctween
{
    public class CCAlphaTo : CCAction
    {
        protected float m_finalAlpha = 0;
        protected float m_startAlpha = 0;
        protected float m_deltaAlpha = 0;

        private UIRect m_rect = null;
        private Material m_material = null;
        private SpriteRenderer m_sprite = null;

        public static CCAlphaTo create(float duration, float alpha)
        {
            CCAlphaTo pRet = new CCAlphaTo();
            pRet.initWithDuration(duration, alpha);
            return pRet;
        }

        public override void startWithTarget(GameObject pTarget)
        {
            base.startWithTarget(pTarget);

            m_rect = pTarget.GetComponent<UIRect>();
            m_sprite = pTarget.GetComponent<SpriteRenderer>();
            if (m_rect == null && m_sprite == null)
            {
                Renderer render = pTarget.GetComponent<Renderer>();
                if (render != null) m_material = render.material;
                if (m_material == null) m_rect = pTarget.GetComponentInChildren<UIRect>();
            }

            m_startAlpha = this.alpha;
            m_deltaAlpha = m_finalAlpha - m_startAlpha;
        }

        public override void step(float t)
        {
            if (m_pTarget)
            {
                this.alpha = m_startAlpha + m_deltaAlpha * t;
            }
        }

        protected bool initWithDuration(float duration, float alpha)
        {
            if (base.initWithDuration(duration))
            {
                m_finalAlpha = alpha;
                return true;
            }
            return false;
        }

        private float alpha
        {
            get
            {
                if (m_rect != null)
                    return m_rect.alpha;

                if (m_sprite != null)
                    return m_sprite.color.a;

                return m_material != null ? m_material.color.a : 1f;
            }
            set
            {
                if (m_rect != null)
                {
                    m_rect.alpha = value;
                }
                else if (m_sprite != null)
                {
                    Color c = m_sprite.color;
                    c.a = value;
                    m_sprite.color = c;
                }
                else if (m_material != null)
                {
                    Color c = m_material.color;
                    c.a = value;
                    m_material.color = c;
                }
            }
        }
    }

    public class CCAlphaBy : CCAlphaTo
    {
        public static new CCAlphaBy create(float duration, float alpha)
        {
            CCAlphaBy pScaleBy = new CCAlphaBy();
            pScaleBy.initWithDuration(duration, alpha);
            return pScaleBy;
        }

        public override void startWithTarget(GameObject pTarget)
        {
            base.startWithTarget(pTarget);

            m_deltaAlpha = m_finalAlpha;
            m_finalAlpha = m_startAlpha + m_deltaAlpha;
        }
    }
}
