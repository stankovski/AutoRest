set autoRestExe=..\binaries\net45\AutoRest.exe -Modeler Swagger -Namespace HelloWorld -Input .\helloworld\helloworld.json
%autoRestExe% -CodeGenerator Go -outputDirectory .\helloworld\go
%autoRestExe% -CodeGenerator Ruby -outputDirectory .\helloworld\ruby
%autoRestExe% -CodeGenerator CSharp -outputDirectory .\helloworld\csharp\generated
%autoRestExe% -CodeGenerator NodeJS -outputDirectory .\helloworld\nodejs\generated
%autoRestExe% -CodeGenerator Java -outputDirectory .\helloworld\java