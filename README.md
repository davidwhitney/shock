# Shock - A .NET Pre-Flight Bootstrapper / Task runner

Shock exists to provide an answer to the very simple question

    How can I run this piece of code before my application starts

If that sounds pretty abstract, it's because it is.

Shock scans your code for tasks that it can execute in your codebase, and allows you to invoke them on the command line. You have access to all your language features, libraries and methods, and can build whatever you want. Sounds simple right?

## Why does it exist

While Shock might look like "yet another task runner" in the style of Grunt and Gulp - it's more appropriate to think of it as a small utility that you can use in building deployment pipelines.

It helps you keep your "random tasks" and "maintainance jobs" and "deployment automation" code right where it needs to be - with your application.

## How do I use it?

Ideally, shock will *just work* by default. It ships as a single executable inside of a nuget package, and has no dependencies. You'll need to add it to your build outputs to actually use it.

You use shock by calling `shock.exe` in the directory of the application that contains your tasks.

Given an application with the following class in it

```csharp
namespace ShockSampleApp
{
    public class DefaultTask
    {
        public void Run()
        {
            Console.WriteLine("Default task.");
        }
    }
}
```
You can invoke shock.exe in the directory containing your binaries

```
    c:\dev\shock\samples\ShockSampleApp\bin>Shock.exe
    Default task.
    Executed: ShockSampleApp.DefaultTask.Run

    c:\dev\shock\samples\ShockSampleApp\bin>
```
And you task will execute. This is the simplest possible example - an application with a single task, matching our default task convention.

Let's look at some more usage examples


# Usage Examples

**Running shock in a directory with no .NET binaries**

```
c:\dev\shock\samples\ShockSampleApp>Shock.exe
usages:
         shock (assemblies autoloaded, default tasks executed)
         shock task [options] (assemblies autoloaded, task executed)
         shock assembly.dll task [options]

flags:
         verbose - Extra diagnostics
         help | ? - Usage examples and task list
         continue - Continue processing on single task errors
         interactive - Don't exit without user input
```
Outcomes: Scans directory and finds nothing, Outputs usages

---

**Running Shock in a directory with .NET binaries with multiple tasks**

Given the following class
```csharp
    public class BuildTasks
    {
        public void ModifyConfig() { Console.WriteLine("Modify configuration"); }
        public void DoOtherThing() { Console.WriteLine("Do other thing"); }
    }
```
When called
```
c:\dev\shock\samples\ShockSampleApp\bin>Shock.exe
usages:
         shock (assemblies autoloaded, default tasks executed)
         shock task [options] (assemblies autoloaded, task executed)
         shock assembly.dll task [options]

flags:
         verbose - Extra diagnostics
         help | ? - Usage examples and task list
         continue - Continue processing on single task errors
         interactive - Don't exit without user input
tasks:

        ShockSampleApp.BuildTasks.ModifyConfig

        ShockSampleApp.BuildTasks.DoOtherThing
```
Outcomes: Scans directory, finds many tasks, outputs task list

**To invoke a task in this scenario**

```
c:\dev\shock\samples\ShockSampleApp\bin>Shock.exe -ModifyConfig
Modify configuration
Executed: ShockSampleApp.BuildTasks.ModifyConfig
```

**Or in the case of namespace conflicts**

```
c:\dev\shock\samples\ShockSampleApp\bin>Shock.exe -ShockSampleApp.BuildTasks.ModifyConfig
Modify configuration
Executed: ShockSampleApp.BuildTasks.ModifyConfig
```
---

**Passing parameters to tasks**

Given the following class
```csharp
    public class BuildTasks
    {
        public void Task(string param) { Console.WriteLine(param); }
    }
```
When called
```
c:\dev\shock\samples\ShockSampleApp\bin>Shock.exe -Task -param=abc
abc
Executed: ShockSampleApp.BuildTasks.Task
```
Outcomes: Scans directory, maps method parameters from the command line.

---

**Running Shock in verbose mode**

Given the directory layout
```
04/04/2016  10:45 AM    <DIR>          .
04/04/2016  10:45 AM    <DIR>          ..
04/04/2016  10:13 AM            29,344 Microsoft.CodeDom.Providers.DotNetCompilerPlatform.dll
04/04/2016  10:13 AM             1,805 Microsoft.CodeDom.Providers.DotNetCompilerPlatform.xml
04/04/2016  10:45 AM    <DIR>          roslyn
04/04/2016  10:50 AM             4,096 ShockSampleApp.dll
04/04/2016  10:08 AM             1,174 ShockSampleApp.dll.config
04/04/2016  10:50 AM            11,776 ShockSampleApp.pdb
               5 File(s)         48,195 bytes
               3 Dir(s)  368,239,435,776 bytes free
```

When called
```
c:\dev\shock\samples\ShockSampleApp\bin>Shock.exe -verbose
Loaded c:\dev\shock\samples\ShockSampleApp\bin\Microsoft.CodeDom.Providers.DotNetCompilerPlatform.dll
Loaded c:\dev\shock\samples\ShockSampleApp\bin\ShockSampleApp.dll
```
Outcomes: Displays the list of imported assemblies.

**Specify .dll to load explicitly**

```
c:\dev\shock\samples\ShockSampleApp\bin>Shock.exe -verbose ShockSampleApp.dll
Loaded ShockSampleApp.dll
```
Outcomes: Only loads tasks for specified dll's.

# Task Discovery Conventions

Shock discovers tasks based on some simple rules by default. Methods are considered tasks if:

- They're declared in a `class` whose name contains `Task`
- They have an attribute applied to them with the name `TaskAttribute`

Shock excludes from its discovery, any `class` found in:

- Any `namespace` starting `System.*`
- Any `namespace` starting `Accessibility.*`
- Any `namespace` starting `Microsoft.*`
- It's own assembly

From the discovered tasks, the tasks that are executed and selected

# Task Selection Conventions

Shock executes a task if:

- The declaring `class` is called `DefaultTask` and the name of the method `Run`
- The `method name`, `full name` or the `classname and method name` is found passed as a command line argument

# Passing Parameters

You can pass parameters by providing their values as key/value pairs on the command line using the syntax

```
shock.exe -paramName=paramValue -otherParam=1
```

Parameters can be any common primitive type.

# Exit Codes

Shock returns the following exit codes - use them in your CI configuration / failure conditions.

- Success = 0
- Failed = 1
- NoTasksRun = 2

If multiple tasks are run and you pass the `-continue` argument a failed task will not stop execution, and you'll get a Success 0 exit code. If tasks fail, diagnostic output will be written to the console.

You can force `shock` to wait for user input - `Press ANY key to exit` - so that you can read diagnostic output messages by passing the command line argument `-interactive`

# Modifying the conventions

Before shock executes anything, it allows you to modify it's discovery and selection conventions in your client code.

Much like executing of tasks, this is done by convention.

If you're happy taking a direct dependency on `shock.exe` in your code, you can implement the following class:

```csharp
    public class ThisClassModifiesShockConventionsThatCanHaveAnyName
    {

        public ThisClassModifiesShockConventions(ActiveConventions conventions)
        {
            ...
        }
    }
```
From here, you can override any of the `ActiveConventions` properties.

If you want to avoid taking a direct dependency on shock, you can implement the following classes, that will be discovered and executed:

```csharp
using SelectionFunc = System.Func<System.Collections.Generic.List<System.Reflection.MethodInfo>, System.Collections.Generic.Dictionary<string, object>, System.Collections.Generic.IEnumerable<System.Reflection.MethodInfo>>;


    public class ModifyDiscoveryConventions
    {
        public void ConfigureTaskDiscovery(List<Func<MethodInfo, bool>> matchRules, List<Func<Type, bool>> exclusionRules)
        {
            ...
        }
    }

    public class ModifySelectionRules
    {
        public void ConfigureTaskSelection(List<SelectionFunc> matchRules)
        {
            ...
        }
    }
```

By modifying the list of selection criteria passed into these methods at runtime, you can completely control the way tasks are identified in your application.

If multiple classes in this style are discovered, order of execution is not guaranteed.


# Contributing

I'd love to talk about anyone that finds this useful, or has any changes to make.

Guidelines:
- Keep change deltas bounded and small - no huge refactors
- Follow the default R# style guidelines

Process:
- Open a GitHub issue for discussion
- Add a failing test
- Implement 

# Authors

- David Whitney