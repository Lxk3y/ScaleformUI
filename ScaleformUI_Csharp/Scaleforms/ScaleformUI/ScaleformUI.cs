﻿using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScaleformUI
{
    public class ScaleformUI : BaseScript
    {
        public static PauseMenuScaleform PauseMenu { get; set; }
        public static MediumMessageHandler MedMessageInstance { get; set; }
        public static InstructionalButtonsScaleform InstructionalButtons { get; set; }
        public static BigMessageHandler BigMessageInstance { get; set; }
        public static PopupWarning Warning { get; set; }
        public static PlayerListHandler PlayerListInstance { get; set; }
        public static MissionSelectorHandler JobMissionSelection { get; set; }
        public static BigFeedHandler BigFeed { get; set; }

        internal static Scaleform _ui { get; set; }
        public ScaleformUI()
        {
            Warning = new();
            MedMessageInstance = new();
            BigMessageInstance = new();
            PlayerListInstance = new();
            JobMissionSelection = new();
            BigFeed = new();
            PauseMenu = new();
            _ui = new("scaleformui");
            InstructionalButtons = new();
            InstructionalButtons.Load();
            Tick += ScaleformUIThread_Tick;

            EventHandlers["onResourceStop"] += new Action<string>((resName) =>
            {
                if (resName == API.GetCurrentResourceName())
                {
                    if (Game.IsPaused && API.GetCurrentFrontendMenuVersion() == -2060115030)
                    {
                        API.ActivateFrontendMenu((uint)Game.GenerateHash("FE_MENU_VERSION_EMPTY_NO_BACKGROUND"), false, -1);
                        API.AnimpostfxStop("PauseMenuIn");
                        API.AnimpostfxPlay("PauseMenuOut", 800, false);
                    }
                    _ui.CallFunction("CLEAR_ALL");
                    _ui.Dispose();
                    PauseMenu.Dispose();
                }
            });
        }

        private async Task ScaleformUIThread_Tick()
        {
            Warning.Update();
            MedMessageInstance.Update();
            BigMessageInstance.Update();
            PlayerListInstance.Update();
            JobMissionSelection.Update();
            InstructionalButtons.Update();
            BigFeed.Update();

            if (_ui is null)
                _ui = new Scaleform("ScaleformUI");

            if (!PauseMenu.Loaded)
                PauseMenu.Load();

            await Task.FromResult(0);
        }
    }
}
