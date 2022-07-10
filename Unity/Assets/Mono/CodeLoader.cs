﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using YooAsset;

namespace ET
{
    public class CodeLoader : IDisposable
    {
        public static CodeLoader Instance = new CodeLoader();

        public Action Update;
        public Action FixedUpdate;
        public Action LateUpdate;
        public Action OnApplicationQuit;

        private Assembly assembly;

        public CodeMode CodeMode { get; set; }

        private CodeLoader()
        {
        }

        public void Dispose()
        {
        }

        public async ETTask Start()
        {
            switch (this.CodeMode)
            {
                case CodeMode.Mono:
                {
                    byte[] assBytes = (await YooAssetProxy.GetRawFileAsync("Code_ProjectS_Hotfix.dll")).GetRawBytes();
                    byte[] pdbBytes = (await YooAssetProxy.GetRawFileAsync("Code_ProjectS_Hotfix.pdb")).GetRawBytes();

                    assembly = Assembly.Load(assBytes, pdbBytes);

                    Dictionary<string, Type> types =
                        AssemblyHelper.GetAssemblyTypes(typeof(Game).Assembly, this.assembly);
                    Game.EventSystem.Add(types);

                    IStaticMethod start = new MonoStaticMethod(assembly, "ET.Client.Entry", "Start");
                    start.Run();
                    break;
                }
                case CodeMode.Reload:
                {
                    byte[] assBytes = File.ReadAllBytes(Path.Combine(Define.BuildOutputDir, "Data.dll"));
                    byte[] pdbBytes = File.ReadAllBytes(Path.Combine(Define.BuildOutputDir, "Data.pdb"));

                    assembly = Assembly.Load(assBytes, pdbBytes);
                    this.LoadLogic();
                    IStaticMethod start = new MonoStaticMethod(assembly, "ET.Client.Entry", "Start");
                    start.Run();
                    break;
                }
            }
        }

        // 热重载调用下面两个方法
        // CodeLoader.Instance.LoadLogic();
        // Game.EventSystem.Load();
        public void LoadLogic()
        {
            if (this.CodeMode != CodeMode.Reload)
            {
                throw new Exception("CodeMode != Reload!");
            }

            // 傻屌Unity在这里搞了个傻逼优化，认为同一个路径的dll，返回的程序集就一样。所以这里每次编译都要随机名字
            string[] logicFiles = Directory.GetFiles(Define.BuildOutputDir, "Logic_*.dll");
            if (logicFiles.Length != 1)
            {
                throw new Exception("Logic dll count != 1");
            }

            string logicName = Path.GetFileNameWithoutExtension(logicFiles[0]);
            byte[] assBytes = File.ReadAllBytes(Path.Combine(Define.BuildOutputDir, $"{logicName}.dll"));
            byte[] pdbBytes = File.ReadAllBytes(Path.Combine(Define.BuildOutputDir, $"{logicName}.pdb"));

            Assembly hotfixAssembly = Assembly.Load(assBytes, pdbBytes);

            Dictionary<string, Type> types =
                AssemblyHelper.GetAssemblyTypes(typeof(Game).Assembly, this.assembly, hotfixAssembly);

            Game.EventSystem.Add(types);
        }
    }
}