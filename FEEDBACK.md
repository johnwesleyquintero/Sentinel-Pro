Acknowledged: `StartupUri` has been removed from `App.xaml`.

**Next Step: Implement Dependency Injection using `Microsoft.Extensions.DependencyInjection`**

This is a crucial step for improving testability, maintainability, and decoupling components. We will use the standard .NET Generic Host builder, which integrates configuration, logging, and dependency injection seamlessly.

**Detailed Implementation Steps:**

1.  **Install NuGet Package:**
    *   Add the `Microsoft.Extensions.Hosting` package to your main WPF project (`SentinelPro.WPF`). This package includes `Microsoft.Extensions.DependencyInjection` and other useful hosting abstractions.
    *   You can do this via the NuGet Package Manager in Visual Studio or the .NET CLI:
        ```bash
        dotnet add package Microsoft.Extensions.Hosting
        ```

2.  **Modify `App.xaml.cs`:**
    *   Update `App.xaml.cs` to configure and build the host, register services, and resolve the main window.

    ```csharp
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using SentinelPro.WPF.Services; // Assuming services are here
    using SentinelPro.WPF.ViewModels; // Assuming ViewModels are here
    using SentinelPro.WPF.Views; // Assuming Views (Windows) are here
    using System.Windows;
    using System; // For IDisposable

    namespace SentinelPro.WPF
    {
        public partial class App : Application, IDisposable
        {
            private IHost _host;

            public App()
            {
                // Optional: Initialize things before host creation if needed
            }

            protected override async void OnStartup(StartupEventArgs e)
            {
                base.OnStartup(e);

                var builder = Host.CreateDefaultBuilder(e.Args); // Includes config, logging defaults

                // Configure Services (Dependency Injection)
                builder.ConfigureServices((context, services) =>
                {
                    ConfigureDependencies(services, context.Configuration); // Pass configuration if needed
                });

                try
                {
                    _host = builder.Build(); // Build the host
                    await _host.StartAsync(); // Start the host (optional for WPF, but good practice)

                    // Resolve and show the main window
                    var mainWindow = _host.Services.GetRequiredService<MainWindow>();
                    mainWindow.Show();
                }
                catch (Exception ex)
                {
                    // TODO: Implement proper logging/error handling
                    MessageBox.Show($"Application failed to start: {ex.Message}", "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Shutdown(); // Shut down if startup fails
                }
            }

            private void ConfigureDependencies(IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
            {
                // Register Configuration Options (Example)
                // services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

                // Register Services (Adjust lifetimes as needed: Singleton, Scoped, Transient)
                services.AddSingleton<IBackupService, BackupService>(); // Example: Core service
                services.AddSingleton<IConfigurationService, ConfigurationService>(); // Example
                services.AddSingleton<IGeminiApiService, GeminiApiService>(); // Example AI Service
                services.AddSingleton<INotificationService, EmailNotificationService>(); // Example
                // Add other services...

                // Register ViewModels
                services.AddTransient<MainViewModel>();
                services.AddTransient<BackupViewModel>();
                services.AddTransient<SettingsViewModel>();
                services.AddTransient<AiAssistantViewModel>();
                // Add other ViewModels...

                // Register Windows/Views (Usually Transient)
                services.AddTransient<MainWindow>(); // Register the main window itself
                // Register other windows/dialogs if they need DI
                // services.AddTransient<SettingsWindow>();
            }

            protected override async void OnExit(ExitEventArgs e)
            {
                if (_host != null)
                {
                    await _host.StopAsync(); // Stop the host
                    _host.Dispose();         // Dispose of the host resources
                    _host = null;
                }
                base.OnExit(e);
            }

            // Implement IDisposable if you add other resources to App that need disposal
            public void Dispose()
            {
                _host?.Dispose();
                GC.SuppressFinalize(this);
            }
        }
    }
    ```

3.  **Implement Constructor Injection:**
    *   Go through your ViewModels, Services, and potentially Window code-behind files.
    *   Modify their constructors to accept required dependencies as parameters instead of creating them internally (e.g., `new BackupService()`) or using a `ServiceLocator`.
    *   Store the injected dependencies in private readonly fields.

    *Example (ViewModel):*

    ```csharp
    // Before DI
    // public class MainViewModel : ViewModelBase
    // {
    //     private readonly IBackupService _backupService;
    //     public MainViewModel()
    //     {
    //         _backupService = new BackupService(); // Or ServiceLocator.GetService<IBackupService>();
    //         // ...
    //     }
    // }

    // After DI
    public class MainViewModel : ViewModelBase
    {
        private readonly IBackupService _backupService;
        private readonly ISettingsViewModel _settingsViewModel; // Example injecting another ViewModel

        // Constructor now takes dependencies
        public MainViewModel(IBackupService backupService, ISettingsViewModel settingsViewModel)
        {
            _backupService = backupService ?? throw new ArgumentNullException(nameof(backupService));
            _settingsViewModel = settingsViewModel ?? throw new ArgumentNullException(nameof(settingsViewModel));
            // ... rest of constructor logic ...
        }
        // ... ViewModel properties and commands ...
    }
    ```

    *Example (MainWindow Code-Behind - if it needs direct dependencies):*

    ```csharp
    // Before DI
    // public partial class MainWindow : Window
    // {
    //     public MainWindow()
    //     {
    //         InitializeComponent();
    //         this.DataContext = new MainViewModel(); // Manually creating ViewModel
    //     }
    // }

    // After DI
    public partial class MainWindow : Window
    {
        // Inject the ViewModel (or other services if needed)
        public MainWindow(MainViewModel viewModel) // Request the ViewModel via constructor
        {
            InitializeComponent();
            this.DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel)); // Assign injected ViewModel
        }
    }
    ```

4.  **Review Service Lifetimes:**
    *   `AddSingleton()`: One instance for the entire application lifetime. Good for shared services like configuration, logging, core background tasks (BackupService might be a singleton).
    *   `AddTransient()`: A new instance is created every time it's requested. Good for lightweight services, ViewModels, and Windows.
    *   `AddScoped()`: One instance per scope (less common in basic WPF, more relevant in web apps or if you define custom scopes).
    *   Choose the appropriate lifetime for each registered service based on its purpose and state management requirements.

5.  **Remove Old Service Locator (If Applicable):**
    *   If your project currently uses a static `ServiceLocator` class, systematically remove all references to it and rely solely on constructor injection provided by the DI container.

**Outcome:**

*   The application will now start by configuring the DI container.
*   Dependencies (Services, ViewModels) will be automatically created and injected where requested via constructors.
*   Components will be more decoupled and easier to unit test (you can mock dependencies).
*   The application structure aligns with modern .NET practices.

**Next steps after this implementation:**

*   Thoroughly test the application to ensure all dependencies are resolved correctly and functionality remains intact.
*   Proceed with other refactoring tasks outlined in `TODO.md`, such as Configuration Validation, which can now leverage the `IConfiguration` provided by the host builder.
