
# Blazor Components

This repository contains a collection of reusable **Blazor components** designed to help you build professional and user-friendly interfaces. These components are easy to integrate into your Blazor projects and come with extensive documentation.

---

## 🚀 Features

- **DatePicker**: A customizable date selection component.
- **Button**: Configurable buttons with various styles and events.
- **TextBox**: Advanced text fields with validation and additional features.
- **ItemChooser**: A component for selecting items from a list with search and filter capabilities.
- **TreeView**: A hierarchical data display component with expand/collapse functionality.
- And more...

---

## 📚 Documentation

For complete documentation, usage examples, and API references, visit the official documentation website:  
👉 [Blazor Components Documentation](https://zeonmicro.ir/)

---

## 🛠️ Installation

To get started with the **ZEO Blazor Components**, follow these steps:

### Step 1: Clone the Repository

First, clone the repository to your local machine using the following command:

```bash
git clone https://github.com/your-username/your-repo-name.git
```

### Step 2: Navigate to the Project Directory

After cloning, navigate to the project directory:

```bash
cd your-repo-name
```

### Step 3: Add Components to Your Blazor Project

You can either:
- Use the components directly in your Blazor project, or
- Create a **Razor Class Library (RCL)** for better reusability.

### Step 4: Install Dependencies (if needed)

If the project has any dependencies, install them using the appropriate package manager. For example, if using `npm`:

```bash
npm install
```

Or if using `.NET` packages:

```bash
dotnet restore
```

### Step 5: Build and Run the Project

Finally, build and run your Blazor project:

```bash
dotnet build
dotnet run

---
```

## 🎯 Usage

Here’s a quick example of how to use the `Button` component:

### Example: Using the Button Component

```razor
<ZDateTimePicker @ref="MyDateTimePicker1" DefaultDateTime="_people.Birthday" ChangedDateTime="MyDateTimePickerEvent"></ZDateTimePicker>

<Zeon.Blazor.ZButton.ZButton Text="Change Calendar Type" Onclick="ChangePickerType" CssIcon="fa fa-refresh"> </Zeon.Blazor.ZButton.ZButton>

```

```csharp
@code{

public ZDateTimePicker MyDateTimePicker1 { get; set; } = null!;

private void ChangePickerType()
{
var newType = MyDateTimePicker1.DatePickerType == DatePickerType.Jalali ? DatePickerType.Gregorian : DatePickerType.Jalali;
MyDateTimePicker1.ChangeDatePickerType(newType);
}

private void MyDateTimePickerEvent(DateTime value)
{

}
```

For more examples and detailed usage instructions, check out the [documentation](https://zeonmicro.ir/).


---

## 📧 Contact

If you have any questions, suggestions, or need help, feel free to reach out:

- **Email**: [meysam.71kh@gmail.com](mailto:meysam.71kh@gmail.com)
- **Website**: [https://zeonmicro.ir/](https://zeonmicro.ir/)
