# SleepStudy-Timeliner

SleepStudy Timeliner was designed and created for all DFIR analysts who want to parse ETL files located in:

- C:\Windows\System32\SleepStudy
- C:\Windows\System32\SleepStudy\ScreenOn

It is a GUI tool written in C# .Net Framework 4.7.2. In order to conver ETL files to CSV format I used https://github.com/forensiclunch/ETLParser. Then the tool takes the CSV output, filter logs providing valuable forensics data and saved them to a seperate file named Timeline_SleepStudy.csv. The timeline is in the TLN format. 

It was tested on:

- Windows 10.0.16299,

# How does it work?
First you have to provide a path to the SleepStudy folder that you want to parse. If inside that foler there is a folder called ScreenOn, the files located there will be parsed as well. Then you also have to provide the path where you want to save the output (timeline in TLN format) and click "PARSE".

![alt text](https://github.com/gajos112/SleepStudy-Timeliner/blob/main/images/0.png?raw=true)

![alt text](https://github.com/gajos112/SleepStudy-Timeliner/blob/main/images/1.png?raw=true)

![alt text](https://github.com/gajos112/SleepStudy-Timeliner/blob/main/images/2.png?raw=true)

![alt text](https://github.com/gajos112/SleepStudy-Timeliner/blob/main/images/3.png?raw=true)
