using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace yt_dlp_loader
{
    internal class FormActionRunner
    {
        private readonly IWin32Window owner;

        public FormActionRunner(IWin32Window owner)
        {
            this.owner = owner;
        }

        public void Run(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        public async Task RunAsync(Button? busyButton, Func<Task> action)
        {
            if (busyButton != null)
            {
                busyButton.Enabled = false;
            }

            try
            {
                await action();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
            finally
            {
                if (busyButton != null)
                {
                    busyButton.Enabled = true;
                }
            }
        }

        public void ShowWarning(string message)
        {
            ShowMessage(message);
        }

        private void ShowError(string message)
        {
            ShowMessage(message);
        }

        private void ShowMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            MessageBox.Show(owner, message, "yt-dlp_loader", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
