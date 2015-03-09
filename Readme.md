# West Wind DeleteFiles
**Windows utility to delete files and folders recursively, optionally based on a time offset with support for deep file nesting**

This small Console utility deletes files and folders recursively and allows filtering files by date.

* Delete files with simple file specs like `c:\temp\*.*`
* Recursively delete files down the folder hierarchy (-r switch)
* Optionally delete empty folders (-f switch)
* Filter by files to delete based on days or seconds before current time (-d -s)
* Files can be optionally dumped into the Recycle Bin for recoverable deletes (-y)
* Works with deeply nested folder hierarchies (like NPM folders)
* Works a round Windows MAX_PATH limitations
* Portable, single-file EXE file

## Usage
Command Line Options:

```
DeleteFiles <filespec> -r -f -y -l -d10 -s3600

Commands:
---------
HELP || /?      This help display           

Options:
--------
pathSpec    FilePath and File Spec. Make sure to add a filespec
-r          Delete files [R]ecursively     
-f          Remove empty [F]olders
-y          Delete to Rec[Y]le Bin (can be slow!)
-l          Trial run - [L]ists items that would be deleted
-q0..2      Quiet mode: -q0 - all (default)  -q1 - No file detail
                        -q2 - No file detail, no summary
-dXX        Number of [D]ays before the current date to delete            
-sXX        Number of [S]econds before the current time to delete
            (seconds override days)
            if neither -d or -s no date filter is applied (default)

Examples:
---------
DeleteFiles c:\temp\*.* -r -f        - deletes all files in temp folder recursively 
                                       and deletes empty folders
DeleteFiles c:\temp\*.* -r -f -d10   - delete files 10 days or older 
DeleteFiles c:\temp\*.* -r -f -s3600 - delete files older than an hour
DeleteFiles ""c:\My Files\*.*"" -r   - deletes all files in temp folder recursively
```


## Requirements
* [Requires the .NET Framework 4.0](http://www.microsoft.com/en-us/download/details.aspx?id=17851)

## Acknowledgement
* Uses [ZetaLongPath](http://zetalongpaths.codeplex.com) by Zeta GMBH for long path deletion

## License
Copyright © 2012-2015 Rick Strahl, West Wind Technologies<br/>
[http://weblog.west-wind.com/](http://weblog.west-wind.com/)

This utility is provided as is, free of charge under MIT licence, and is provided without warranty. You can use it as you see fit with no limitations and the source code provided can be modified as needed. If you make changes or fixes please put in a pull request or open an issue to get any changes added.

If you're feeling generous and you find this tool useful consider making a small donation:

* [Make a donation for DeleteFiles using PayPal](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=3CY6HGRTHSV5Y)


This tool is published under MIT license terms:

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

### History

##### Version 1.10
*Feb. 28th, 2015*

* **Support for paths longer than MAX_PATH**<br/>
You can now delete deeply nested paths (such as nasty NPM hierarchies). Integrated with [ZetaLongPaths](https://github.com/UweKeim/ZetaLongPaths/) to provide long path support on all directory and file delete operations.

* **Delete ReadOnly, System and Hidden Files**<br/>
Previously read-only files wouldn't delete. This updates now deletes any kind of file regardless of attribute settings. Locked and access denied files still will not work, but everything else will delete.

* **Quiet Mode**<br/>
Add -q0..2 switch to allow quiet operation that runs without showing every file and folder deleted. -q0 shows all - q1 only shows summary, -q2 shows only startup banner.


