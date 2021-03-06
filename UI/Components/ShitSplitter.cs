﻿using LiveSplit.Model;
using LiveSplit.UI.Components;
using LiveSplit.UI.Components.AutoSplit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LiveSplit.ManualGameTime.UI.Components
{
    public partial class ShitSplitter : Form
    {
        protected ITimerModel Model { get; set; }
        protected ManualGameTimeSettings Settings { get; set; }

        public ShitSplitter(LiveSplitState state, ManualGameTimeSettings settings)
        {
            InitializeComponent();
            Model = new TimerModel()
            {
                CurrentState = state
            };
            Settings = settings;
        }

        private void txtGameTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                try
                {
                    var parsedTimeSpan = TimeSpanParser.Parse(txtGameTime.Text);
                    var newGameTime = parsedTimeSpan + (Settings.UseSegmentTimes ? Model.CurrentState.CurrentTime.GameTime : TimeSpan.Zero);
                    Model.CurrentState.SetGameTime(newGameTime);
                    Model.Split();
                    txtGameTime.Text = "";
                }
                catch { }
            }
        }
    }
}
