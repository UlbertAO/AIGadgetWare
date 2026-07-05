# ImproperInputValidationFix

This project is a small C# console application that demonstrates how to inspect JSON data for special characters and flag values that may require validation. It parses a sample JSON object and checks each value while allowing certain keys to be skipped.

## What this project is about

It shows a simple example of input validation in C#. The app uses JSON parsing and regular expressions to identify values that contain characters outside a basic allowed set.

## How it can benefit you

This project is useful for learning how to validate user input or structured data before processing it. It can also serve as a starting point for improving security and preventing unexpected characters from entering your application.

## How to run

Make sure you have .NET 6 installed, then run the following commands from the project folder:

```bash
dotnet build
dotnet run
```

The program will print whether the sample JSON contains any values with special characters.
