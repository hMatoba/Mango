Mango
=====

|Build Status|


MongoDB utility in .NET Core

How to Use
----------

::

  [MongoDoc]
  class Model1
  {
      [BsonId]
      [BsonRepresentation(BsonType.String)]
      public string Id { get; set; }

      [BsonElement("token")]
      [BsonRepresentation(BsonType.String)]
      public string Token { get; set; }
  }

  // Create collection(s)
  MongoInitializer.Run(connectionString, dbName, assemblyName, modelsNamespace);

License
-------

This software is released under the MIT License, see LICENSE.txt.

.. |Build Status| .. image:: https://travis-ci.org/hMatoba/Mango.svg?branch=master
    :target: https://travis-ci.org/hMatoba/Mango
