# CNC_Analyser

## Table of Contents
- [Intro](#intro)
- [Setup](#setup)
- [Usage](#usage)

## Intro
Hello, this is my first solo project, a CNC analyser where it has these features:
- Compare two CNC files with tolerance
- Find Extremas of all axes in file
- Limit test of axes
- Graph for an axis analysis
- Simulation of how the axis moves

## Setup
In order the program to work, you **must** go to settings and click browse folder button.  
In the folder you select you **must** have these 3 files:
- Axes.txt
- ExternalExceptions.txt
- Limits.txt

After you select the folder, in Axes.txt these sections must be present:
- [Axes], [Params], [CourseSequence], [LayupStart], [LayupEnd] (With square brackets)

Here is an example of how Axes.txt should look.
[Axes.txt](./Test_Example/Axes.txt)

In **ExternalExceptions.txt** and **Limits.txt** you don't need maual writing, they will fill 
when you open the External Exceptions and Axes limit in settings.

## Usage
In order to have the desired results, you **MUST** have the correct foler selected.  
With compare you select two CNC files, set the tolerance, have the option to include External 
values and feedrate.  
With Axes extrema, limit text, simulation and graph you select only 1 file.  
Movement in simulator is WASD, Q for up, Z for down, and rotate with right mouse button.  
You can zoom in and zoom out in graph section.  
