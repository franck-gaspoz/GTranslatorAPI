# GTranslatorAPI
Google Translate Rest API (Free) - .NET Core 5 , .NetStandard2.1
VSCode / Visual Studio
<hr>

[![licence mit](https://img.shields.io/badge/licence-MIT-blue.svg)](license.md) This project is licensed under the terms of the MIT license: [LICENSE.md](LICENSE.md)  

![last commit](https://img.shields.io/github/last-commit/franck-gaspoz/GTranslatorAPI?style=plastic)
![version](https://img.shields.io/github/v/tag/franck-gaspoz/GTranslatorAPI?style=plastic)

**a C# library that deals with Google Translate REST API. Offers google translate service operations from a C# software (with no api token)**

```CSharp
var gcl = new GTranslatorAPIClient();
// source language, target language, text to be translated
var r = gcl.TranslateAsync(Languages.en, Languages.fr, "text to be translated");
// translated text
var res = r.Result; 
```

## Get the library
### source from GitHub:
```dosbatch
cd myProjects
git clone https://github.com/franck-gaspoz/GTranslatorAPI.git
cd .\GTranslatorAPI\
code .
```
*then go Run > Start Debugging , select .Net Core engine*, you should get the output:
```
GTranslatorAPI command line interface 1.0.0.0
(c)MIT 2020 franck.gaspoz@gmail.com

command line syntax:
source_langid target_langid text [-q]
    source_langid : original text lang id
    target_langid : translated text lang id
    -q : turn off all outputs excepting errors
or
    -list : dump list of languages ids & names
```
### package from nuget:
```dosbatch
dotnet add package GTranslatorAPI --version 1.0.0 --source https://www.nuget.org/api/v2/package/
```

### package from GitHub:
```dosbatch
dotnet add package GTranslatorAPI --version 1.0.0 --source https://nuget.pkg.github.com/franck-gaspoz/index.json
```

## Use the library
let's build a new C# console project for testing the GTranslatorAPI nuget package, using powershell, dotnet and vscode :
```dosbatch
mkdir MyProject
cd MyProject
dotnet new console
dotnet add package GTranslatorAPI --version 1.0.0
code .
```
let's change the default main method in Program.cs, to change its default behavior (output 'Hello world!') by a new one, that **adds a translation of 'Hello world!' to spanish**, using the GTranslatorAPI :
```C#
using System;
using GTranslatorAPI;

namespace MyProject
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = "Hello world!";
            Console.WriteLine(s);
            var tr = new GTranslatorAPIClient();
            var r = tr.TranslateAsync(Languages.en,Languages.es,s);
            var res = r?.Result;            
            Console.WriteLine(res?.TranslatedText);    
            var dbg=$"-- is canceled={r?.IsCanceled},is completed={r?.IsCompleted},is completed successfully={r?.IsCompletedSuccessfully},is faulted={r?.IsFaulted}";
            Console.WriteLine(dbg);       
        }
    }
}

```
then run the project (ctrl+F5 | Run > Start without debugging). You should get:

![run sample in vscode](Doc/run.png)

<hr>

> ### :information_source: About this repository
> To go further, please read informations at : [http://franckgaspoz.fr/en/use-google-translate-for-free-on-the-command-line/](http://franckgaspoz.fr/en/use-google-translate-for-free-on-the-command-line/) or contact the autor
