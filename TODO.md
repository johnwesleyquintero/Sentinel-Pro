# SENTINEL PRO ROADMAP - PROJECT MANAGEMENT - MILESTONE TRACKING

## 1.0.0

**FYI:**

1. We have a landing page at wwwroot\index.html this serves as our landing page where users can download our releases and therefore, we should keep this directory `wwwroot` and dont't delete `netlify.toml` as they host our landing page.

2. Don't delete this `TODO.md` too because how would we track our development progress if we dont have this?

---


- [ ] Add basic functionality
- [ ] Add documentation
- [ ] Add tests
- [ ] Add logging
- [ ] Add configuration
- [ ] Add error handling
- [ ] High priority: 🛠️ **Fix Issues and Implement Enhancements**

I need your assistance in addressing errors and implementing improvements based on your suggestions. Below is the task list:

**TASK LIST:**

1. **Codebase Cleanup:**
   - [x] Remove old references from `WorkspaceCleanup` to `SentinelPro` across the entire codebase.
   - Delete outdated files and perform a thorough cleanup of the codebase.
   - Alternatively, reset all configurations to `SentinelPro` for better clarity and organization.

2. **GitHub Pipeline Setup:**
   - Configure a GitHub pipeline to automate the build process.
   - Ensure the pipeline releases the build to `/wwwroot/publish/SentinelPro-Setup.exe`.

---

- [ ] Integrate Gemini API
- [ ] Clean up unused Ollama-related code
- [ ] Update documentation for new features
- [ ] Add tests for Gemini implementation
- [ ] Review project structure for unused files
- [ ] Create visual project structure diagram
- [ ] Update wwwroot README with better documentation
- [ ] Improve navigation links in wwwroot landing page
- [ ] Investigate missing setup file issue (Cannot GET /wwwroot/publish/SentinelPro-Setup.exe)
- [ ] Implement error handling for Gemini API
- [ ] Document new services architecture
- [ ] Add configuration validation
- [ ] Review logging implementation
- [ ] Optimize service dependencies
- [ ] Implement proper dependency injection
- [ ] Add unit tests for ViewModels
- [ ] Improve error handling in services
- [ ] Create documentation for service architecture
- [ ] Add unit tests for MonitoringService
- [ ] Add unit tests for NotificationService
- [ ] Review DI configuration in ServiceLocator
- [ ] Enhance logging implementation in LoggingService
- [ ] Add validation for appsettings.json configuration
- [ ] Implement performance monitoring for services
- [ ] Implement Setup Wizard: Run for advanced configuration
- [ ] Publish SentinelPro-Setup.exe - Windows installer and Setup Wizard to wwwroot\publish