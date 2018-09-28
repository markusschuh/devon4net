mkdir results

cd ./OASP4Net.Domain.Context/src
dotnet restore
dotnet build >..\..\results\OASP4Net.Domain.Context.result.txt


cd ..\..
cd ./OASP4Net.Domain.UnitOfWork/src
dotnet restore
dotnet build >..\..\results\OASP4Net.Domain.UnitOfWork.result.txt

cd ..\..
cd ./OASP4Net.Infrastructure.AOP/src
dotnet restore
dotnet build >..\..\results\OASP4Net.Infrastructure.AOP.result.txt

cd ..\..
cd ./OASP4Net.Infrastructure.ApplicationUser/src
dotnet restore
dotnet build >..\..\results\OASP4Net.Infrastructure.ApplicationUser.result.txt

cd ..\..
cd ./OASP4Net.Infrastructure.Communication/src
dotnet restore
dotnet build >..\..\results\OASP4Net.Infrastructure.Communication.result.txt

cd ..\..
cd ./OASP4Net.Infrastructure.Cors/src
dotnet restore
dotnet build >..\..\results\OASP4Net.Infrastructure.Cors.result.txt

cd ..\..
cd ./OASP4Net.Infrastructure.Extensions/src
dotnet restore
dotnet build >..\..\results\OASP4Net.Infrastructure.Extensions.result.txt

cd ..\..
cd ./OASP4Net.Infrastructure.JWT/src
dotnet restore
dotnet build >..\..\results\OASP4Net.Infrastructure.JWT.result.txt

cd ..\..
cd ./OASP4Net.Infrastructure.JWT.MVC/src
dotnet restore
dotnet build >..\..\results\OASP4Net.Infrastructure.JWT.MVC.result.txt

cd ..\..
cd ./OASP4Net.Infrastructure.Log/src
dotnet restore
dotnet build >..\..\results\OASP4Net.Infrastructure.Log.result.txt

cd ..\..
cd ./OASP4Net.Infrastructure.Middleware/src
dotnet restore
dotnet build >..\..\results\OASP4Net.Infrastructure.Middleware.result.txt

cd ..\..
cd ./OASP4Net.Infrastructure.MVC/src
dotnet restore
dotnet build >..\..\results\OASP4Net.Infrastructure.MVC.result.txt

cd ..\..
cd ./OASP4Net.Infrastructure.SMTP/src
dotnet restore
dotnet build >..\..\results\OASP4Net.Infrastructure.SMTP.result.txt

cd ..\..
cd ./OASP4Net.Infrastructure.Swagger/src
dotnet restore
dotnet build >..\..\results\OASP4Net.Infrastructure.Swagger.result.txt

cd ..\..
cd ./OASP4Net.Infrastructure.Test/src
dotnet restore
dotnet build >..\..\results\OASP4Net.Infrastructure.Test.result.txt

cd ..\..
