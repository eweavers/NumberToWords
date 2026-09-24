Number to Words
================

Takes a number and spits out the word version, formatted like a check amount -
"123.45" becomes "ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS". 

A C# api does the actual conversion, plain HTML/CSS/JS page on top so you can poke at it in a browser instead of just curling it.

NumberToWords.sln - solution file, ties the two C# projects together
api - the actual solution (.NET 10 minimal api)
docs - the frontend, plain html/css/js, this is what GitHub Pages serves
tests - xunit tests for the conversion logic

LIVE VERSION ON : https://eweavers.github.io/NumberToWords/
[!URGENT!] - Since I am hosting on a free version of Render the api will sleep after inactivity so first request may take up to 60 seconds, after that will be rapid [!URGENT!]

Need the .NET 10 SDK.

local :

build everything:        dotnet build NumberToWords.sln
run the tests:            dotnet test
run the api locally:      cd api, then dotnet run

That last one spins it up on localhost:5000 (or whatever port it prints to the console). The frontend's api address is hardcoded in docs/script.js (look for API_BASE near the top) so for local testing just point that constant at localhost:5000 temporarily.

You can also hit the api directly:
  localhost:5000/api/convert?number=1234.56
comes back as json, something like:
  { "input": "1234.56", "words": "ONE THOUSAND, TWO HUNDRED AND THIRTY-FOUR DOLLARS AND FIFTY-SIX CENTS" }

Hosting it:

GitHub Pages only serves static files, it can't run a C# process. So:

Frontend -> GitHub Pages.

Api -> needs an actual host that runs .NET. Render. Render specifically has no native .NET support, it needs Docker, there's a Dockerfile sitting in api/.

Once the api's actually deployed somewhere, go update API_BASE in docs/script.js

Using it:

Open the page, type a number (negatives and decimals both work), it converts after a short pause.
