using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace JetBrains_rekrutacja
{

    public enum ConsoleType
    {
        CMD,
        POWERSHELL
    }

    public enum StreamType
    {
        STDOUT,
        STDERR
    }

    public partial class Form1 : Form, IStreamObserver
    {

        SynchronizationContext synchronizationContext;
        private ConsoleType consoleType = ConsoleType.CMD;


        public Form1()
        {
            InitializeComponent();
            // OutputTextBox.SelectionColor = Color.Green;
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            consoleChooserCMD.BackColor = Color.Pink;
            consoleChooserPowerShell.BackColor = Color.Gray;
            ProcessExecutor.GetInstance().Subscribe(this);
            synchronizationContext = SynchronizationContext.Current;
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void OutputTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private async void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter) return;
            OutputTextBox.Invoke((MethodInvoker)(() => OutputTextBox.AppendText("Executing...\n")));
            Application.DoEvents();
            OutputTextBox.Refresh();

            Stopwatch sw = Stopwatch.StartNew();

            string commandStr = InputTextBox.Text;
            ICommand command = CommandFactory.CreateCommand(commandStr, consoleType);
            await command.ExecuteAsync();
            sw.Stop();
            OutputTextBox.AppendText($"Execution finished! Time: {sw.Elapsed.TotalSeconds:F3}s\n");
        }

        private void consoleChooserCMD_Click(object sender, EventArgs e)
        {
            consoleType = ConsoleType.CMD;
            consoleChooserPowerShell.Checked = false;
            consoleChooserPowerShell.BackColor = Color.Gray;
            // consoleChooserCMD.Checked = true;
            consoleChooserCMD.BackColor = Color.Pink;
        }

        private void consoleChooserPowerShell_Click(object sender, EventArgs e)
        {
            consoleType = ConsoleType.POWERSHELL;
            // consoleChooserPowerShell.Checked = true;
            consoleChooserPowerShell.BackColor = Color.Pink;
            consoleChooserCMD.Checked = false;
            consoleChooserCMD.BackColor = Color.Gray;
        }

        public void Notify(string output, StreamType st)
        {
            try
            {
                switch (st)
                {
                    case StreamType.STDERR:
                        synchronizationContext.Post((arg) =>
                        {
                            OutputTextBox.SelectionColor = Color.Red;
                            OutputTextBox.AppendText(output);
                            OutputTextBox.ScrollToCaret();
                            OutputTextBox.Refresh();
                        }, null);
                        break;
                    case StreamType.STDOUT:
                        synchronizationContext.Post((arg) =>
                        {
                            OutputTextBox.SelectionColor = Color.Green;
                            OutputTextBox.AppendText(output);
                            OutputTextBox.ScrollToCaret();
                            OutputTextBox.Refresh();
                        }, null);
                        break;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ProcessExecutor.GetInstance().Cancel();
        }

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control && e.KeyCode == Keys.C && ProcessExecutor.GetInstance().IsRunning)
            {
                ProcessExecutor.GetInstance().Cancel();
                OutputTextBox.SelectionColor = Color.White;
                OutputTextBox.AppendText("\nExecution Canceled.\n");
            }
        }
    }
}
