using ProgressBar_MVVM.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgressBar_MVVM.ViewModels
{
    internal class MainWinVM : NotifyPropertyChangedBase
    {
        private int progress;
        private string status;

        /// <summary>
        /// The maximum progress value of the <b>ProgressBar</b>.
        /// </summary>
        public int MaxProgress { get; } = 100;
        /// <summary>
        /// The current progress value of the <b>ProgressBar</b>.
        /// </summary>
        public int Progress
        {
			get => progress;
            set => SetProperty(ref progress, value);
		}
        /// <summary>
        /// The loading status of the <b>Status_TB</b>.
        /// </summary>
        public string Status
        {
            get => status;
            set => SetProperty(ref status, value);
        }



        public MainWinVM()
        {
            Status = "Status: ";
        }

    }
}
