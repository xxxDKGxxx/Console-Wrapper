# JetBrains_rekrutacja

This project is a Windows Forms application that allows executing system commands in either **CMD** or **PowerShell** and displays the command output in a real-time console-like interface. The application observes the output from both standard output (STDOUT) and error output (STDERR) streams and displays them with different colors for distinction.

The primary components of the project include:

- A form-based UI where users can input commands.
- A flexible command execution mechanism that supports both **CMD** and **PowerShell**.
- Real-time streaming and output management of command execution, with color-coded output for standard and error messages.

## Features

- **Console Type Selection**: Users can choose between CMD and PowerShell consoles to execute their commands.
- **Real-time Output**: STDOUT and STDERR streams are handled separately and displayed in real-time with different colors.
- **Task-Based Command Execution**: The application uses asynchronous tasks to execute commands and fetch results concurrently.
- **Cancellation Support**: Users can cancel running commands, stopping the process immediately.
  
## Technologies

- **C#** and **.NET Framework**: The application is built using C# in a Windows Forms environment.
- **Task-based Asynchronous Pattern**: Asynchronous execution of commands is handled using `async/await`.
- **Stream Observation**: Uses the Observer design pattern to manage output and error streams.

## Usage

### Setting Up

1. Clone this repository to your local machine:
    ```bash
    git clone https://github.com/yourusername/JetBrains_rekrutacja.git
    ```

2. Open the project in Visual Studio or your preferred C# IDE.

3. Build the project and run the application.

### Running the Application

1. Upon launching the application, you will see two buttons that let you choose the console type (`CMD` or `PowerShell`).

2. Enter a command into the `InputTextBox` and press **Enter** to execute the command.

3. The command's output will be displayed in the `OutputTextBox`, where:

   - **STDOUT** (normal output) will be shown in **green**.
   - **STDERR** (error output) will be shown in **red**.

4. The application also shows the execution time once the command finishes.

5. You can switch between **CMD** and **PowerShell** at any time by clicking the respective buttons.

### Canceling a Command

If you want to cancel a running command:

- You can stop the execution by clicking the **Cancel** button (if implemented).
- The process will be killed and the output will be terminated.

## Code Overview

### Form1 (Main UI)

This class handles the UI of the application. It includes methods for interacting with the user interface and setting up event handlers for key presses and button clicks.

- `textBox1_KeyPress`: Handles command execution when the Enter key is pressed.
- `consoleChooserCMD_Click` and `consoleChooserPowerShell_Click`: Handle the console type selection.
- `Notify`: This method receives output and error messages and updates the UI with colored text.

### CommandFactory

The `CommandFactory` class creates instances of `ICommand` based on the chosen console type (`CMD` or `PowerShell`). It delegates the command execution to either `CmdCommand` or `PowerShellCommand`.

### ICommand Interface

This interface defines the contract for executing commands asynchronously. Both `CmdCommand` and `PowerShellCommand` implement this interface.

### CmdCommand & PowerShellCommand

These classes are responsible for executing the respective commands in **CMD** and **PowerShell**.

- Each class takes a command string and executes it using the respective system process (`cmd.exe` or `PowerShell.exe`).
- They use `ProcessStartInfo` to configure the execution of the system processes and redirect their outputs (STDOUT and STDERR).

### ProcessExecutor (Stream Observer)

This class implements the **Observer** design pattern and manages the execution of processes. It subscribes to the output and error streams of the executed commands.

- `Run`: Starts the process and listens to both the output and error streams.
- `NotifySubscribers`: Notifies all subscribed observers (like `Form1`) when there is new output.
- `Cancel`: Cancels the process execution and stops the observers.

### IStreamObserver Interface

This interface defines a method `Notify` that is used to push output to any observer. In this case, `Form1` implements this interface and updates the UI with the command output.

### ISubject Interface

Defines methods for subscribing and unsubscribing observers. The `ProcessExecutor` class implements this interface to notify subscribed components (observers) about new output or errors.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

Feel free to extend and modify the application to suit your needs. Contributions are welcome!

