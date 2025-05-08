# Old Phone Pad Converter

A C# program for converting old phone keypad inputs into text messages.

## Overview

This program converts inputs similar to keypad inputs on old phones into readable text. For example, pressing “4433555 555666#” on an old phone keypad would produce "HELLO".

## Features

- Converts phone keypad numeric inputs to text
- Handles spaces between repeated keys
- Supports backspace character (*) for corrections
- Processes inputs from files for batch conversion
- Provides detailed error messages for invalid inputs

## Input Format
- Inputs can only contain digits (0-9) & spaces representing pauses & * representing backspaces 
- Please see character mapping below:
  - 1: &'(
  - 2: ABC
  - 3: DEF
  - 4: GHI
  - 5: JKL
  - 6: MNO
  - 7: PQRS
  - 8: TUV
  - 9: WXYZ
  - 0: [space]


## Usage

Run the application and follow the prompts:
OldPhonePadChallenge.exe
The application will display instructions and continuously prompt for input until you type "exit" or "quit".

If you require a copy of the .exe please reach out to James Fry


### Examples

Input | Output
----- | ------
44 444# | HI
4433555 555666# | HELLO
227*# | A
22#*# | ERROR: Invalid key press

## Requirements

- .NET Framework 4.7.2 or later
- C# 7.0 or later
