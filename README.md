<h1> Performance Aware Programming </h1>

This is the first assignment for the [Performance Aware Programming series by Casey Muratori](https://www.computerenhance.com/p/table-of-contents).

"This series is designed for programmers who know how to write programs, but don’t know how hardware runs those programs.
It’s designed to bring you up to speed on how modern CPUs work, how to estimate the expected speed of performance-critical code, and the basic optimization techniques every programmer should know."

This assignment is part of building a virtual CPU, specifically the [Intel 8086](https://en.wikipedia.org/wiki/Intel_8086).
In this assignment I simulate an instruction decode, specifically the mov instruction.

[The MOV instruction copies a word or byte of data from a specified source to a specified destination. 
The destination can be a register or a memory location. The source can be a register, a memory location or an immediate number.](https://www.pcpolytechnic.com/it/ppt/8086_instruction_set.pdf)

In order to simulate this, we read in a binary file with a list of instructions. Each instruction consists of 2 bytes i.e:

**0001011 11001011**
and decode the bytes to produce an 8086 assembly instruction:
**mov cx, bx**

<h2> To run the program: </h2>

This program is written in c#.
Donet 8 is a requirement to run this program.

dotnet run [binary file]

2 binary files are provided:

**listing_0037_single_register_mov**
**listing_0038_many_register_mov**

When running the program with one of the provided files, mov assembly instructions will be printed to the console.

There are two files with an example of what the correct output for the decoded binary files should be:

**listing_0037_single_register_mov.asm**
**listing_0038_many_register_mov.asm**





