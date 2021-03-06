﻿using LiveSplit.ManualGameTime.UI.Components;
using LiveSplit.Model;
using LiveSplit.Model.Comparisons;
using LiveSplit.TimeFormatters;
using LiveSplit.UI.Components;
using LiveSplit.UI.Components.AutoSplit;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LiveSplit.UI.Components
{
    public class ManualGameTimeComponent : LogicComponent
    {
        public ManualGameTimeSettings Settings { get; set; }

        public GraphicsCache Cache { get; set; }
        protected LiveSplitState CurrentState { get; set; }
        public Form GameTimeForm { get; set; }
        protected Point PreviousLocation { get; set; } 

        public override string ComponentName
        {
            get { return "Manual Game Time"; }
        }

        public ManualGameTimeComponent(LiveSplitState state)
        {
            Settings = new ManualGameTimeSettings();
            GameTimeForm = new ShitSplitter(state, Settings);
            state.OnStart += state_OnStart;
            state.OnReset += state_OnReset;
            CurrentState = state;
        }

        void state_OnReset(object sender, TimerPhase e)
        {
            GameTimeForm.Close();
            PreviousLocation = GameTimeForm.Location;
        }

        void state_OnStart(object sender, EventArgs e)
        {
            GameTimeForm = new ShitSplitter(CurrentState, Settings);
            CurrentState.Form.Invoke(new Action(() => GameTimeForm.Show(CurrentState.Form)));
            if (!PreviousLocation.IsEmpty)
                GameTimeForm.Location = PreviousLocation;
            CurrentState.IsGameTimePaused = true;
            CurrentState.SetGameTime(TimeSpan.Zero);
        }

        public override Control GetSettingsControl(LayoutMode mode)
        {
            return Settings;
        }

        public override System.Xml.XmlNode GetSettings(System.Xml.XmlDocument document)
        {
            return Settings.GetSettings(document);
        }

        public override void SetSettings(System.Xml.XmlNode settings)
        {
            Settings.SetSettings(settings);
        }

        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
        }

        public override void Dispose()
        {
            if (GameTimeForm != null && !GameTimeForm.IsDisposed)
                GameTimeForm.Close();
            CurrentState.OnStart -= state_OnStart;
            CurrentState.OnReset -= state_OnReset;
        }
    }
}
