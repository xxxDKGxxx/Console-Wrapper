using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace JetBrains_rekrutacja
{
    /// <summary>
    /// Enumeration representing the console types (CMD or PowerShell).
    /// </summary>
    public enum ConsoleType
    {
        CMD,
        POWERSHELL
    }

    /// <summary>
    /// Enumeration representing the stream types (STDOUT or STDERR).
    /// </summary>
    public enum StreamType
    {
        STDOUT,
        STDERR
    }

    /// <summary>
    /// Main form for the application, implementing IStreamObserver to handle the output streams.
    /// </summary>
    public partial class Form1 : Form, IStreamObserver
    {
        private SynchronizationContext synchronizationContext;

        private ConsoleType consoleType = ConsoleType.CMD;

        /// <summary>
        /// Initializes the form and subscribes to the process executor for output notifications.
        /// </summary>
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

        /// <summary>
        /// Event handler for key press in the input text box. Executes the command when Enter is pressed.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">Event arguments containing the key press information.</param>
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

        /// <summary>
        /// Event handler for CMD console type button click. Switches the console type to CMD.
        /// </summary>
        private void consoleChooserCMD_Click(object sender, EventArgs e)
        {
            consoleType = ConsoleType.CMD;
            consoleChooserPowerShell.Checked = false;
            consoleChooserPowerShell.BackColor = Color.Gray;
            // consoleChooserCMD.Checked = true;
            consoleChooserCMD.BackColor = Color.Pink;
        }

        /// <summary>
        /// Event handler for PowerShell console type button click. Switches the console type to PowerShell.
        /// </summary>
        private void consoleChooserPowerShell_Click(object sender, EventArgs e)
        {
            consoleType = ConsoleType.POWERSHELL;
            // consoleChooserPowerShell.Checked = true;
            consoleChooserPowerShell.BackColor = Color.Pink;
            consoleChooserCMD.Checked = false;
            consoleChooserCMD.BackColor = Color.Gray;
        }

        /// <summary>
        /// Method that handles notifications about command output or error.
        /// Updates the UI to show STDOUT in green and STDERR in red.
        /// </summary>
        /// <param name="output">The output string to display.</param>
        /// <param name="st">The type of stream (STDOUT or STDERR).</param>
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

        /// <summary>
        /// Event handler for form closing. Cancels the process if it is running.
        /// </summary>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ProcessExecutor.GetInstance().Cancel();
        }

        /// <summary>
        /// Event handler for key down in the input text box. Cancels the running process if Ctrl+C is pressed.
        /// </summary>
        /// <param name="sender">The object that triggered the event.</param>
        /// <param name="e">Event arguments containing the key down information.</param>
        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C && ProcessExecutor.GetInstance().IsRunning)
            {
                ProcessExecutor.GetInstance().Cancel();
                OutputTextBox.SelectionColor = Color.White;
                OutputTextBox.AppendText("\nExecution Canceled.\n");
            }
        }
    }
}
