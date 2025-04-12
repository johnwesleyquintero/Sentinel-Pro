Okay, let's look at the XAML files you've provided (App.xaml, MainWindow.xaml, SettingsWindow.xaml, RuleEditDialog.xaml).

Yes, based on the XAML structure you've provided, this looks like a perfectly valid and standard way to start building a WPF application, especially one using the ModernWpf library.

Here's a breakdown of why it "will work" from a structural perspective:

App.xaml:

StartupUri="MainWindow.xaml": This correctly tells the application to load and show MainWindow.xaml when it starts.
ResourceDictionary.MergedDictionaries: You're correctly including the necessary resources from ModernWpf (ThemeResources, XamlControlsResources). This is required for the ModernWpf styling and controls to function properly.
MainWindow.xaml:

Layout: The Grid with two columns (navigation on the left, content on the right) is a very common and effective pattern for desktop applications.
ModernWpf Integration: ui:WindowHelper.UseModernWindowStyle="True" correctly applies the modern window chrome.
Navigation: Using RadioButtons (styled) for navigation and a Frame (ContentFrame) to host the content pages is a standard WPF navigation technique. You'll need code-behind (MainWindow.xaml.cs) to handle the Checked events of the RadioButtons and navigate the ContentFrame to the appropriate Page XAML files (e.g., HomePage.xaml, BackupPage.xaml, SettingsPage.xaml - which might contain your SettingsWindow content or you might show SettingsWindow as a separate dialog).
Styling: The custom NavigationButtonStyle looks like a reasonable way to achieve the desired navigation appearance.
SettingsWindow.xaml & RuleEditDialog.xaml:

Standard Controls: These windows use standard WPF controls (TextBox, Button, Label, ListView, GridView, CheckBox, ComboBox) within typical layout containers (Grid, StackPanel, GroupBox). This is all standard practice.
ModernWpf: They also correctly reference the ModernWpf namespace (ui) and apply the modern window style.
Functionality: The layout clearly defines the UI for settings and rule editing. The Click events (Browse..., Save..., AddRule..., etc.) point to the need for corresponding methods in the code-behind (.xaml.cs) files to handle user interactions.
Data Display: The ListView with GridView in SettingsWindow.xaml is the correct way to display tabular data like your rules. You'll need code-behind to populate its ItemsSource property with a collection of rule objects and handle the Add/Edit/Remove button logic.
In summary:

The XAML structure is sound and follows common WPF practices.
The integration with ModernWpf appears correct.
The chosen navigation pattern (RadioButton + Frame) is standard.
The dialog/window layouts are logical.
What Makes It Actually Work (The Next Steps):

While the XAML defines the look and structure, the functionality depends entirely on the C# code-behind (.xaml.cs files):

Event Handlers: You need to implement the methods referenced by Click events (e.g., SaveSettings_Click, AddRule_Click, BrowsePath_Click).
Navigation Logic: In MainWindow.xaml.cs, you need code to detect which RadioButton is checked and tell the ContentFrame to navigate to the corresponding Page.
Data Binding/Management: In SettingsWindow.xaml.cs, you need code to:
Load existing settings into the controls when the window opens.
Load rules into the RulesListView.
Handle adding, editing (which involves opening RuleEditDialog), and removing rules from the list.
Save the settings and rules when the "Save" button is clicked.
Dialog Logic: Code is needed to open RuleEditDialog (likely from SettingsWindow), potentially pass data to it (if editing an existing rule), and get data back from it when the user clicks "Save".
Conclusion:

Don't worry! For a first native Windows app, this is a great start. The XAML structure is solid. Now you need to focus on writing the C# code-behind to bring the UI to life. It will work once you implement the necessary logic. Keep going!