# SENTINEL PRO ROADMAP - PROJECT MANAGEMENT - MILESTONE TRACKING

## 1.0.0 Release Candidate

**FYI:**

1.  We have a landing page at `wwwroot/index.html` this serves as our landing page where users can download our releases and therefore, we should keep this directory `wwwroot` and don't delete `netlify.toml` as they host our landing page.
2.  Don't delete this `TODO.md` too because how would we track our development progress if we dont have this?

---

### 🧹 Codebase Health & Refactoring

*   [x] **Codebase Cleanup:** Remove old references from `WorkspaceCleanup` to `SentinelPro` across the entire codebase. *(Already marked done)*
*   [ ] **Dependency Injection:** Refactor `ServiceLocator` (if used) or implement standard .NET Dependency Injection (e.g., `Microsoft.Extensions.DependencyInjection`) for better testability and maintainability.
*   [ ] **Configuration Validation:** Implement validation logic for `appsettings.json` on startup to ensure required settings (like Gemini API Key, Backup Location) are present and potentially valid.
*   [ ] **Optimize Service Dependencies:** Review service interactions and dependencies to reduce coupling and improve modularity.
*   [ ] **Project Structure Review:** Analyze the solution structure (`SentinelPro.sln`) and remove any unused projects or files.
*   [ ] **Ollama Code Removal:** Thoroughly scan and remove all code, configuration, and references related to the previous Ollama implementation.

### ✨ Core Feature Implementation & Enhancement

*   [ ] **Implement Setup Wizard:** Create the initial Setup Wizard flow to guide users through essential configurations (Backup Location, Gemini Key, optional: Schedules, Encryption).
*   [ ] **Implement Scheduled Backups:** Ensure the scheduling feature (daily, weekly, etc.) is fully functional based on user configuration.
*   [ ] **Implement Backup Encryption:** Add robust AES-256 encryption/decryption logic for backups, managed via configuration and potentially a user-set key/password.
*   [ ] **Implement Differential Backups:** Develop the logic to identify changed files and create differential backups to save space and time.
*   [ ] **Implement Retention Policies:** Create the mechanism to automatically delete older backups based on configured rules (e.g., keep last N backups, keep backups younger than X days).
*   [ ] **Implement Email Notifications:** Integrate SMTP client logic to send notifications for backup success/failure/warnings based on user settings.
*   [ ] **Implement Backup Integrity Checks:** Add a feature (manual or automatic post-backup) to verify the integrity of backup archives.
*   [ ] **Refine Command-Line Interface (CLI):** Ensure the CLI allows for automating key operations like backup creation, restoration, and potentially configuration management.

### 🤖 AI Feature Integration (Gemini API)

*   [ ] **Integrate Gemini API Client:** Set up the core service/client for interacting with the Gemini API using the configured API key.
*   [ ] **Implement AI - Intelligent Code Assist:** Develop the UI/logic to provide context-aware code suggestions within the relevant application context (if applicable, or clarify where this feature manifests).
*   [ ] **Implement AI - Code Explanation:** Build the feature allowing users to select code (or provide snippets) and get explanations via the Gemini API.
*   [ ] **Implement AI - Basic Error Detection:** Implement the feature to analyze code snippets using Gemini for potential basic errors.
*   [ ] **Implement AI - Natural Language Queries:** Develop the interface and logic to parse user questions about backups (e.g., "show backups from last week") and query the backup history/metadata, potentially using Gemini for understanding the query.
*   [ ] **Gemini API Error Handling:** Implement robust error handling for API calls (e.g., invalid key, network issues, rate limits, API errors).

### 🧪 Testing & Quality Assurance

*   [ ] **Unit Tests - Core Logic:** Write unit tests for critical backup, restore, encryption, and differential backup logic.
*   [ ] **Unit Tests - ViewModels:** Add unit tests for key ViewModels to verify command execution, property changes, and interaction logic.
*   [ ] **Unit Tests - Services:** Add unit tests for core services (e.g., `BackupService`, `ConfigurationService`, `NotificationService`, `MonitoringService`).
*   [ ] **Integration Tests - Gemini API:** Create integration tests to verify the interaction with the live Gemini API (requires careful handling of API keys, consider using user secrets or environment variables in test environments).
*   [ ] **Integration Tests - File System:** Write integration tests for backup/restore operations interacting with the actual file system.

### 📚 Documentation

*   [ ] **Update README.md:** Ensure all features listed in the README (especially new AI ones and core features being implemented) are accurate and reflect the current state. Add details about the Setup Wizard.
*   [ ] **Update `wwwroot` README:** Improve the documentation within the `wwwroot` directory if it serves as more than just the landing page host (e.g., if it contains user guides). *(Self-correction: README.md seems the main doc source, this might be low priority unless `wwwroot` has separate docs)*.
*   [ ] **Document Service Architecture:** Create internal documentation (e.g., in `/docs` or using XML comments) explaining the responsibilities and interactions of the key services.
*   [ ] **Document Configuration:** Clearly document all settings available in `appsettings.json` and their purpose/valid values.
*   [ ] **Create Visual Project Structure Diagram:** Generate or draw a diagram illustrating the main components and dependencies of the application.

### 🚀 Build, Deployment & User Experience

*   [ ] **Configure GitHub Actions Pipeline:** Set up the CI/CD pipeline to build the WPF application, run tests, and package the installer.
*   [ ] **Pipeline: Publish Installer:** Configure the pipeline to place the final `SentinelPro-Setup.exe` into the `/wwwroot/publish/` directory within the repository structure *before* deployment (or as part of the deployment artifact).
*   [ ] **Fix Landing Page Link:** Investigate and resolve the "Cannot GET /wwwroot/publish/SentinelPro-Setup.exe" issue on the Netlify-hosted landing page. Ensure the path is correct relative to how Netlify serves the site.
*   [ ] **Improve Landing Page Navigation:** Review and enhance the navigation links on `wwwroot/index.html` for clarity and usability.
*   [ ] **Logging Implementation Review:** Enhance and standardize logging throughout the application using the configured `LoggingService`. Ensure useful information is logged at appropriate levels (Debug, Info, Warn, Error).
*   [ ] **Performance Monitoring:** Implement basic performance monitoring for key operations (e.g., backup duration, restore duration) and log the results.

### ✅ Final Release Steps (1.0.0)

*   [ ] **Final Testing:** Perform thorough end-to-end testing of all features.
*   [ ] **Create Installer:** Generate the final `SentinelPro-Setup.exe` including the Setup Wizard.
*   [ ] **Publish Installer:** Ensure the installer is correctly placed in `wwwroot/publish/` and accessible via the landing page.
*   [ ] **Tag Release:** Create a Git tag for version 1.0.0.
*   [ ] **Update Changelog:** Finalize the changelog for the 1.0.0 release.

---
