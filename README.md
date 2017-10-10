# Compiler Tools

This project contains tools and algorithms for studying and learning compiler
construction and formal languages. The tools provided are **not** intended
for mainstream, professional or commercial use (although the project is licensed
MIT so do whatever you want at your own risk. 

## Project structure

This project is a Visual Studio Solution with several C# 4.5 projects (some
libraries and some console applications so far). To build it, only the standard
.NET framework core is necessary. 

### Grammars

This library contains an embedded DSL to define grammars (in principle, CFGs)
in C#, that can be later analyzed, transformed and used in other ways to
build parsers, generators, etc. The DSL makes heavy use of operator overloading
and other fluid patterns. Documentation is on its way...

### TopDownParsing 

This library contains tools and algorithms to implement Top-Down recursive
and/or predictive parsers (so far requiring LL(1) grammars). Among these tools
are the algorithms for building the LL(1) table and determining if a grammar
is indeed LL(1).

### Other stuff

There are also some examples of usage of the previous tools.

## Collaboration

Fork, fix, and submit a pull request; or submit an issue. Valuable contributions
from students (either in code form or simply posting an issue) could potentially
be traded for additional points, questions in an exam, or something else...