Keen .NET SDK
============

Usage
-----

The Keen IO .NET SDK is used to do custom analytics and event tracking for .NET applications. Use this SDK to capture large volumes of event data such as user actions, errors, server interactions, or any arbitrary event you specify. The SDK posts your events to Keen IO, a highly available, scalable cloud datastore. See [Keen IO docs](https://keen.io/docs) for instructions on extracting, querying, and building custom analytics with your data.

Requirements
------------

The SDK was written for .NET v4.5.


Installation
------------

The easiest way to get started with the Keen IO .NET SDK is to use the [KeenClient NuGet package](http://www.nuget.org/packages/Keen.NET/). 

That can be installed from the Package Manager Console in Visual Studio with the command :

```
  PM> Install-Package Keen.NET

```

Initializing the Library
------------------------

```
  var prjSettings = new ProjectSettingsProvider("YourProjectID", writeKey: "YourWriteKey");
  var keenClient = new KeenClient("YourProjectID", writeKey: "YourWriteKey", readKey: "YourReadKey", masterKey: "YourMasterkey");
```

Recording Events
----------------

Event data is provided to the client as an object. A simple way to do this is with an anonymous object:

```
  var eventPurchase = new
    {
        category = "magical animals",
        username = "hagrid",
        price = 7.13,
        payment_type = "information",
        animal_type = "norwegian ridgeback dragon"
    };
    
  keenClient.AddEvent("purchases", eventPurchase);
```

Full Example
------------

```
  static void Main(string[] args){
      // Set up the client
      var keenClient = new KeenClient("YourProjectID", writeKey: "YourWriteKey");

      // Build an event object
      var eventPurchase = new
      {
          category = "magical animals",
          username = "hagrid",
          price = 7.13,
          payment_type = "information",
          animal_type = "norwegian ridgeback dragon"
      };

      // send the event
      keenClient.AddEvent( "purchases", eventPurchase);
  }
```

