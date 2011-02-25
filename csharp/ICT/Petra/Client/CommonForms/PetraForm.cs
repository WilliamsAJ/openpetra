//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christiank
//
// Copyright 2004-2010 by OM International
//
// This file is part of OpenPetra.org.
//
// OpenPetra.org is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra.org is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Resources;
using System.Threading;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Common.Controls;

//using Ict.Petra.Client.CommonDialogs;
using Ict.Common;

namespace Ict.Petra.Client.CommonForms
{
#if TODO
    /// This Form is the Base Form for all Petra Forms.
    ///
    /// It contains the File and Help Menus, a Toolbar with a Close button,
    /// a Statusbar and a StatusBarTextProvider.
    ///
    /// @Comment All Forms that are used in Petra should inherit from this Form -
    ///   except for Dialog-style (Modal) Forms, these should inherit from
    ///   TFrmPetraDialog (in Ict.Petra.Client.CommonDialogs.dll)!
    public class TFrmPetra : Form
    {
        /// <summary>
        /// Special property to determine whether our code is running in the WinForms Designer.
        /// The result of this property is correct even if InitializeComponent() wasn't run yet
        /// (.NET's DesignMode property returns false in that case)!
        /// </summary>
        protected bool InDesignMode
        {
            get
            {
                return (this.GetService(typeof(IDesignerHost)) != null)
                       || (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime);
            }
        }
    }
#endif

    /// <summary>todoComment</summary>
    public delegate void ActionEventHandler(object sender, ActionEventArgs e);

    /// <summary>
    /// this class provides some useful methods for a Petra form
    /// </summary>
    public class TFrmPetraUtils
    {
        /// <summary>
        /// will set the help text for each control, when it gets the focus
        /// </summary>
        private TExtStatusBarHelp FStatusBar;

        /// <summary>
        /// This is a reference to the WinForm that contains this Petra object
        /// this object implements the IFrmPetra interface
        /// </summary>
        protected IFrmPetra FTheForm;

        /// <summary>
        /// points to the same object as FTheForm, but already casted to a WinForm
        /// </summary>
        protected System.Windows.Forms.Form FWinForm;

        /// Tells whether the Form is activated for the first time (after loading the Form) or not
        protected Boolean FFormActivatedForFirstTime;

        /// Set this to true to prevent the automatic hookup of change Events of all Controls on the Form
        protected Boolean FNoAutoHookupOfAllControls;

        /// This will hold a reference to ALL controls on the screen  even if they are buried in groub boxes, panels, or tab pages
        protected ArrayList FAllControls;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ACallerWindowHandle">the int handle of the form that has opened this window; needed for focusing when this window is closed later</param>
        /// <param name="ATheForm"></param>
        /// <param name="AStatusBar"></param>
        public TFrmPetraUtils(IntPtr ACallerWindowHandle, IFrmPetra ATheForm, TExtStatusBarHelp AStatusBar)
        {
            FFormActivatedForFirstTime = true;

            FTheForm = ATheForm;
            FWinForm = (Form)ATheForm;
            FStatusBar = AStatusBar;

            TFormsList.GFormsList.NotifyWindowOpened(ACallerWindowHandle, FWinForm.Handle);

            // WriteToStatusBar(Catalog.GetString("Ready."));
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TFrmPetra_Activated(System.Object sender, System.EventArgs e)
        {
            if (FFormActivatedForFirstTime == true)
            {
                // prevent this happening again
                FFormActivatedForFirstTime = false;

                // do any low level initialisation
                LocalRunOnceOnActivation();

                // virtual method, overriden in child forms
                RunOnceOnActivation();
            }
        }

        /// <summary>
        /// just call this function to clean up when closing the form
        /// </summary>
        public void Close()
        {
            // to prevent strange error message, that would stop the form from closing
            FFormActivatedForFirstTime = false;
        }

        /** used to allow subforms to initialise
         */
        public void LocalRunOnceOnActivation()
        {
            if (!FNoAutoHookupOfAllControls)
            {
                HookupAllControls();
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void RunOnceOnActivation()
        {
            if (FTheForm != null)
            {
                FTheForm.RunOnceOnActivation();
            }
        }

        /** used to iterate through the controls on the form
         */
        public void EnumerateControls(Control c)
        {
            foreach (Control ctrl in c.Controls)
            {
                // recurse into children;
                // but special case for UpDownBase/NumericUpDown, because we don't want the child controls of that
                if ((ctrl.HasChildren == true) && !(ctrl is UpDownBase))
                {
                    EnumerateControls(ctrl);
                }
                else
                {
                    FAllControls.Add(ctrl);
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public virtual void HookupAllControls()
        {
            FAllControls = new ArrayList();
            EnumerateControls(FWinForm); //this adds all controls on form to ArrayList

            // this is on an international version of Windows, so we want no bold fonts
            // because the letters are difficult to read
            bool changeFonts = TAppSettingsManager.ChangeFontForLocalisation();

            foreach (Control ctrl in FAllControls)
            {
                if (changeFonts)
                {
                    if (TAppSettingsManager.ReplaceFont(ctrl.Font))
                    {
                        // remove bold and replace with regular
                        ctrl.Font = new System.Drawing.Font(ctrl.Font.Name,
                            ctrl.Font.Size,
                            System.Drawing.FontStyle.Regular);
                    }
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="container"></param>
        public virtual void HookupAllInContainer(Control container)
        {
            // not implemented here
        }

        /// Set this to true to prevent the automatic hookup of change Events of all Controls on the Form
        public Boolean NoAutoHookupOfAllControls
        {
            set
            {
                FNoAutoHookupOfAllControls = value;
            }
        }

        #region Enable and Disable Action

        /// <summary>
        /// todoComment
        /// </summary>
        public event ActionEventHandler ActionEnablingEvent;

        private SortedList <string, bool>FActionStates = new SortedList <string, bool>();

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AActionName"></param>
        /// <param name="enable"></param>
        public void EnableAction(string AActionName, bool enable)
        {
            // store action enabled
            if (FActionStates.ContainsKey(AActionName))
            {
                FActionStates[AActionName] = enable;
            }
            else
            {
                FActionStates.Add(AActionName, enable);
            }

            //trigger event
            ActionEnablingEvent(this, new ActionEventArgs(AActionName, enable));
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AActionName"></param>
        /// <returns></returns>
        public bool IsEnabled(string AActionName)
        {
            if (FActionStates.ContainsKey(AActionName))
            {
                return FActionStates[AActionName];
            }

            return true;
        }

        /// <summary>todoComment</summary>
        public bool FormActivatedForFirstTime
        {
            get
            {
                return FFormActivatedForFirstTime;
            }
            set
            {
                FFormActivatedForFirstTime = value;
            }
        }

        /// useful for initialising actions, eg based on permissions
        virtual public void InitActionState()
        {
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Form_KeyDown(System.Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                ExecuteAction(eActionId.eClose);
            }
            else if (e.KeyCode == Keys.F1)
            {
                ExecuteAction(eActionId.eHelp);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Mni_Click(System.Object sender, System.EventArgs e)
        {
            if (!(sender is ToolStripItem))
            {
                return;
            }

            ExecuteAction((TItemTag)((ToolStripItem)sender).Tag);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ATag"></param>
        public void ExecuteAction(TItemTag ATag)
        {
            ExecuteAction(ATag.Id);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="id"></param>
        public void ExecuteAction(eActionId id)
        {
            switch (id)
            {
                case eActionId.eClose:
                    this.Close();
                    FWinForm.Close();
                    break;

                case eActionId.eHelpDevelopmentTeam:
#if TODO
                    using (DevelopmentTeamDialog teamDialog = new DevelopmentTeamDialog())
                    {
                        teamDialog.ShowDialog();
                    }
#endif
                    break;

                case eActionId.eHelpAbout:
#if TODO
                    using (AboutPetraDialog aboutDialog = new AboutPetraDialog())
                    {
                        aboutDialog.ShowDialog();
                    }
#endif
                    break;

                case eActionId.eHelp:
                {
                    // TODO help action
                }
                break;
            }
        }

        /// <summary>
        /// add the form to the forms list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TFrmPetra_Load(System.Object sender, System.EventArgs e)
        {
            TFormsList.GFormsList.Add(FWinForm);
        }

        /**
         * Event Handler that is invoked when the Form is about to close - no matter
         * how the closing was invoked (by calling Form.Close, a Close button, the
         * x Button of a Form, etc).
         *
         * @param sender Event sender
         * @param e EventArgs that allow cancelling of the closing
         *
         */
        public virtual void TFrmPetra_Closing(System.Object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!CanClose() || !FTheForm.CanClose())
            {
                // MessageBox.Show('TFrmPetra.TFrmPetra_Closing: e.Cancel := true');
                e.Cancel = true;
            }
            else
            {
                // MessageBox.Show('TFrmPetra.TFrmPetra_Closing: GFormsList.Remove(Self as Form)');
                TFormsList.GFormsList.NotifyWindowClose(this.FWinForm.Handle);
                TFormsList.GFormsList.Remove(FWinForm);
            }
        }

        #endregion

        #region Helper Functions

        /**
         * This function can be used to write to the StatusBar.
         *
         * TLogging can use it as a callback procedure, so it does not need to know
         * about the StatusBar itself
         *
         * @param s the text to be displayed in the StatusBar
         *
         */
        public void WriteToStatusBar(String s)
        {
            // StatusBar appears to be threadsafe; otherwise you would need a Invoke(System.Delegate(@myDelegate)); call
            FStatusBar.ShowMessage(s);
        }

        /**
         * This function tells the caller whether the window can be closed.
         * It can be used to find out if something is still edited, for example.
         *
         * @return true if window can be closed
         *
         */
        virtual public bool CanClose()
        {
            return true;
        }

        #endregion

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AControl"></param>
        /// <param name="AHelpText"></param>
        public void SetStatusBarText(Control AControl, string AHelpText)
        {
            FStatusBar.SetHelpText(AControl, AHelpText);
        }

        const Int16 MAX_COMBOBOX_HISTORY = 30;

        /// <summary>
        /// add new value of combobox to the user defaults, or move existing value to the front;
        /// limits the number of values to MAX_COMBOBOX_HISTORY
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        public void AddComboBoxHistory(System.Object Sender, TAcceptNewEntryEventArgs e)
        {
            string keyName = "CmbHistory" + ((Control)Sender).Name;
            StringCollection values = StringHelper.StrSplit(TUserDefaults.GetStringDefault(keyName, ""), ",");

            values.Remove(e.ItemString);
            values.Insert(0, e.ItemString);

            while (values.Count > MAX_COMBOBOX_HISTORY)
            {
                values.RemoveAt(values.Count - 1);
            }

            TUserDefaults.SetDefault(keyName, StringHelper.StrMerge(values, ","));
        }

        /// <summary>
        /// load the history of a combobox for auto completion from the user defaults
        /// </summary>
        /// <param name="AComboBox"></param>
        public void LoadComboBoxHistory(TCmbAutoComplete AComboBox)
        {
            AComboBox.SetDataSourceStringList(TUserDefaults.GetStringDefault("CmbHistory" + AComboBox.Name, ""));
        }
    }

    /// <summary>todoComment</summary>
    public enum eActionId
    {
        /// <summary>todoComment</summary>
        eHelp,

        /// <summary>todoComment</summary>
        eAbout,

        /// <summary>todoComment</summary>
        eClose,

        /// <summary>todoComment</summary>
        eHelpDevelopmentTeam,

        /// <summary>todoComment</summary>
        eHelpAbout,

        /// <summary>todoComment</summary>
        eBugReport
    };

    /// <summary>
    /// this is a helper class to identify menu items and toolbar buttons
    /// </summary>
    public class TItemTag
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="AId">identifier for the type of menu item</param>
        public TItemTag(eActionId AId)
        {
            id = AId;
        }

        private eActionId id;

        /// <summary>todoComment</summary>
        public eActionId Id
        {
            get
            {
                return id;
            }
        }
    }

    /// <summary>todoComment</summary>
    public class PetraForm
    {
        /// <summary>todoComment</summary>
        public const Int32 AUTOSCALEBASESIZEWIDTHFOR96DPI = 6;
    }

    /// <summary>todoComment</summary>
    public interface IFrmPetra
    {
        /// <summary>todoComment</summary>
        void RunOnceOnActivation();

        /// <summary>todoComment</summary>
        bool CanClose();

        /// <summary>todoComment</summary>
        TFrmPetraUtils GetPetraUtilsObject();
    }
}