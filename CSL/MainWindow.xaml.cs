﻿using CSLPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

/// <summary>
/// Community San Andreas Multiplayer Launcher namespace
/// </summary>
namespace CSL
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ICSL
    {
        /// <summary>
        /// Modules
        /// </summary>
        private ICSLModule[] modules;

        /// <summary>
        /// Pages
        /// </summary>
        private ICSLPage[] pages;

        /// <summary>
        /// Configuration
        /// </summary>
        private Configuration configuration;

        /// <summary>
        /// Lookup modules
        /// </summary>
        private Dictionary<string, ModulesData> lookupModules = new Dictionary<string, ModulesData>();

        /// <summary>
        /// Community San Andreas Multiplayer Launcher modules reference
        /// </summary>
        private ref ICSLModule[] ModulesReference
        {
            get
            {
                if (modules == null)
                {
                    modules = new ICSLModule[0];
                }
                return ref modules;
            }
        }

        /// <summary>
        /// Community San Andreas Multiplayer Launcher pages reference
        /// </summary>
        private ref ICSLPage[] PagesReference
        {
            get
            {
                if (pages == null)
                {
                    pages = new ICSLPage[0];
                }
                return ref pages;
            }
        }

        /// <summary>
        /// Community San Andreas Multiplayer Launcher modules
        /// </summary>
        public ICSLModule[] Modules
        {
            get
            {
                return ModulesReference.Clone() as ICSLModule[];
            }
        }

        /// <summary>
        /// Community San Andreas Multiplayer Launcher pages
        /// </summary>
        public ICSLPage[] Pages
        {
            get
            {
                return PagesReference.Clone() as ICSLPage[];
            }
        }

        /// <summary>
        /// Global configuration
        /// </summary>
        public ACSLConfiguration GlobalConfiguration
        {
            get
            {
                if (configuration == null)
                {
                    configuration = new Configuration();
                }
                return configuration;
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            GlobalConfiguration.Load(Path.Combine(Environment.CurrentDirectory, "config.json"));
            ModulesData[] modules_data = ModulesProvider.LoadAll(configuration.ModulesPath);
            List<ICSLModule> modules = new List<ICSLModule>();
            List<ICSLPage> pages = new List<ICSLPage>();
            lookupModules.Clear();
            foreach (ModulesData md in modules_data)
            {
                if (!(lookupModules.ContainsKey(md.Name)))
                {
                    lookupModules.Add(md.Name, md);
                    modules.AddRange(md.Modules);
                    pages.AddRange(md.Pages);
                }
            }
            this.modules = modules.ToArray();
            this.pages = pages.ToArray();
            foreach (ICSLPage page in pages)
            {
                if (page != null)
                {
                    // TODO
                }
            }
            foreach (ICSLModule module in modules)
            {
                if (module != null)
                {
                    module.Init(this);
                }
            }
            modules.Clear();
            pages.Clear();
        }

        /// <summary>
        /// Get Community San Andreas Multiplayer Launcher modules by name
        /// </summary>
        /// <param name="modulesName">Community San Andreas Multiplayer Launcher modules name</param>
        /// <returns>Community San Andreas Multiplayer Launcher modules</returns>
        public ICSLModule[] GetModulesByName(string modulesName)
        {
            ICSLModule[] ret = null;
            if ((lookupModules != null) && (modulesName != null))
            {
                if (lookupModules.ContainsKey(modulesName))
                {
                    ret = lookupModules[modulesName].Modules;
                }
            }
            if (ret == null)
            {
                ret = new ICSLModule[0];
            }
            return ret;
        }

        /// <summary>
        /// Get Community San Andreas Multiplayer Launcher pages by name
        /// </summary>
        /// <param name="pagesName">Community San Andreas Multiplayer Launcher pages name</param>
        /// <returns>Community San Andreas Multiplayer Launcher pages</returns>
        public ICSLPage[] GetPagesByName(string pagesName)
        {
            ICSLPage[] ret = null;
            if ((lookupModules != null) && (pagesName != null))
            {
                if (lookupModules.ContainsKey(pagesName))
                {
                    ret = lookupModules[pagesName].Pages;
                }
            }
            if (ret == null)
            {
                ret = new ICSLPage[0];
            }
            return ret;
        }

        #region Events

        /// <summary>
        /// WIndow closed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closed(object sender, EventArgs e)
        {
            foreach (ICSLModule module in ModulesReference)
            {
                if (module != null)
                {
                    module.Exit();
                }
            }
            modules = null;
        }

        #endregion
    }
}
