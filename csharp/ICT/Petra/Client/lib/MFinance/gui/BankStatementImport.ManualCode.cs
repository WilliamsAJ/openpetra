﻿/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Data;
using System.Windows.Forms;
using Mono.Unix;
using Ict.Common;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.Interfaces;
using Ict.Petra.Shared.Interfaces.Plugins.MFinance;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Client.MFinance.Logic;

namespace Ict.Petra.Client.MFinance.Gui
{
    /// manual methods for the generated window
    public partial class TFrmBankStatementImport
    {
        private Int32 FLedgerNumber;

        /// <summary>
        /// use this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;

                TFinanceControls.InitialiseAccountList(ref cmbSelectBankAccount, FLedgerNumber, true, false, true, true);

                // we can only load the statements when the ledger number is known
                PopulateStatementCombobox();
                cmbSelectStatement.SelectedIndex = cmbSelectStatement.Items.Count - 1;
            }
        }

        private BankImportTDS FMainDS = new BankImportTDS();
        private DataView FMatchView = null;
        private DataView FTransactionView = null;

        private void InitializeManualCode()
        {
            pnlDetails.Visible = false;
            cmbSelectStatement.SelectedValueChanged += new EventHandler(SelectBankStatement);
        }

        private void SelectBankStatement(System.Object sender, EventArgs e)
        {
            // TODO: check if we want to save the changed matches?
            SaveMatches(null, null);

            CurrentlySelectedMatch = null;

            // load the transactions of the selected statement, and the matches
            FMainDS =
                TRemote.MFinance.ImportExport.WebConnectors.GetBankStatementTransactionsAndMatches(
                    cmbSelectStatement.GetSelectedInt32(), FLedgerNumber);

            grdAllTransactions.Columns.Clear();
            grdAllTransactions.AddTextColumn(Catalog.GetString("Nr"), FMainDS.AEpTransaction.ColumnOrder, 40);
            grdAllTransactions.AddTextColumn(Catalog.GetString("Account Name"), FMainDS.AEpTransaction.ColumnAccountName, 150);
            grdAllTransactions.AddTextColumn(Catalog.GetString("description"), FMainDS.AEpTransaction.ColumnDescription, 200);
            grdAllTransactions.AddDateColumn(Catalog.GetString("Date Effective"), FMainDS.AEpTransaction.ColumnDateEffective);
            grdAllTransactions.AddCurrencyColumn(Catalog.GetString("Transaction Amount"), FMainDS.AEpTransaction.ColumnTransactionAmount);

            FTransactionView = FMainDS.AEpTransaction.DefaultView;
            FTransactionView.AllowNew = false;
            FTransactionView.Sort = AEpTransactionTable.GetOrderDBName() + " ASC";
            grdAllTransactions.DataSource = new DevAge.ComponentModel.BoundDataView(FTransactionView);

            TFinanceControls.InitialiseMotivationDetailList(ref cmbMotivationDetail, FLedgerNumber, true);
            TFinanceControls.InitialiseCostCentreList(ref cmbGiftCostCentre, FLedgerNumber, true, false, true, true);
            TFinanceControls.InitialiseAccountList(ref cmbGiftAccount, FLedgerNumber, true, false, true, false);
            TFinanceControls.InitialiseCostCentreList(ref cmbGLCostCentre, FLedgerNumber, true, false, true, true);
            TFinanceControls.InitialiseAccountList(ref cmbGLAccount, FLedgerNumber, true, false, true, false);

            grdGiftDetails.Columns.Clear();
            grdGiftDetails.AddTextColumn(Catalog.GetString("Motivation"), FMainDS.AEpMatch.ColumnMotivationDetailCode, 100);
            grdGiftDetails.AddTextColumn(Catalog.GetString("Cost Centre"), FMainDS.AEpMatch.ColumnCostCentreCode, 150);
            grdGiftDetails.AddTextColumn(Catalog.GetString("Cost Centre Name"), FMainDS.AEpMatch.ColumnCostCentreName, 200);
            grdGiftDetails.AddCurrencyColumn(Catalog.GetString("Amount"), FMainDS.AEpMatch.ColumnGiftTransactionAmount);
            FMatchView = FMainDS.AEpMatch.DefaultView;
            FMatchView.AllowNew = false;
            grdGiftDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMatchView);

            TFinanceControls.InitialiseAccountList(ref cmbSelectBankAccount, FLedgerNumber, true, false, true, true);

            if (cmbSelectStatement.GetSelectedInt32() != -1)
            {
                FMainDS.AEpStatement.DefaultView.RowFilter = String.Format("{0}={1}",
                    AEpStatementTable.GetStatementKeyDBName(),
                    cmbSelectStatement.GetSelectedInt32());
                cmbSelectBankAccount.SetSelectedString(((AEpStatementRow)FMainDS.AEpStatement.DefaultView[0].Row).BankAccountCode);
                FMainDS.AEpStatement.DefaultView.RowFilter = string.Empty;
            }

            rbtListAll.Checked = true;
            grdAllTransactions.SelectRowInGrid(1);
        }

        private void PopulateStatementCombobox()
        {
            // TODO: add datetimepicker to toolstrip
            // see http://www.daniweb.com/forums/thread109966.html#
            // dtTScomponent = new ToolStripControlHost(dtMyDateTimePicker);
            // MainToolStrip.Items.Add(dtTScomponent);
            // DateTime.Now.AddMonths(-100);
            DateTime dateStatementsFrom = DateTime.MinValue;

            // update the combobox with the bank statements
            AEpStatementTable stmts = TRemote.MFinance.ImportExport.WebConnectors.GetImportedBankStatements(dateStatementsFrom);

            cmbSelectStatement.BeginUpdate();
            cmbSelectStatement.DataSource = stmts.DefaultView;
            cmbSelectStatement.DisplayMember = AEpStatementTable.GetDateDBName();
            cmbSelectStatement.ValueMember = AEpStatementTable.GetStatementKeyDBName();
            cmbSelectStatement.DataSource = stmts.DefaultView;
            cmbSelectStatement.EndUpdate();

            cmbSelectStatement.SelectedIndex = -1;
        }

        private void ImportNewStatement(System.Object sender, EventArgs e)
        {
            // look for available plugin for importing a bank statement.
            // the plugin will upload the data into the tables a_ep_statement and a_ep_transaction on the server/database
            string BankStatementImportPlugin = TAppSettingsManager.GetValueStatic("Plugin.BankStatementImport", "");

            if (BankStatementImportPlugin.Length == 0)
            {
                MessageBox.Show(Catalog.GetString("Please install a valid plugin for the import of bank statements!"));
                return;
            }

            // namespace of the class TBankStatementImport, eg. Plugin.BankImportFromCSV
            // the dll has to be in the normal application directory
            string Namespace = BankStatementImportPlugin;
            string NameOfDll = Namespace + ".dll";
            string NameOfClass = Namespace + ".TBankStatementImport";

            // dynamic loading of dll
            System.Reflection.Assembly assemblyToUse = System.Reflection.Assembly.LoadFrom(NameOfDll);
            System.Type CustomClass = assemblyToUse.GetType(NameOfClass);

            IImportBankStatement ImportBankStatement = (IImportBankStatement)Activator.CreateInstance(CustomClass);

            Int32 StatementKey;

            if (ImportBankStatement.ImportBankStatement(out StatementKey))
            {
                PopulateStatementCombobox();

                // select the loaded bank statement and display all transactions
                cmbSelectStatement.SetSelectedInt32(StatementKey);
            }
        }

        private void MotivationDetailChanged(System.Object sender, EventArgs e)
        {
            cmbGiftCostCentre.Enabled = false;
            cmbGiftAccount.Enabled = false;

            // look for the motivation detail.
            // if the associated cost centre is a summary cost centre,
            // the user can select from a list of the reporting costcentres
            // else make the costcentre readonly.
            if (cmbMotivationDetail.SelectedIndex == -1)
            {
                return;
            }

            DataView v = new DataView(FMainDS.AMotivationDetail);
            v.RowFilter = AMotivationDetailTable.GetMotivationDetailCodeDBName() +
                          " = '" + cmbMotivationDetail.GetSelectedString() + "'";

            if (v.Count == 0)
            {
                cmbGiftCostCentre.Enabled = false;
                return;
            }

            AMotivationDetailRow motivationDetailRow = (AMotivationDetailRow)v[0].Row;

            cmbGiftAccount.Filter = AAccountTable.GetAccountCodeDBName() + " = '" + motivationDetailRow.AccountCode + "'";

            v = new DataView(FMainDS.ACostCentre);
            v.RowFilter = ACostCentreTable.GetCostCentreCodeDBName() +
                          " = '" + motivationDetailRow.CostCentreCode + "'";

            if (v.Count == 0)
            {
                cmbGiftCostCentre.Enabled = false;
                return;
            }

            ACostCentreRow costCentreRow = (ACostCentreRow)v[0].Row;

            if (costCentreRow.PostingCostCentreFlag)
            {
                cmbGiftCostCentre.Filter = ACostCentreTable.GetCostCentreCodeDBName() +
                                           " = '" + costCentreRow.CostCentreCode + "'";
            }
            else
            {
                cmbGiftCostCentre.Filter = ACostCentreTable.GetCostCentreToReportToDBName() +
                                           " = '" + costCentreRow.CostCentreCode + "'";
                cmbGiftCostCentre.Enabled = true;
            }
        }

        private void NewTransactionCategory(System.Object sender, EventArgs e)
        {
            GetValuesFromScreen();
            CurrentlySelectedMatch = null;

            rbtGLWasChecked = rbtGL.Checked;
            rbtGiftWasChecked = rbtGift.Checked;
            rbtUnmatchedWasChecked = rbtUnmatched.Checked;

            pnlGiftEdit.Visible = rbtGift.Checked;
            pnlGLEdit.Visible = rbtGL.Checked;

            if (rbtGift.Checked)
            {
                // select first detail
                grdGiftDetails.SelectRowInGrid(1);
                AEpMatchRow match = GetSelectedMatch();
                txtDonorKey.Text = StringHelper.FormatStrToPartnerKeyString(match.DonorKey.ToString());
            }

            if (rbtGL.Checked)
            {
                DisplayGLDetails();
            }
        }

        private BankImportTDSAEpTransactionRow CurrentlySelectedTransaction = null;
        private BankImportTDSAEpMatchRow CurrentlySelectedMatch = null;
        private bool rbtGLWasChecked = false;
        private bool rbtGiftWasChecked = false;
        private bool rbtUnmatchedWasChecked = false;

        /// store current selections in the a_ep_match table
        private void GetValuesFromScreen()
        {
            if (cmbSelectStatement.GetSelectedInt32() != -1)
            {
                FMainDS.AEpStatement.DefaultView.RowFilter = String.Format("{0}={1}",
                    AEpStatementTable.GetStatementKeyDBName(),
                    cmbSelectStatement.GetSelectedInt32());
//TODO                ((AEpStatementRow)FMainDS.AEpStatement.DefaultView[0].Row).LedgerNumber = FLedgerNumber;
//TODO                ((AEpStatementRow)FMainDS.AEpStatement.DefaultView[0].Row).BankAccountCode = cmbSelectBankAccount.GetSelectedString();
                FMainDS.AEpStatement.DefaultView.RowFilter = String.Empty;
            }

            if (CurrentlySelectedMatch == null)
            {
                return;
            }

            if (rbtGiftWasChecked)
            {
                for (int i = 0; i < FMatchView.Count; i++)
                {
                    AEpMatchRow match = (AEpMatchRow)FMatchView[i].Row;
                    match.DonorKey = Convert.ToInt64(txtDonorKey.Text);
                    match.Action = MFinanceConstants.BANK_STMT_STATUS_MATCHED_GIFT;
                }

                CurrentlySelectedTransaction.MatchAction = MFinanceConstants.BANK_STMT_STATUS_MATCHED_GIFT;

                GetGiftDetailValuesFromScreen();

                // TODO: validation> calculate the sum of the gift details and check with the bank transaction amount
            }

            if (rbtGLWasChecked)
            {
                CurrentlySelectedMatch.Action = MFinanceConstants.BANK_STMT_STATUS_MATCHED_GL;
                CurrentlySelectedTransaction.MatchAction = MFinanceConstants.BANK_STMT_STATUS_MATCHED_GL;

                GetGLValuesFromScreen();
            }

            if (rbtUnmatched.Checked)
            {
                for (int i = 0; i < FMatchView.Count; i++)
                {
                    AEpMatchRow match = (AEpMatchRow)FMatchView[i].Row;
                    match.Action = MFinanceConstants.BANK_STMT_STATUS_UNMATCHED;
                }

                CurrentlySelectedTransaction.MatchAction = MFinanceConstants.BANK_STMT_STATUS_UNMATCHED;
            }
        }

        private void AllTransactionsFocusedRowChanged(System.Object sender, EventArgs e)
        {
            pnlDetails.Visible = true;

            GetValuesFromScreen();

            CurrentlySelectedMatch = null;

            CurrentlySelectedTransaction = ((BankImportTDSAEpTransactionRow)grdAllTransactions.SelectedDataRowsAsDataRowView[0].Row);

            // load selections from the a_ep_match table for the new row
            FMatchView.RowFilter = AEpMatchTable.GetMatchTextDBName() +
                                   " = '" + CurrentlySelectedTransaction.MatchText + "'";

            AEpMatchRow match = (AEpMatchRow)FMatchView[0].Row;

            if (match.Action == MFinanceConstants.BANK_STMT_STATUS_MATCHED_GIFT)
            {
                rbtGift.Checked = true;

                txtDonorKey.Text = StringHelper.FormatStrToPartnerKeyString(match.DonorKey.ToString());

                grdGiftDetails.SelectRowInGrid(1);
                grdAllTransactions.Focus();
            }
            else if (match.Action == MFinanceConstants.BANK_STMT_STATUS_MATCHED_GL)
            {
                rbtGL.Checked = true;
                DisplayGLDetails();
            }
            else
            {
                rbtUnmatched.Checked = true;
            }

            rbtGLWasChecked = rbtGL.Checked;
            rbtGiftWasChecked = rbtGift.Checked;
            rbtUnmatchedWasChecked = rbtUnmatched.Checked;
        }

        private void GiftDetailsFocusedRowChanged(System.Object sender, EventArgs e)
        {
            GetGiftDetailValuesFromScreen();
            CurrentlySelectedMatch = GetSelectedMatch();
            DisplayGiftDetails();
        }

        private BankImportTDSAEpMatchRow GetSelectedMatch()
        {
            DataRowView[] SelectedGridRow = grdGiftDetails.SelectedDataRowsAsDataRowView;

            if (SelectedGridRow.Length >= 1)
            {
                return (BankImportTDSAEpMatchRow)SelectedGridRow[0].Row;
            }

            return null;
        }

        private void DisplayGiftDetails()
        {
            CurrentlySelectedMatch = GetSelectedMatch();

            if (CurrentlySelectedMatch != null)
            {
                txtAmount.Text = CurrentlySelectedMatch.GiftTransactionAmount.ToString();

                if (CurrentlySelectedMatch.IsMotivationDetailCodeNull())
                {
                    cmbMotivationDetail.SelectedIndex = -1;
                }
                else
                {
                    cmbMotivationDetail.SetSelectedString(CurrentlySelectedMatch.MotivationDetailCode);
                }

                if (CurrentlySelectedMatch.IsAccountCodeNull())
                {
                    cmbGiftAccount.SelectedIndex = -1;
                }
                else
                {
                    cmbGiftAccount.SetSelectedString(CurrentlySelectedMatch.AccountCode);
                }

                if (CurrentlySelectedMatch.IsCostCentreCodeNull())
                {
                    cmbGiftCostCentre.SelectedIndex = -1;
                }
                else
                {
                    cmbGiftCostCentre.SetSelectedString(CurrentlySelectedMatch.CostCentreCode);
                }
            }
        }

        private void GetGiftDetailValuesFromScreen()
        {
            if (CurrentlySelectedMatch != null)
            {
                // TODO: support more motivation groups.
                CurrentlySelectedMatch.MotivationGroupCode = FMainDS.AMotivationDetail[0].MotivationGroupCode;
                CurrentlySelectedMatch.MotivationDetailCode = cmbMotivationDetail.GetSelectedString();
                CurrentlySelectedMatch.AccountCode = cmbGiftAccount.GetSelectedString();
                CurrentlySelectedMatch.CostCentreCode = cmbGiftCostCentre.GetSelectedString();
                CurrentlySelectedMatch.GiftTransactionAmount = Convert.ToDouble(txtAmount.Text);
                CurrentlySelectedMatch.DonorKey = Convert.ToInt64(txtDonorKey.Text);

                FMainDS.ACostCentre.DefaultView.RowFilter = String.Format("{0}='{1}'",
                    ACostCentreTable.GetCostCentreCodeDBName(), CurrentlySelectedMatch.CostCentreCode);

                if (FMainDS.ACostCentre.DefaultView.Count == 1)
                {
                    CurrentlySelectedMatch.CostCentreName = ((ACostCentreRow)FMainDS.ACostCentre.DefaultView[0].Row).CostCentreName;
                }

                FMainDS.ACostCentre.DefaultView.RowFilter = "";
            }
        }

        private void DisplayGLDetails()
        {
            // there is only one match?
            // TODO: support split GL transactions
            CurrentlySelectedMatch = (BankImportTDSAEpMatchRow)FMatchView[0].Row;

            if (CurrentlySelectedMatch != null)
            {
                if (CurrentlySelectedMatch.IsAccountCodeNull())
                {
                    cmbGLAccount.SelectedIndex = -1;
                }
                else
                {
                    cmbGLAccount.SetSelectedString(CurrentlySelectedMatch.AccountCode);
                }

                if (CurrentlySelectedMatch.IsCostCentreCodeNull())
                {
                    cmbGLCostCentre.SelectedIndex = -1;
                }
                else
                {
                    cmbGLCostCentre.SetSelectedString(CurrentlySelectedMatch.CostCentreCode);
                }

                if (!CurrentlySelectedMatch.IsReferenceNull())
                {
                    txtGLReference.Text = CurrentlySelectedMatch.Reference;
                }

                if (CurrentlySelectedMatch.IsNarrativeNull())
                {
                    txtGLNarrative.Text = CurrentlySelectedTransaction.Description;
                }
                else
                {
                    txtGLNarrative.Text = CurrentlySelectedMatch.Narrative;
                }
            }
        }

        private void GetGLValuesFromScreen()
        {
            if (CurrentlySelectedMatch != null)
            {
                CurrentlySelectedMatch.AccountCode = cmbGLAccount.GetSelectedString();
                CurrentlySelectedMatch.CostCentreCode = cmbGLCostCentre.GetSelectedString();
                CurrentlySelectedMatch.Reference = txtGLReference.Text;
                CurrentlySelectedMatch.Narrative = txtGLNarrative.Text;
            }
        }

        private Int32 NewMatchKey = -1;

        private void AddGiftDetail(System.Object sender, EventArgs e)
        {
            GetValuesFromScreen();

            // get a new detail number
            Int32 newDetailNumber = 0;
            double amount = 0;
            AEpMatchRow match = null;

            for (int i = 0; i < FMatchView.Count; i++)
            {
                match = (AEpMatchRow)FMatchView[i].Row;

                if (match.Detail >= newDetailNumber)
                {
                    newDetailNumber = match.Detail + 1;
                }

                amount += match.GiftTransactionAmount;
            }

            if (match != null)
            {
                AEpMatchRow newRow = FMainDS.AEpMatch.NewRowTyped();
                newRow.EpMatchKey = NewMatchKey--;
                newRow.MatchText = match.MatchText;
                newRow.Detail = newDetailNumber;
                newRow.LedgerNumber = match.LedgerNumber;
                newRow.AccountCode = match.AccountCode;
                newRow.CostCentreCode = match.CostCentreCode;
                newRow.DonorKey = match.DonorKey;
                newRow.GiftTransactionAmount = CurrentlySelectedTransaction.TransactionAmount -
                                               amount;
                FMainDS.AEpMatch.Rows.Add(newRow);

                // select the new gift detail
                grdGiftDetails.SelectRowInGrid(grdGiftDetails.Rows.Count - 1);
            }
        }

        private void RemoveGiftDetail(System.Object sender, EventArgs e)
        {
            GetValuesFromScreen();

            if (CurrentlySelectedMatch == null)
            {
                MessageBox.Show(Catalog.GetString("Please select a row before deleting a detail"));
                return;
            }

            // we should never allow to delete all details, otherwise we have nothing to copy from
            // also cannot delete the first detail, since there is the foreign key from a_ep_transaction on epmatchkey?
            if (CurrentlySelectedTransaction.EpMatchKey == CurrentlySelectedMatch.EpMatchKey)
            {
                MessageBox.Show(Catalog.GetString("Cannot delete the first detail"));
            }
            else
            {
                FMainDS.AEpMatch.Rows.Remove(CurrentlySelectedMatch);
                CurrentlySelectedMatch = null;

                // select the last gift detail
                grdGiftDetails.SelectRowInGrid(grdGiftDetails.Rows.Count - 1);
            }
        }

        private void SaveMatches(System.Object sender, EventArgs e)
        {
            GetValuesFromScreen();

            if (TRemote.MFinance.ImportExport.WebConnectors.CommitMatches(FMainDS))
            {
                FMainDS.AcceptChanges();
            }
            else
            {
                MessageBox.Show(Catalog.GetString(
                        "The matches could not be stored. Please ask your System Administrator to check the log file on the server."));
            }
        }

        private void CreateGiftBatch(System.Object sender, EventArgs e)
        {
            GetValuesFromScreen();

            // TODO: should we first ask? also when closing the window?
            SaveMatches(null, null);

            Int32 GiftBatchNumber = TRemote.MFinance.ImportExport.WebConnectors.CreateGiftBatch(FMainDS, FLedgerNumber, -1);

            if (GiftBatchNumber != -1)
            {
                MessageBox.Show(String.Format(Catalog.GetString("Please check Gift Batch {0}"), GiftBatchNumber));
            }
            else
            {
                MessageBox.Show(Catalog.GetString("Problem: No gift batch has been created"));
            }
        }

        private void CreateGLBatch(System.Object sender, EventArgs e)
        {
            GetValuesFromScreen();

            // TODO: should we first ask? also when closing the window?
            SaveMatches(null, null);

/*
 *          Int32 GLBatchNumber = TRemote.MFinance.ImportExport.WebConnectors.CreateGLBatch(FMainDS, FLedgerNumber, -1);
 *
 *          if (GLBatchNumber != -1)
 *          {
 *              MessageBox.Show(String.Format(Catalog.GetString("Please check GL Batch {0}"), GiftBatchNumber));
 *          }
 *          else
 *          {
 *              MessageBox.Show(Catalog.GetString("Problem: No GL batch has been created"));
 *          }
 */
        }

        private void TransactionFilterChanged(System.Object sender, EventArgs e)
        {
            GetValuesFromScreen();
            pnlDetails.Visible = false;
            CurrentlySelectedMatch = null;
            CurrentlySelectedTransaction = null;

            if (FTransactionView == null)
            {
                return;
            }

            if (rbtListAll.Checked)
            {
                FTransactionView.RowFilter = "";
            }
            else if (rbtListGift.Checked)
            {
                // TODO: allow splitting a transaction, one part is GL/AP, the other is a donation?
                //       at Top Level: split transaction, results into 2 rows in aeptransaction (not stored). Merge Transactions again?

                FTransactionView.RowFilter = String.Format("{0}='{1}'",
                    BankImportTDSAEpTransactionTable.GetMatchActionDBName(),
                    MFinanceConstants.BANK_STMT_STATUS_MATCHED_GIFT);
            }
            else if (rbtListUnmatched.Checked)
            {
                FTransactionView.RowFilter = String.Format("{0}='{1}'",
                    BankImportTDSAEpTransactionTable.GetMatchActionDBName(),
                    MFinanceConstants.BANK_STMT_STATUS_UNMATCHED);
            }
            else if (rbtListGL.Checked)
            {
                FTransactionView.RowFilter = String.Format("{0}='{1}'",
                    BankImportTDSAEpTransactionTable.GetMatchActionDBName(),
                    MFinanceConstants.BANK_STMT_STATUS_MATCHED_GL);
            }

            if (FTransactionView.Count > 0)
            {
                grdAllTransactions.SelectRowInGrid(1);
            }
        }
    }
}