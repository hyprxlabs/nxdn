namespace Hyprx.Exec.Tests;

public static class PowershellCommandTests
{
    [Fact]
    public static void RunScript()
    {
        var pwsh = PathFinder.Which("powershell");
        Assert.SkipWhen(pwsh.IsNullOrWhiteSpace(), "PowerShell is not available on this system. Skipping test.");

        // Example of how to run a command and handle the output
        var output = new PowershellCommand()
           .RunScript("echo 'Hello, World!'");

        Assert.Equal(0, output.ExitCode);
    }

    [Fact]
    public static async ValueTask RunScriptAsync()
    {
        var pwsh = PathFinder.Which("powershell");
        Assert.SkipWhen(pwsh.IsNullOrWhiteSpace(), "PowerShell is not available on this system. Skipping test.");

        // Example of how to run a command asynchronously and handle the output
        var output = await new PowershellCommand()
           .RunScriptAsync("echo 'Hello, World!'", TestContext.Current.CancellationToken);

        Assert.Equal(0, output.ExitCode);
    }

    [Fact]
    public static void OutputScript()
    {
        var pwsh = PathFinder.Which("powershell");
        Assert.SkipWhen(pwsh.IsNullOrWhiteSpace(), "PowerShell is not available on this system. Skipping test.");

        // Example of how to run a command and get the output
        var output = new PowershellCommand()
           .OutputScript("echo 'Hello, World!'");

        Assert.Equal(0, output.ExitCode);
        Assert.NotEmpty(output.Stdout);

        var text = output.Text().Trim();
        Assert.NotEmpty(text);
        Assert.Equal("Hello, World!", text);
    }

    [Fact]
    public static async ValueTask OutputScriptAsync()
    {
        var pwsh = PathFinder.Which("powershell");
        Assert.SkipWhen(pwsh.IsNullOrWhiteSpace(), "PowerShell is not available on this system. Skipping test.");

        // Example of how to run a command asynchronously and get the output
        var output = await new PowershellCommand()
           .OutputScriptAsync("echo 'Hello, World!'", TestContext.Current.CancellationToken);

        Assert.Equal(0, output.ExitCode);
        Assert.NotEmpty(output.Stdout);
        Console.WriteLine(output.Stdout[output.Stdout.Length - 1]);
        Console.WriteLine("stdout length: {0}", output.Stdout.Length);

        var text = output.Text().Trim();
        Assert.NotEmpty(text);
        Assert.Equal("Hello, World!", text);
    }
}