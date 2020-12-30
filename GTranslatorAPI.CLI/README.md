# GTranslatorAPI.CLI
Google Translate Rest API (Free) - Command line tool - .NET Core 5 , .NetStandard2.1
VSCode / Visual Studio
<hr>

[![licence mit](https://img.shields.io/badge/licence-MIT-blue.svg)](license.md) This project is licensed under the terms of the MIT license: [LICENSE.md](LICENSE.md)  

![last commit](https://img.shields.io/github/last-commit/franck-gaspoz/GTranslatorAPI?style=plastic)
![version](https://img.shields.io/github/v/tag/franck-gaspoz/GTranslatorAPI?style=plastic)

**a C# library that deals with Google Translate REST API. Offers google translate service operations from a C# software (with no api token)**. 

This project is a sample command line tool built within the library.  

The main documentation of the project is here: [../README.md](../README.md)

# Build the tool

from powershell command line:
```dos  
mkdir gtranslator
cd gtranslator
dotnet new console
dotnet add package GTranslatorAPI.CLI -v 1.0.0
dotnet build
cd .\bin\Debug\net5.0\
copy gtranslator.runtimeconfig.json GTranslatorAPI.CLI.runtimeconfig.json
```
then you can run the tool:
```dos  
dotnet GTranslatorAPI.CLI.dll
dotnet GTranslatorAPI.CLI.dll -list
dotnet GTranslatorAPI.CLI.dll en es "hello world!"
```

<hr>

> ### :information_source: About this repository
> To go further, please read informations at : [http://franckgaspoz.fr/en/use-google-translate-for-free-on-the-command-line/](http://franckgaspoz.fr/en/use-google-translate-for-free-on-the-command-line/) or contact the autor
