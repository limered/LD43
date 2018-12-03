using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Systems.Sound.Messages
{
    public class MuteMusicMessage
    {
        private readonly bool _mute;
        public bool Mute { get { return _mute; } }

        public MuteMusicMessage(bool mute)
        {
            _mute = mute;
        }
    }
}
