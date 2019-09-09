Cool Compiler
=============

This project is a compiler of the Cool (view [*The Cool Reference Manual*](https://theory.stanford.edu/~aiken/software/cool/cool-manual.pdf)) language programming

Project structure
-----------------

This project is a Visual Studio Solution with one **C# 4.7.1** project.

Lexer and Parsing
-----------------

We use the parser generator **ANTLR 4.7.1**.

Semantics
---------

Use of the visitor pattern to travel around the Abstract Syntax Tree AST.

Optimization Code
-----------------

Developing...

Code Generation
---------------

At the first Three Address Code (TAC) is generated and after MIPS code is generated too.

Unit Testing
------------

The project contains some unit testing for Lexer, Parsing, Semantics and Topological Sort algorithm. The test cases for each unit testing are in the folder TestCases.

Collaboration
-------------

Fork, fix, and submit a pull request; or submit an issue. Valuable contributions
from anybody are good received.

Authors
-------

Two students of 4th year of Computer Science at the University of Havana:

Iván Galbán Smith <ivan.galban.smith@gmail.com>

Yanoel Llano Boitel <y.llano@estudiantes.matcom.uh.cu>
