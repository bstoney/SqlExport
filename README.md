# SqlExport

This is an experimental application. It has been built over many years and iterations as a way of experimenting with new
.NET language features or verifying design patters. 

The the primary goal of the application is to provide a way of exporting data from different relational data source. 
Unlike other applications it tries to be data source agnostic and provides an adapter interface that can be implemented
to provide access any data, though the results are expected to be displayed in a tabular format e.g. a relational
database or folder of CSV files. 

The secondary objective is to support large result sets. By large I mean many millions of rows, I have not used it with
any result sets greater than a billion rows. 

This is a personal project I built and refactored many times as my needs and development experience changed. There are
many inconsistencies in the coding style and different approaches to solving problems. As this is an experiment there
are no tests. I hope someone else can finds something useful here.

## Features and other stuff

* Multiple configurable data sources (these all work to a greater or lesser extent)
  * SQL Server
  * ODBC
  * Ocacle
  * Sybase
  * SqlLite
  * Text - custom SQL for reading a folder containing CSV or other formatted text data
  * Linq - limited C# Linq syntax support

* Supports adding new data sources via adapter interfaces
* Large result sets
* Hopefully low memory foot print
* GUI written in WPF and Windows Forms
* Syntax highlighting 
* Multiple connection sessions
* Multiple result displays
* Console interface that handles even larger result sets
* User configuration
