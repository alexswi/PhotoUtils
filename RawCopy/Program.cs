using Spectre.Console;

namespace PhotoUtils.RawCopy;

internal class Program {
	public const string InputDirectory = @"E:\DCIM\100EOSR5";
	public static string OutputDirectory = @"D:\Photos\Porcessed";
	private static void Main(string[] args) {
		var nfiles = 0;
		var copiedFiles = 0;
		var notCopiedFiles = 0;

		var files = Directory.GetFiles(InputDirectory, "*.dop");
		AnsiConsole.MarkupLine($"[yellow]{files.Length:N0}[/][green] dop files found![/]");
		foreach (var dopFile in files) {
			var fileInfo = new FileInfo(dopFile);
			var rawFile = dopFile.Remove(dopFile.Length - 4);
			var rawFileInfo = new FileInfo(rawFile);
			var datedOutputDirectory = Path.Combine(OutputDirectory, rawFileInfo.CreationTime.Year.ToString(), rawFileInfo.CreationTime.Month.ToString("D2"));
			if (!Directory.Exists(datedOutputDirectory)) {
				Directory.CreateDirectory(datedOutputDirectory);
			}
			var dopOutputFile = Path.Combine(datedOutputDirectory, fileInfo.Name);
			var cr3OutputFile = Path.Combine(datedOutputDirectory, rawFileInfo.Name);
			if (File.Exists(dopOutputFile)) {
				var outputFileInfo = new FileInfo(dopOutputFile);
				if (outputFileInfo.LastWriteTime >= fileInfo.LastWriteTime) {
					notCopiedFiles++;
					continue;
				}
			}
			File.Copy(dopFile, dopOutputFile, true);
			File.Copy(rawFile, cr3OutputFile, true);
			copiedFiles++;
		}
		AnsiConsole.MarkupLine($"[yellow]{copiedFiles:N0}[/][green] dop and raw files copied![/]");
		AnsiConsole.MarkupLine($"[red]{notCopiedFiles:N0}[/][green] dop and raw files not copied![/]");
	}
}
