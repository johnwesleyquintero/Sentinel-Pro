<p align="center">
  <img src="wwwroot/Assets/Branding/logo.svg" alt="Sentinel Pro Logo" width="400">
</p>

<h1 align="center">Your Ultimate AI-Powered Workspace Guardian 🛡️</h1>

> Sentinel Pro is a sophisticated WPF application meticulously designed to safeguard your valuable work by managing workspace backups and restorations with unparalleled ease. Elevating your workflow, Sentinel Pro now integrates **Sentinel AI** 🤖, leveraging the power of the **Gemini API** for intelligent code assistance, natural language backup queries, and more. This comprehensive tool ensures your projects are always protected, instantly recoverable, and continuously optimized.

---

## 🌟 Key Features

### 💾 Core Backup & Workspace Management

| Feature                     | Description                                                                                                |
| :-------------------------- | :--------------------------------------------------------------------------------------------------------- |
| **Effortless Backup Mgmt**  | View, manage, and organize workspace backups with details like timestamps, descriptions, and size.           |
| **One-Click Restoration**   | Instantly restore previous workspace states with a single click.                                           |
| **Intuitive UI**            | Navigate a modern, user-friendly graphical interface designed for seamless interaction.                    |
| **Robust PowerShell Core**  | Leverages PowerShell for reliable and efficient backup and restoration operations.                         |
| **Detailed History**        | Track backup history with timestamps and descriptions.                                                     |
| **Scheduled Backups**       | Automate backups at regular intervals (daily, weekly, etc.).                                               |
| **Optimized Storage**       | Benefit from backup compression to minimize storage space usage.                                           |
| **Flexible Profiles**       | Create and manage multiple backup profiles for different projects or configurations.                       |
| **Backup Integrity Checks** | Ensure the integrity and reliability of your backups with built-in verification.                           |
| **Advanced Search**         | Quickly find specific backups using powerful search and filtering options.                                 |
| **Config Import/Export**    | Easily share or transfer backup configurations.                                                            |
| **Command-Line Interface**  | Automate operations and integrate Sentinel Pro into scripting workflows.                                   |
| **Email Notifications**     | Stay informed about backup success, failure, or warnings.                                                  |
| **Secure Encryption**       | Protect sensitive backup data with robust AES-256 encryption.                                              |
| **Differential Backups**    | Save time and storage space by backing up only changed files.                                              |
| **Direct Directory Access** | Easily browse backup contents directly from the file system.                                               |
| **Custom Storage Location** | Choose precisely where your backups are stored.                                                            |
| **Retention Policies**      | Automatically manage backup lifecycles by configuring retention rules.                                     |

### 🤖 AI-Powered Assistance (via Gemini API)

Sentinel Pro enhances your productivity with integrated AI features powered by Google's Gemini API.

| Feature                        | Description                                                                                                |
| :----------------------------- | :--------------------------------------------------------------------------------------------------------- |
| **Intelligent Code Assist**    | Get context-aware code suggestions and completions directly within your workflow.                          |
| **Code Explanation**           | Understand complex or unfamiliar code blocks with clear, natural language explanations.                    |
| **Basic Error Detection**      | Identify potential basic errors or issues in your code snippets.                                           |
| **Natural Language Queries**   | Ask questions about your backups or workspace status using everyday language (e.g., "Show recent backups"). |
| **AI-Enhanced Future**         | Actively developing more advanced AI features to further streamline your development process.              |

---

## 💻 System Requirements

-   **Operating System:** Windows 10 or later (64-bit recommended)
-   **Runtime:** .NET 8.0 or later (Included in installer)
-   **PowerShell:** PowerShell 7.4 or later recommended
-   **Disk Space:** 100MB minimum for application (additional space required for backups)
-   **Memory:** 4GB RAM minimum (8GB+ Recommended for optimal performance, especially with AI features)
-   **Gemini API:** Requires a valid API key and an active internet connection for AI features.

---

## 🚀 Getting Started

### For End Users

1.  **Download:** Get the latest installer (`SentinelPro-Setup.exe`) from our official landing page (hosted via `wwwroot`).
2.  **Install:** Run the setup wizard. It will guide you through the installation and ensure necessary dependencies like the .NET Runtime are present.
3.  **Configure:** On first launch, configure essential settings like your Gemini API Key (if using AI features) and desired backup locations via the Setup Wizard or application settings.
4.  **Launch & Use:** Start Sentinel Pro and begin managing your workspaces!

### For Developers

1.  **Clone Repository:** `git clone https://github.com/your-username/Sentinel-Pro.git`
2.  **Open Solution:** Open `SentinelPro.sln` in Visual Studio 2022 or later.
3.  **Restore Dependencies:** Ensure all NuGet packages are restored.
4.  **Configure:** Add your Gemini API Key in the appropriate configuration file (e.g., `appsettings.json` or user secrets) for testing AI features.
5.  **Build & Run:** Build the solution (Debug or Release) and run the `SentinelPro.WPF` project.

---

## ⚙️ Configuration

Key settings can be adjusted within the application or via configuration files (`appsettings.json`).

| Setting             | Purpose                                                                 | Notes                                          |
| :------------------ | :---------------------------------------------------------------------- | :--------------------------------------------- |
| **Backup Location** | Default directory for storing backups.                                  | Default: `__cleanup_backups__` in home folder. |
| **Gemini API Key**  | Your API key for accessing Google's Gemini AI services.                 | Required for all AI features.                  |
| **Compression**     | Adjust compression level (balance speed vs. size).                      |                                                |
| **Encryption Key**  | Key used for encrypting/decrypting backups (if enabled).                | Store securely.                                |
| **Schedules**       | Define automated backup intervals and times.                            |                                                |
| **Notifications**   | Configure email settings for backup alerts.                             | SMTP server details, recipients, etc.          |
| **Retention Rules** | Set policies for how long backups are kept (e.g., keep last 5).         |                                                |
| **Logging Level**   | Control the verbosity of application logs (Debug, Info, Error, etc.). |                                                |

---

## 🔧 Troubleshooting

| Issue                      | Potential Solution                                                                                             |
| :------------------------- | :------------------------------------------------------------------------------------------------------------- |
| **No Backups Displayed**   | Verify `rollback_info.json` exists and is accessible; check configured backup location permissions.            |
| **Error Messages**         | Review the status bar or application logs (`Logs` folder) for detailed error information.                      |
| **Backup/Restore Failures**| Ensure Sentinel Pro has necessary write/read permissions for source and target directories; check disk space. |
| **Slow Performance**       | Check system resources (CPU, RAM, Disk I/O); consider adjusting compression level or excluding large files.    |
| **AI Features Not Working**| Verify your Gemini API key is correct and active; ensure a stable internet connection.                         |

---

*(Sections for Performance Benchmarks, Security Features, Monitoring, CI/CD Pipeline, Quality Metrics, License, Contributing, and Support can remain largely the same as they describe the project's standards and processes, but ensure they are accurate.)*

---

## 🔒 Production Readiness

*(This section seems well-defined and showcases the project's maturity. Ensure the stated metrics like 100% code coverage are accurate.)*

---
## 🤝 Contributing

*(This section is well-defined and outlines the process for contributors. Ensure the guidelines are clear and concise.)*
