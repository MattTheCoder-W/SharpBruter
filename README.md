# Sharp Bruter - Brute Force in C#

- [Requirements](##Requirements)
- [Usage](##Usage)
- [Arguments](##Arguments)
    * [Optional Arguments](###Optional%20Arguments)
- [Tested On](##Tested%20on)

---

<br/>

![UsageGif](https://i.ibb.co/YDpHc0t/output2.gif)

<br/>

## Requirements:

* Mono (or other software for running .exe c# files)

<br/>

## Usage:

To use this script:

* Download **repository**
* Open **terminal**
* Go to **downloaded directory**

Type

> mono SharpBruter.exe /i

This will open `interactive` mode of this script.

To use standard mode, type:

> mono SharpBruter.exe /h

Help page will popup.

<br/>

## Arguments:

Argument | Description
:---: | :---:
`/u` | Url for target file on website (with http://)
`/l` | Username for website form
`/p` | Password dictionary
`/headers` | POST requests data headers, more info in `/h`

<br/>

### Optional Arguments:

Argument | Description
:---: | :---:
`/v` | Verbose mode to display percentage and info while brute forcing<br/>(It might slow down script a little bit)
`/h` or `/help` | Help page with definitions of script arguments

<br/><br/>

## Tested on:

* Windows 10
* Linux 20.04
* Linux 18
