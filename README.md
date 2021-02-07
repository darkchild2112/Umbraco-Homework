# Umbraco-Homework

There's fundamentally two parts to the application, a .Net Core API and then a seperate User Interface (UI) built using VueJs. The API has been built using Visual Studio for Mac, so if you get any issues running on a Windows system, please get in contact. The UI has been built using the VueJs CLI and Visual Studio Code.

## The API

The API has been written using .Net Core 3.1. I did consider .Net 5 but as this is fairly new and I've only just started using it, I decided to play it safe. I've used Entity Framework Core for data access and also Swagger for easy API documenting and testing. Swagger is also the tool used to generate the Serial mumbers and view prize entries. For unit testing I've used XUnit and Moq, however I mostly used an InMemory database context and didn't use the Moqing framework in the end, but I was sure I would in future enhancements. 

### Project Setup

The Solution contains two projects; one for the API and the other for Unit Tests. I did originally start with more, with the Models and services in seperate projects, but there seems to be an issue with creating Entity Framework migrations on Visual Studio for Mac with this setup. This was frustrating as I've had this working on a Windows setup in the past :(

### Dependency Injection

I've used the built in Dependency Injection in .NET Core to inject services and DB Context into the controllers. The controllers themselves are lightweight with all the logic in the services for loose coupling and reusability.

### Nuget Packages

Nuget should automatically restore all packages when frist loading the solution. However, if this doesn't happen please right click on the solution in the solution explorer and select the 'Restore Nuget packages' option from the context menu.

### Datbase

The Database itself is self installing and will create the DB and all necessary tables when the API runs for the first time. The only thing you need to do is check the connection string in the appSettings.json file to make sure the default connection string points to an available SQL server on your system.

### Running the Solution

To run the solution simply hit 'run' in the toolbar at the top of Visual Studio. You can then use the Swagger interface (which is set to the default page) to generate the serial numbers using the SerialNumber/GenerateSerialNumberRange API Method. The serial numbers expire after 10 minutes, I've set this to a low number initially so you can see the UI response when submitting an expired Serial Number. If you'd like to change this, there's a setting in the appSettings.json file called "SerialNumberExpiryMilliseconds" which can be changed. Once you have a valid serial number you can then use this on the UI to submit a Prize entry.

When you have some prize entries submitted via the UI or alternatively directly through the swagger interface using the /PrizeDraw/SubmitEntry API method, you can then use the swagger interface to view all prize entries using the /PrizeDraw/GetAllEntries.

## User Interface

The user interface has been written using the VueJs CLI and Visual Studio Code. It uses Boostrap for styling and Responsive layout. I have also used some third party plugins via npm such as moment JS for date object creation and axios for communicating with the API.

### Requirements

    * Node.js is required for the NPM package management and server

### Usage 

Firstly, install all third party packages by running the command

    'npm install'

To run the site you'll then need to run the command

    'npm run serve'

The server should host the site on http://localhost:8080 which can be entered into any browser.

The UI will instantly try and connect to the API to get config information, so ensure the API is running first. Alternatively refresh the page to initialise the site.




