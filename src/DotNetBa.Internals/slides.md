---
theme : "black"
transition: "zoom"
highlightTheme: "darkula"
separator: ^---$
verticalSeparator: ^--$
---

#### .NET BA 05/2019

# Increasing performance with ref-likes
### (safely)

#### by Marek Linka

--

## About me

* Senior software engineer @ ERNI
* Focus on up-and-coming .NET (Core, ASP.NET, ML.NET)
* Machine learning enthusiast and driver
* CoreFX and ML.NET contributor
* Debugger of the strange :)

----

https://twitter.com/mareklinka

--

## Agenda

* Ref-like constructs in C# 6, 7, 7.2
* `ref struct`
* `stackalloc`
* `Span<T>` and `Memory<T>`
* `readonly struct`
* IL code and benchmarks

---

## Passing stuff by reference

--

### Type system

* Types of types:
    * Reference type (classes)
    * Value type (structs)
    * Reference (internal, out of scope)
* Where are they allocated?
* Passing a reference type is different than passing a value type

--

### Structures

* *Typically* allocated on the stack, but
    * Boxing
    * Structs can implement interfaces
    * Structs can be large and copying them up and down is bad for performance

--

### References in C# < 7

* We had `ref` and `out` parameters for methods

----

``` csharp
void Func1(ref int X) { ... }

void Func2(out int X) { ... }
```

----

* This is rather limiting, you must declare a method to work with a reference to a struct

--

### C# 7 to the rescue

* Two new `ref`-related features
    * `ref` locals - you can store a reference in a local and work with it
    * `ref` returns - you can return references from functions
* Both features primarily targeted at working with large structs
* `Span<T>` and `Memory<T>`
    * A safe abstraction over arbitrary memory
    * A lot of limitations, a lot of advantages

--

### C# 7.2 brings even more

* Readonly structs
* Readonly ref locals and returns
* ref ternary operator

--

### But why bother?

* Performance
* These features are not aimed at a generic application developer
* However, they are still very useful and will see more usage across the framework in the future
* It's useful to know when to use them, how, and how they work

---

## Let's dive in