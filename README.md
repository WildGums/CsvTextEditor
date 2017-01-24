# CsvTextEditor

This repository contains code for a CsvTextEditor control as well as a simple application to show case the control.

The application is built using [Orchestra](https://github.com/WildGums/Orchestra)

The control uses [AvalonEdit](http://avalonedit.net/) and loosely implements [Elastic-tabstops](http://nickgravgaard.com/elastic-tabstops/) in the background (i.e. instead of tabstops we use commas as the separators).

We have purposely kept the control simple. We welcome all pull requests, however please discuss large feature implementations ahead of time.

## Goal

We work with a lot of csv files on a daily basis and wanted a no-nonsense, quick and simple tool to edit csv files.

We found that Excel reformats csv files  when saving a file, which causes us all sorts of issues. (Like changing date formats and stripping out leading '0').

Essentially CsvTextEditor is a simple "text editor" with some extra features that make it easy to edit csv files and will not reformat or change the format in any way.

### Limitations

#### Speed

The control is effective with relatively small csv files but struggles with large files.

Our use case is typically 20 columns (or less) and a few hundred rows.

We would welcome any efforts to improve the performance if someone was willing to tackle this issue.

#### Csv format

We only support comma separated files. (i.e. we expect the csv files to be fairly clean and do not support quotes or imbedded commas within the text.)

We would also welcome a PR that allowed CsvTextEditor to handle more cases.

## Features

- All the features available in AvalonEdit are also available in CsvTextEditor
- Display the columns using elastic formating
- Delete lines ("CTRL +L")
- Duplicate lines ("CTRL + D")
- Add columns (",")
- Delete columns ("CTL + ,")
- Search and replace ("CTRL + F") (Search will also highlight all occurrences in the file)
- Line and column number are in the status bar (bottom right corner)
- Highlight word (Just select some characters or double click on a word and the same occurrences will be highlighted.)
- Easy navigation between "cells" (Arrows, Tab, SHIFT + Tab)
- Undo/Redo
- Save
- As you add more characters to a column it will automatically expand the whole column
- Syntax highlighting (Numbers are shown in blue font)
- Automatically highlight True/False values
- Word hints are displayed as you type based on words that are already in the same column

![CsvTextEditor screenshot](doc/images/CsvTextEditor.png)

## License

MIT License