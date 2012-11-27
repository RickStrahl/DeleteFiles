#West Wind DeleteFiles
**A small, recursive Console File Delete Utility**
* * * 

This small Console utility deletes files recursively and allows filtering files by date. 

* Allows for a filespecs in the form of c:\temp\*.*
* Can recursive delete files and optionally delete empty folders
* Files can be optionally dumped into the Recycle Bin
* Date filtering based on days or seconds before current time

##Usage Options
Command Line Options:
	DeleteFiles <filespec> -r -d10 -t12/1/2012

	Commands:
	---------
	HELP || /?  This help display           

	Options:
	--------
	pathSpec    Path and File Spec. Make sure to add a filespec
                 (use quotes around paths that contain spaces)
	-r          Delete files recursively     
	-f          Remove empty Folders
	-y          Delete to Recyle Bin (can be slow!)
	-dXX        Number of days before the current date to delete            
	-sXX        Number of seconds before the current time to delete
				(seconds override days)
				if neither -d or -s no date filter is applied

	Examples:
	---------
	DeleteFiles c:\temp\*.* -r         - deletes all files in temp folder recursively
	DeleteFiles c:\temp\*.* -r -d10    - delete files 10 days or older
	DeleteFiles c:\temp\*.* -r -s3600  - delete files older than an hour
	DeleteFiles "c:\My Files\*.*" -r - deletes all files in temp folder recursively


##Requirements
[Requires the .NET Framework 4.0](http://www.microsoft.com/en-us/download/details.aspx?id=17851)

##License
This tool is published under MIT license terms:

Copyright © 2012 Rick Strahl, West Wind Technologies, http://west-wind.com/

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
associated documentation files (the "Software"), to deal in the Software without restriction, 
including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial 
portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.