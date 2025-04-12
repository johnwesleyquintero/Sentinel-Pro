# Sentinel Pro Branding Package

## Overview
This document outlines the comprehensive branding guidelines and design assets for Sentinel Pro. The branding system is designed to convey security, professionalism, and cutting-edge technology through consistent visual elements and experiences.

## 1. Brand Identity

### Core Values
- Security & Protection
- Innovation & Intelligence
- Professionalism & Reliability
- User-Centric Design

### Brand Voice
- Clear and authoritative
- Professional yet approachable
- Technology-forward but accessible
- Focused on user empowerment

### Logo Design

#### Primary Logo Specifications
The primary logo embodies the core values of Sentinel Pro through:
- Construction: Shield form with overlapping curved segments
- Style: Gradient blue tones
- Symbolism: Protection (shield) + AI technology (flowing curves)
- Clear Space: Maintain padding of 1x logo height on all sides
- Minimum Size: 32px height for digital, 0.25" for print

#### Logo Variants
| Variant | Description | Usage |
|---------|-------------|--------|
| Full Logo | Shield icon + "Sentinel Pro" text | Primary brand identification |
| Icon Only | Shield icon | Favicons, small applications |
| Wordmark | "Sentinel Pro" text | Horizontal space constraints |
| Monochrome | Single color version | Black/white backgrounds |

#### Logo Usage Guidelines
- Never stretch or distort the logo
- Maintain aspect ratio when scaling
- Use provided color variations only
- Ensure sufficient contrast with backgrounds
- Avoid placing logo on busy patterns

### Core Color Palette
| Color | Hex Code | RGB | Usage |
|-------|----------|-----|--------|
| Primary Blue | #00E1FF | 0,225,255 | UI curves, primary branding |
| Secondary Blue | #0097B2 | 0,151,178 | Depth elements |
| Dark Background | #1E1E1E | 30,30,30 | UI background |
| Light Text | #F5F5F5 | 245,245,245 | Dark mode text |
| Dark Text | #2D2D2D | 45,45,45 | Light mode text |

## 2. Icon System

### Design Principles
- Consistent line weight (2px)
- Rounded corners (2px radius)
- Shield motif integration
- Clear silhouettes
- Balanced negative space

### Application Icons
All application icons maintain the shield motif while adapting to different contexts:

```plaintext
📁 c:\Users\johnw\WorkspaceCleanup\Assets\Icons\App
- app_icon.ico (16, 32, 48, 256px versions)
- app_icon_dark.png
- app_icon_light.png
- splash_logo.png
- favicon.ico
```

### Feature Icons
Feature icons combine the shield motif with contextual elements:

```plaintext
📁 c:\Users\johnw\WorkspaceCleanup\Assets\Icons\Features
- backup.svg (shield with downward arrow)
- restore.svg (shield with upward arrow)
- ai_assistant.svg (robot face with shield elements)
- settings.svg (gear with shield integration)
- security.svg (lock with shield elements)
- history.svg (clock with shield elements)
- search.svg (magnifying glass with shield elements)
- profile.svg (user with shield elements)
```

### Navigation & Status Icons
Consistent style across all system indicators:

```plaintext
📁 c:\Users\johnw\WorkspaceCleanup\Assets\Icons\Navigation
- home.svg
- backups.svg
- history.svg
- search.svg
- settings.svg
- ai.svg
- help.svg
- user.svg

📁 c:\Users\johnw\WorkspaceCleanup\Assets\Icons\Status
- success.svg (checkmark in shield)
- warning.svg (exclamation in shield)
- error.svg (x in shield)
- loading.svg (animated shield with spinning segment)
- info.svg (i in shield)
```

## 3. Color System

### Primary Colors
```plaintext
- Shield Blue: #00E1FF (primary accent)
  - Hover: #33E7FF
  - Active: #00B4CC
- Deep Blue: #0097B2 (secondary accent)
  - Hover: #00A8C7
  - Active: #007A8F
- Dark Background: #1E1E1E (main background)
- Light Background: #F5F5F5 (light mode background)
```

### Secondary Colors
```plaintext
- Success Green: #00CF7F
  - Hover: #00E68F
  - Active: #00B36E
- Warning Yellow: #FFB800
  - Hover: #FFC633
  - Active: #CC9300
- Error Red: #FF4747
  - Hover: #FF6B6B
  - Active: #CC3939
- Info Blue: #3E8EFF
  - Hover: #66A5FF
  - Active: #3271CC
```

### Neutral Colors
```plaintext
- Gray-100: #F5F5F5 (backgrounds)
- Gray-200: #E5E5E5 (borders)
- Gray-300: #D4D4D4 (disabled)
- Gray-400: #A3A3A3 (placeholder)
- Gray-500: #737373 (secondary text)
- Gray-600: #525252 (primary text light)
- Gray-700: #404040 (surfaces dark)
- Gray-800: #262626 (surfaces darker)
- Gray-900: #171717 (background dark)
```

## 4. Typography

### Primary Font
```plaintext
- Font Family: Segoe UI (matching Windows UI)
- Weights:
  - Light (300): Headlines, display text
  - Regular (400): Body text, labels
  - Semibold (600): Subheadings, emphasis
  - Bold (700): Primary headings, CTAs
```

### Alternative Font (Web/Cross-platform)
```plaintext
- Font Family: Inter
- Weights:
  - Light (300)
  - Regular (400)
  - Semibold (600)
  - Bold (700)
```

### Type Scale
- Display: 48px/3rem
- H1: 40px/2.5rem
- H2: 32px/2rem
- H3: 24px/1.5rem
- H4: 20px/1.25rem
- Body: 16px/1rem
- Small: 14px/0.875rem
- Caption: 12px/0.75rem

## 5. UI Component Library

### Buttons
```plaintext
- Primary Button:
  - Background: Shield Blue
  - Text: White
  - Hover: 10% lighter
  - Active: 10% darker

- Secondary Button:
  - Background: Transparent
  - Border: Shield Blue
  - Text: Shield Blue
  - Hover: 5% Shield Blue background

- Tertiary Button:
  - Background: Transparent
  - Text: Shield Blue
  - Hover: 5% Shield Blue background

- Danger Button:
  - Background: Error Red
  - Text: White
  - Hover: 10% lighter
```

### Input Fields
```plaintext
- Text Input:
  - Background: Dark gray (Gray-700)
  - Border: Gray-600
  - Focus: Shield Blue

- Dropdown:
  - Background: Dark gray
  - Accent: Shield Blue
  - Options: Gray-600

- Checkbox & Radio:
  - Border: Gray-500
  - Selected: Shield Blue
  - Focus: Shield Blue glow

- Toggle:
  - Off: Gray-500
  - On: Shield Blue
  - Handle: White
```

### Cards & Panels
```plaintext
- Card:
  - Background: Gray-800
  - Shadow: 0 4px 6px rgba(0,0,0,0.1)
  - Border: 1px solid Gray-700
  - Radius: 8px

- Panel:
  - Background: Gray-800
  - Border-left: 4px Shield Blue
  - Padding: 16px

- Dialog:
  - Background: Gray-800
  - Border: 1px solid Gray-700
  - Shadow: 0 8px 16px rgba(0,0,0,0.2)
```

## 6. Animation & Interaction

### Timing
- Ultra-fast: 100ms (subtle feedback)
- Fast: 200ms (hover states)
- Standard: 300ms (transitions)
- Complex: 400ms (page transitions)

### Animation Guidelines
```plaintext
- Transitions: 300ms ease-in-out
- Hover Effects:
  - Scale: 1.02
  - Brightness: 1.1
  - Shadow: +4px blur

- Loading States:
  - Spinner: Shield segments rotating
  - Progress: Shield filling
  - Success: Shield forming with checkmark
```

### Interaction States
- Rest: Default state
- Hover: Subtle highlight
- Active: Clear feedback
- Focus: Accessible outline
- Disabled: Reduced opacity

## 7. Accessibility Guidelines

### Color Contrast
- Minimum contrast ratio: 4.5:1
- Large text contrast ratio: 3:1
- Interactive elements: 3:1

### Focus Indicators
- Visible focus states
- Keyboard navigation support
- Skip-link implementation

### Screen Readers
- Semantic HTML structure
- ARIA labels where needed
- Alternative text for images

## 8. Responsive Design

### Breakpoints
- Mobile: 320px - 767px
- Tablet: 768px - 1023px
- Desktop: 1024px+

### Layout Adjustments
- Fluid typography
- Flexible grids
- Adaptive components
- Touch-friendly targets

## 9. Implementation Files

### Logo Files
```plaintext
📁 c:\Users\johnw\WorkspaceCleanup\Assets\Branding\Logo
- sentinel_pro_logo_full.svg
- sentinel_pro_logo_full.png
- sentinel_pro_logo_icon.svg
- sentinel_pro_logo_icon.png
- sentinel_pro_logo_wordmark.svg
- sentinel_pro_logo_wordmark.png
```

### Style Definitions
```plaintext
📁 c:\Users\johnw\WorkspaceCleanup\Assets\Branding\Styles
- colors.xaml (WPF color resources)
- typography.xaml (WPF text styles)
- components.xaml (WPF control templates)
- animations.xaml (WPF animations)
```

### Documentation
```plaintext
📁 c:\Users\johnw\WorkspaceCleanup\Assets\Branding\Docs
- brand_guidelines.pdf
- icon_usage.md
- color_system.md
```

## 10. Application UI Examples

### Layout Structure
1. Dark Theme with Shield Blue accents
2. Left Navigation with icon + text menu items
3. Main Content Area with cards for backups
4. Status Bar at bottom for notifications
5. AI Chat Panel that slides in from right

### Component Examples
- Navigation menus
- Data tables
- Form layouts
- Modal dialogs
- Notification systems

### Responsive Patterns
- Mobile-first approach
- Collapsible navigation
- Adaptive layouts
- Touch-optimized interfaces