# SleepStudy-Timeliner

SleepStudy Timeliner was designed and created for all DFIR analysts who want to parse ETL files located in:

- C:\Windows\System32\SleepStudy
- C:\Windows\System32\SleepStudy\ScreenOn

It is a GUI tool written in C# .Net Framework 4.7.2. In order to conver ETL files to CSV format I used https://github.com/forensiclunch/ETLParser. Then, the tool takes the CSV output, and filter logs providing valuable forensics data.

It was tested on:

- Windows 10.0.16299,
