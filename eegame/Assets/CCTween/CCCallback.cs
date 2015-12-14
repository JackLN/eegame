using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cctween
{
    public class CCCallback : CCAction
    {
        private Action m_callback;
        private Action<object> m_callbackHasArg;

        private bool m_bExecute = false;
        private object m_arg = null;
        private bool m_isHasArg = false;

        // 无延时、无参数
        public static CCCallback create(Action callback)
        {
            CCCallback pAction = new CCCallback();
            pAction.initWithDuration(0, callback);
            return pAction;
        }

        // 有延时、无参数
        public static CCCallback create(float d, Action callback)
        {
            CCCallback pAction = new CCCallback();
            pAction.initWithDuration(d, callback);
            return pAction;
        }

        // 无延时、有参数
        public static CCCallback create(object arg, Action<object> callback)
        {
            CCCallback pAction = new CCCallback();
            pAction.initWithDuration(0, callback, arg);
            return pAction;
        }

        // 有延时、有参数
        public static CCCallback create(float d, object arg, Action<object> callback)
        {
            CCCallback pAction = new CCCallback();
            pAction.initWithDuration(d, callback, arg);
            return pAction;
        }

        public override void step(float t)
        {
            if (m_bExecute || t < 1.0f)
            {
                return;
            }

            m_bExecute = true;

            if (!m_isHasArg)
            {
                if (m_callback != null)
                {
                    m_callback();
                    m_callback = null;
                }
            }
            else
            {
                if (m_callbackHasArg != null)
                {
                    m_callbackHasArg(m_arg);
                    m_callbackHasArg = null;
                }
            }
        }

        protected bool initWithDuration(float duration, Action callback)
        {
            if (base.initWithDuration(duration))
            {
                m_callback = callback;
                m_arg = null;
                m_isHasArg = false;
                m_bExecute = false;
                return true;
            }
            return false;
        }

        protected bool initWithDuration(float duration, Action<object> callback, object arg)
        {
            if (base.initWithDuration(duration))
            {
                m_callbackHasArg = callback;
                m_arg = arg;
                m_isHasArg = true;
                m_bExecute = false;
                return true;
            }
            return false;
        }
    }
}
