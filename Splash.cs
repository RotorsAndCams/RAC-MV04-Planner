﻿using System;
using System.Reflection;
using System.Windows.Forms;

namespace MissionPlanner
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();

            string strVersion = typeof(Splash).GetType().Assembly.GetName().Version.ToString();


            Console.WriteLine(strVersion);


            Console.WriteLine("Splash .ctor");
        }
    }
}