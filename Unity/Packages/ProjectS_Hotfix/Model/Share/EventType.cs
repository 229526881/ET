﻿using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    namespace EventType
    {
        public struct AppStart
        {
        }

        public struct SceneChangeStart
        {
        }

        public struct SceneChangeFinish
        {
        }

        public struct PingChange
        {
            public long Ping;
        }

        public struct AfterCreateClientScene
        {
        }

        public struct AfterCreateCurrentScene
        {
        }

        public struct AfterCreateLoginScene
        {
        }

        public struct AppStartInitFinish
        {
        }

        public struct LoginFinish
        {
        }

        public struct LoadingBegin
        {
        }

        public struct LoadingFinish
        {
        }

        public struct EnterMapFinish
        {
        }

        public struct AfterUnitCreate
        {
        }

        public struct LoadConfig
        {
            public Dictionary<string, byte[]> configBytes;
        }
        
        public struct NumericChange
        {
            public NumericComponent NumericComponent;
            public NumericType NumericType;
            public float Result;
        }
    }
}