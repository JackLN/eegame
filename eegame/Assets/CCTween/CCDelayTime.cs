using UnityEngine;
using System.Collections;

namespace cctween
{
    public class CCDelayTime : CCAction
    {
        public static CCDelayTime create(float d)
        {
            CCDelayTime pAction = new CCDelayTime();
            pAction.initWithDuration(d);
            return pAction;
        }

        public override void step(float t)
        {
        }
    }
}