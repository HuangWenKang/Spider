FROM microsoft/dotnet:2.2-sdk as build-image

WORKDIR /home/app

COPY ./*.sln ./
COPY ./*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./${file%.*}/ && mv $file ./${file%.*}/; done

RUN dotnet restore

COPY . .

RUN dotnet test --verbosity=normal --results-directory /TestResults/ --logger "trx;LogFileName=test_results.xml" ./HtmlSpider.Test/HtmlSpider.Test.csproj

RUN dotnet publish ./Scheduler.API/Scheduler.API.csproj -o /publish/

FROM microsoft/dotnet:2.2-aspnetcore-runtime

WORKDIR /publish

COPY --from=build-image /publish .

COPY --from=build-image /TestResults /TestResults

ENTRYPOINT ["dotnet", "Scheduler.API.dll"]

