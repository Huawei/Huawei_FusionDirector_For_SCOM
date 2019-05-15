
set PATH=C:\Program Files (x86)\MSBuild\14.0\Bin\;%PATH%
set PROJECT_ROOT=%WORKSPACE%

cd ..
call D:\plugins\CodeDEX\tool\fortify\bin\sourceanalyzer -b Huawei-SCCMPlugin msbuild /t:rebuild /p:Configuration=release;Platform=x86;VisualStudioVersion=14.0 %PROJECT_ROOT%\src\Huawei-SCCMPlugin.sln

call D:\plugins\CodeDEX\tool\fortify\bin\sourceanalyzer -b Huawei-SCCMPlugin %PROJECT_ROOT%/**/*.xml %PROJECT_ROOT%/**/*.config %PROJECT_ROOT%/**/*.properties