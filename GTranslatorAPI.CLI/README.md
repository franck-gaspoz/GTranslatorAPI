# GTranslatorAPI.CLI
Google Translate Rest API (Free) - Command line tool - .NET Core 5 , .NetStandard2.1
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
```Dos
cd myProjects
git clone https://github.com/franck-gaspoz/GTranslatorAPI.git
cd .\GTranslatorAPI\
code .
```
*then go Run > Start Debugging , select .Net Core engine*, you should get the output:
```Dos
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
### app package from nuget:
```Dos
dotnet add package GTranslatorAPI.CLI --version 1.0.0 --source https://www.nuget.org/api/v2/package/
```

### app package from GitHub:
```Dos
dotnet add package GTranslatorAPI.CLI --version 1.0.0 --source https://nuget.pkg.github.com/franck-gaspoz/index.json
```


<hr>

> ### :information_source: About this repository
> To go further, please read informations at : [http://franckgaspoz.fr/en/use-google-translate-for-free-on-the-command-line/](http://franckgaspoz.fr/en/use-google-translate-for-free-on-the-command-line/) or contact the autor
