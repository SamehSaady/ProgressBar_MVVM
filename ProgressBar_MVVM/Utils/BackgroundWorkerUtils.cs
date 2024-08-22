using ProgressBar_MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ProgressBar_MVVM.Utils
{
    /// <summary>
    /// Provides utilities for managing background operations using a <see cref="BackgroundWorker"/> for a WPF <see cref="Window"/>.
    /// </summary>
    internal static class BackgroundWorkerUtils
    {
        private static MainWinVM _mainWinVM;
        private static Action _onWorkCompleted;
        private static Action<Exception> _onException;
        private static BackgroundWorker _backgroundWorker;
        private static DoWorkEventArgs _doWorkEventArgs;
        private static Action _work;
        private static string exitKey = "Exit_Work";



        /// <summary>
        /// Handles the <b><see cref="BackgroundWorker.DoWork"/></b> event by executing the specified work.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Contains the event data.</param>
        private static void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                _doWorkEventArgs = e;
                _work();
            }
            catch (Exception ex)
            {
                if (ex.Message == exitKey)
                    return;

                if(_onException != null)
                    _onException(ex);
            }

        }

        /// <summary>
        /// Handles the <b><see cref="BackgroundWorker.ProgressChanged"/></b> event by updating the progress value and status in the view model.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Contains the event data, including the progress percentage and status.</param>
        private static void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _mainWinVM.Progress = e.ProgressPercentage;
            _mainWinVM.Status = e.UserState.ToString();
        }

        /// <summary>
        /// Handles the <b><see cref="BackgroundWorker.RunWorkerCompleted"/></b> event by updating the status in the view model based on the operation result and calling <b><see cref="_onWorkCompleted"/></b> if successfully completed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Contains the event data, including information about cancellation, errors, or successful completion.</param>
        private static void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                _mainWinVM.Status = $"Status: Cancelled!";
            }
            else if (e.Error != null)
            {
                _mainWinVM.Status = $"Status: An error occurred:\n{e.Error.Message}";
            }
            else
            {
                _mainWinVM.Status = "Status: Completed!";
                _mainWinVM.Progress = _mainWinVM.MaxProgress;

                if (_onWorkCompleted != null)
                    _onWorkCompleted();
            }
        }




        /// <summary>
        /// Initializes the <see cref="BackgroundWorkerUtils"/> with the specified view model.
        /// </summary>
        /// <param name="mainWinVM">The view model that controls the main window's data.</param>
        /// <param name="onWorkCompleted">Called when the work is completed successfully (no errors nor cancelled).</param>
        /// <param name="onException">Called when an exception is raised.</param>
        public static void Initialize(MainWinVM mainWinVM, Action onWorkCompleted, Action<Exception> onException)
        {
            _mainWinVM = mainWinVM;
            _onWorkCompleted = onWorkCompleted;
            _onException = onException;

            _backgroundWorker = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            _backgroundWorker.DoWork += BackgroundWorker_DoWork;
            _backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            _backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
        }

        /// <summary>
        /// Starts the background <paramref name="work"/> asynchronously.
        /// </summary>
        /// <param name="work">The work to be performed in the background.</param>
        public static void RunAsync(Action work)
        {
            if (_backgroundWorker.IsBusy)
                return;

            _work = work;
            _mainWinVM.Progress = 0;
            _mainWinVM.Status = "Status: In Progress..";
            _backgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Requests cancellation of the currently running background operation.
        /// </summary>
        /// <remarks>
        /// The cancellation is implemented once <b><see cref="SetProgress(int, string)"/></b> or <b><see cref="IncreaseProgress(int, string)"/></b> are called.
        /// </remarks>
        public static void CancelAsync()
        {
            if (_backgroundWorker.IsBusy)
            {
                _backgroundWorker.CancelAsync();  // Set the cancellation flag.
                // [_backgroundWorker.CancellationPending] will now be [true].
            }
        }

        /// <summary>
        /// Sets the progress of the background operation to the specified value and optionally updates the status.
        /// </summary>
        /// <param name="value">The progress value to set.</param>
        /// <param name="status">
        /// The status message to update. If <c>null</c>, the current status in the view model is used.
        /// </param>
        /// <remarks>
        /// If the <paramref name="value"/> exceeds the <b>MaxProgress</b>, the <b>MaxProgress</b> will be used.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown when the operation is canceled, to exit the <see cref="BackgroundWorker.DoWork"/> event handler.
        /// </exception>
        public static void SetProgress(int value, string status = null)
        {
            // Check if cancellation was requested:
            if (_backgroundWorker.CancellationPending)
            {
                _doWorkEventArgs.Cancel = true;  // This will set [e.Cancelled] inside [RunWorkerCompleted] to [true].
                throw new Exception(exitKey);  // Exiting the [DoWork] handler and by then, [RunWorkerCompleted] will be triggered.
            }

            if (value >= _mainWinVM.MaxProgress)
                return;

            _backgroundWorker.ReportProgress(value, status?? _mainWinVM.Status);
        }

        /// <summary>
        /// Increases the progress of the background operation by the specified increment and optionally updates the status.
        /// </summary>
        /// <param name="increment">The amount by which to increase the progress.</param>
        /// <param name="status">
        /// The status message to update. If <c>null</c>, the current status in the view model is used.
        /// </param>
        /// <remarks>
        /// If the progress value exceeds the <b>MaxProgress</b>, the <b>MaxProgress</b> will be used.
        /// </remarks>
        public static void IncreaseProgress(int increment, string status = null)
        {
            SetProgress(_mainWinVM.Progress + increment, status);
        }

    }
}
