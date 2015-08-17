# F# Workshop Notes

## Introduction

- Statically typed
- Type interence engine
- Promotes immutability, can't mutate state
- Whitespace _is_ significant
- Single-pass compiled; starts from the start and ends at the end. You can't call things unless they are defined. Can only use what you have defined.

## Using the REPL (F# interactive)

If you want to evaluate something in the REPL, append a double semicolon at the end of the line:

```
> let b = 10;;
```

Otherwise, select your code in the editor and hit `^⏎` to evaluate it in the REPL

## Functions

```
let a = 1
```
This is **not a function**. This is just a _binding_ of 1 to the name _a_.

```
let increment a = a + 1
```

This **is a function**. Functions are _first-class_. The syntax is identical as the binding above. Increment is the name, and it takes an argument `a`. The result? Add 1 to a.

Now calling it:

```
> increment 10;;

val it : int = 11
```

Functions allow a lot of characters in their names, can even use whitespace using backticks

```
let ``increment my value`` a = a + 1
```

Pretty nice to for tests, as we can have punctuation.

### Types and type inference

Type signature of the function of `a` was inferred:

![](http://puu.sh/jE86v/af8d6dcdf6.png)

Takes an `int` and returns an `int`

```
int -> int
```

It's inferred here, but you can be explicit:

```
let increment' (a : int) : int = a + 1
```

![](http://puu.sh/jE8a5/079f6c7beb.png)

As you can see, they are the same signatures

Changing the increment to `1.0` would infer `a` as a `float` instead:

![](http://puu.sh/jE8cs/3e1ed48bb4.png)

You can also specify return types

```
let increment''' : int -> int = fun a -> a + 1
```

The `int -> int` means that you constrain the signature, and here we're constraining it using an anonymous function (below).

The `int -> int` belongs to the `increment'''` binding, not the anonymous function:

```
let increment''' : int -> int
```

#### Anonymous function

You can also have anonymous functions using the `fun` keyword

```
let increment'' = fun a -> a + 1
```

Same type signature, it's inferred from the anonymous function `fun a -> a + 1`:

![](http://puu.sh/jE8na/c99c5d649d.png)

### Currying and signatures with multiple parameters

Functions only take **one** argument. Add `a` is a argument that returns another argument. Remember, from LSD, this concept is known as **currying**.

![](http://puu.sh/jE8GG/c0268f3b9b.png)

Think of it like:

```
  a       b      ret
(int -> (int -> int))
            fn
      fn
```

This means we can **call a function with an incomplete set of arguments**:

```
> add 1;;

val it : (int -> int) = <fun:it@52-5>
```

Returns a partially complete function, still needs a new parameter. If we bind this partial function to a name, we can partially apply the function, say `inc` to `add 1`, then using `inc 3` to return `4`:

```
> let inc = add 1;;

val inc : (int -> int)

> inc 3;;

val it : int = 4
```

Hence, we can **pass functions around** partially.


When we're calling a function, we don't need to add the parens. But the evaluation is LTR; the evaluation to `f` takes precedence over the `+ 2`. The `(fun x -> x * 2)` is the `f` which we pass in `n` to.

```
let applyFunctionThenAdd2 f n = 
    f n + 2

Examples.test "Multiply by two then add two" (fun () ->
    applyFunctionThenAdd2 (fun x -> x * 2) 10 = 22
)
```

#### Pipelining 

Pipeline operator (`>|`) flips the function call around:

Thus: `x |> f = fx`:

```
let examplePipe n =
    n
    |> add 10      (* n is piped into here as add 10 n        *)
    |> increment   (* output of above is piped into increment *)
    |> add 20      (* output of increment is piped to add 20  *)
```

This is essentially:

```
(add 20 (increment (add 10 n)))
```

As you can see, we would evaluate `(add 10 n)`, then pass that result to `increment` then pass that result to `add 20`.

#### Custom Operators

We can define custom operators wrapped in parens:

```
// the pipe operator has a very simple implementation
// try implementing ||> to do the same things as pipe
let (||>) x f = f x
```

Here we just implemented the pipe operator

### Scoping

Inner functions hide their scope from outer functions

### Recursion

If you want to do easy recursion, by using `rec` keyword.

If the last thing in your recursive function calls the recursive function itself, then the compiler will optimise it. You **will not build up the stack** by doing this because F# will handle it.

![](http://puu.sh/jEa2u/98e82963ff.png)

I was missing the `rec` keyword before the `factorial` definition

### Conditional branching

When conditional branching, your return must be the same on all branches.

```
if predicate exp1 exp2 then
  "hello"
else
  "world"
```

is okay but

```
if predicate exp1 exp2 then
  1234
else
  "world"
```

isn't.

## Tuples

- Tuples are a comma separated collection of values.
- Useful for passing stuff around

The signature of tuples are inferred with `*`, not a `->` like functions:

```
val vect1 : float * float = (5.0, 14.5)
val fileSize : string * int = ("/var/log/err.log", 452264)
```

### Decomposing / pattern matching

We can break down tuples into different labels:

```
> let fileSize   = ("/var/log/err.log", 452264);;

val fileSize : string * int = ("/var/log/err.log", 452264)

> let file, size = fileSize;;

val size : int = 452264
val fileName : string = "/var/log/err.log"
```

Don't care about the size of that file? Use an `_` to ignore it when decomposing:

```
> let fileName', _ = fileSize;;

val fileName' : string = "/var/log/err.log"
```

Tuples used for C# `out` references (to avoid state change):

```
> let parsed = System.Int32.TryParse "1234";;

val parsed : bool * int = (true, 1234)

> let parsed = System.Int32.TryParse "banana";;

val parsed : bool * int = (false, 0)
```

Now we can use decomposing to pluck out the `true` and `1234` or `false` and `0` in either case:

```
> let didItParse, _ = parsed

val didItParse : bool = false
```

## Records

- Like tuples but with named fields
- Essentially, structs
- Records are composable; can have a record in a record
- Records are **immutable**

Use the keyword `type` to create a new Record type

```
type Coord2d = { x : float; y : float }
let vect1    = { x = 3.5  ; y = 5.6   }
```

We create **values** (not *instances*) of `Coord2d`. The type inference will match `vect1` with `Coord2d`. The inference engine will use the most recent definition of the Record, so if we duplicated `Coord2d` to `Vector` just underneath `Coord2d`, then `Vector` would be matched instead.

```
type Player = { name: string;    coordinates: Coord2d   }
let someone = { name = "Freddy"; coordinates = vect1    }
```

### Pseudo-mutability with `with` keyword

Remember, it's not mutable. So, we take an existing value and create a new one to, say, move `someone` somewhere else:

```
let someoneMoved = { someone with coordinates = { x = 0.0; y = 0.0 } }
```

We can use a new line instead of a semicolon:

```
let vect2 = { vect1 with 
                        x = 0.0
                        y = 0.0 }
```

If we want to reference out own value that we're 'mutating', we can do that too:

```
let someoneMoved = { someone with coordinates = { x = coordinates.x + 1; y = 0.0 } }
```

### Deconstructing Records

We can deconstruct records too, just like tuples:

```
let someonesCoordinates = someone.coordinates;
```

### Equality Checks

Records have **structural equality** semantic. Compared on the values. Unlike F# classes, which is **referenced equality**, compared instances on the references in memory.

```
type Coord2dClass(x : float, y : float) = class
    member m.X = x
    member m.Y = y
end

let v1 = new Coord2dClass(5.0, 5.0)
let v2 = new Coord2dClass(5.0, 5.0)

v1 = v2   (* val it : bool = false *)
```

vs

```
let v1 = { x = 5.0; y = 5.0 }
let v2 = { x = 5.0; y = 5.0 }

v1 = v2   (* val it : bool = true *)
```

We only really use classes for C# interoperability—you don't *usually* use classes.

## Discriminated Unions (DU)

- A bit like `enum`s but a whole lot more powerful

```
type Colour =
  | Red
  | Blue
  | Yellow
```

Each type labels/case constructor (Red, Blue, Yellow) of the type Color. We can't create Color, we can only use Red/Blue/Yellow.

```
let stopSign = Red
```

This means `stopSign` is not of type `Red`, but of type `Color` constructed with `Red`.

### Pattern match expression

Use the `match` keyword to match a DU with its type labels.

```
let describeColour c =
  match c with
  | Blue   -> "It's blue"
  | Red    -> "It's red"
  | Yellow -> "It's yellow"
```

Similar to conditional branching, every match but be of the same type. 

Inference is done top-to-bottom, as is evaluation.

```
let describeColour c =
  match c with
  | Blue   -> "It's blue"
  | Blue   -> "IT IS BLUE"
  | Red    -> "It's red"
  | Yellow -> "It's yellow"
```

The match `"IT IS BLUE"` won't be ever be used, since evaluation is done top down (the lowercase one will be used instead).

We can also remove the `match c with` by using the lambda `function` keyword instead:

```
let describeColour' = function
  | Blue   -> "It's blue"
  | Red    -> "It's red"
  | Yellow -> "It's yellow"
```

This will lead to anonymous arguments being used:

```
val describeColour' : _arg1:Colour -> string
```

Remember, F# only takes one argument, which is why we have `_arg1`.

Lambda `function` is just a _syntax shortcut_ for `match`.

### Data in DUs

Each of our cases can take data along with the case name.

```
type ContactDetails =
  | Email of string
  | Phone of int
```

We would use a Record or Tuple if we wanted **both** the `Email` and `Phone`. But here, we just one one **or** the other.

Hence:

```
type Person = { Name : string; ContactDetails : ContactDetails }
```

A person's contact details could be `Email` or `Phone`, but not both.

To create a DU with data, it's similar syntax to a function call:

```
let jim  = { Name = "Jim";  ContactDetails = Email "jim@example.org" }
let tess = { Name = "Tess"; ContactDetails = Phone 0411222333 }
```

### Equality

Like Records, we have value equality for DUs and not reference equality.

### Optional DUs with no associated data

Our system now supports `Nothing` for no contact details. This type case has **no data** associated to it.

```
type ContactDetails =
  | Email of string
  | Phone of int
  | Nothing
```

Now we can print using `_` if there is no associated data (like a default) in a pipeline provided we pass in a ContactDetails:

```
let printContactDetails = function
  | Email e -> sprintf "email address - %s" e
  | Phone p -> sprintf "phone number - %010d" p
  | Nothing ->         "no contact details found"
```

We can't do `null` since that would be a pointer, but this is value-based semantics.

### Nested case constructor

Assume we broke out the `Nothing` into:

```
type ContactDetails =
  | Email of string
  | Phone of int

type MaybeContactDetails =
  | Nothing
  | Details of ContactDetails
```

This would mean we can construct these types either as:

```
(* Idiomatic approach *)
let contactDetails = Email "jim@example.org" |> Details

(* Not so idiomatic approach but still okay *)
let contactDetails = Details (Phone 0411222333)

(* Nothing! *)
let contactDetails = Nothing
```

This is because we're wrapping `Details`.

#### Deconstructed Pattern Matching

We can now **deconstruct** on pattern matching and go as deep into the structure as possible:

```
let howToContact (person : Person) =
	(* Contact match intermediate using deep matching *)
	let contactMatch =
		match person.ContactDetails with
		  | Details (Email e) -> sprintf "email address - %s" e
		  | Details (Phone p) -> sprintf "phone number - %010d" p
		  | Nothing           -> "does not want to be contacted"
	sprintf "%s %s" person.Name contactMatch
```

## Optionals

The option type is defined like this (the `'a` is a type parameter)

```
type Option<'a> =
    | None
    | Some of 'a
 ```
 
Now all we do when we define our fields, we append `option` keyword:
 
``` 
type ContactDetails =
  | Email of string
  | Phone of int

type Person = { Name : string; ContactDetails : ContactDetails option }
```

Thus, we use an idiomatic nested case constructor and pass it into `Some` or `None`:

```
(* Essentially Nothing *)
let contactDetails = None

(* Email with Some *)
let contactDetails = Email "jim@example.org" |> Some

(* Phone with Some *)
let contactDetails = Phone 0411222333 |> Some
```

## Guard clauses

Attaches a predicate to the end of a pattern match.

Cannot change the binding, only matches more restively and less often.

Use the `when` keyword in a pattern match will apply the pattern only when the `when` predicate is true:

```
let partition' n =
  match n with
  | x when x < 0 -> "negative"
  | x when x > 0 -> "positive"
  | _            -> "zero"
```

It's a trade-off to use guard clauses—you have to use an exhaustive `_` wildcard *without* a guard.

## Pattern matching in arguments

You can just elevate pattern matches in the argument:

```
let thirdElementIsEven (_, _, n) = 
    n % 2 = 0
```

This will let the inference engine to do the matching for you.

## `choice`

A choice is one or the other, but not nothing

A reusable union type

```
type Choice<'a, 'b> = 
  | Choice1Of2 of 'a
  | Choice2Of2 of 'b
```
e.g.:

```
type PostagePrice = 
    | Dollars of decimal
type PostageError = 
    | TooBig 
    | TooHeavy

(* Takes a PostageSatchel and returns a choice between a Price or Error *)
type PostagePriceCalculator = 
	PostageSatchel -> Choice<PostagePrice, PostageError>
 Choice1Of2 (Dollars 12M)
```

and use it like:

```
10M      |> Dollars |> Choice 1Of1 (* $10 as Choice1      *)
TooBig   |> Choice2Of2             (* TooBig as Choice2   *)
TooHeavy |> Choice2Of2             (* TooHeavy as Choice2 *)
```

## Lists

In F#, a list is a set of tuples, `[H|Tail]`, where `H` is `'T` and `Tail`. The tail is a second set of Tuples and so on an so forth, that is:

```
(1 (2, (3, 4))) etc.
```

or as per the F# definition:

```
type List<'T> =
 (* Empty list *)
 | ([]) of List<'T>
 (* A list is a tuple *)
 | (::) of Head: 'T * Tail: List<'T>
```

### List definition types

- **empty**: `[]`
- **cons**: `1 :: 2 :: 3 :: 4 :: []` (end the list with an empty list)
- **explicit**: `[ 1; 2; 3; 4 ]` (remember a `;` can be substituted with a newline)
- **ranges**: `[ 1 .. 100 ]`
- **ranges with +ve steps**: `[ 0 .. 2 .. 10 ]`, even numbers 0 to 10
- **ranges with -ve steps**: `[ 10 .. -2 .. 0 ]`, even numbers 10 to 0

### List comprehensions

#### Concatenation

Concatenate a list using `@`

```
let appendedList  = [ 1; 2; 3 ] @ [ 4; 5; 6 ]
```

#### Iteration

To for loop

```
[ for i in 1 .. 10 -> somethingThatUses i ]

or

[
	for i in 1 .. 10 do
	  yield somethingThatUses i
]
```

Down for loop

```
[ for i in 10 .. -1 .. 1 -> somethingThatUses i ]

or

[
	for i in 10 .. -1 .. 1 do
	  yield somethingThatUses i
]
```

#### Splitting a list

```
x :: _                (* head of list *)
_ :: x                (* tail of list *)
_ :: _ :: _ :: x :: _ (* fourth item in list *)
```

#### Useful List Functions

##### Map 

```
[ 1 .. 10 ] |> List.map (fun x -> x * x)
```

##### Filter 

```
[ 1 .. 10 ] |> List.filter (fun x -> x % 2 = 0)
```

##### Sum 

```
[ 1 .. 10 ] |> List.sum
```

##### Length 

```
[ 1 .. 10 ] |> List.length
```

##### Reverse 

```
[ 1 .. 10 ] |> List.reverse
```

##### Reduce

Will successively apply a function to each item in the list

```
> [ 1 .. 10 ] |> List.reduce (fun acc x -> acc + x);;

val it : int = 55
```

##### Fold

Similar to reduce, but will apply a function to each item in the list, however it starts off with an initial value that is passed in and does not have to be of the same type as the list. 

Here we can see that it's returning a string from a list of numbers...

```
> [ 1 .. 10 ] |> List.fold (fun state x -> sprintf "%s %d" state x) "Numbers:";;

val it : string = "Numbers: 1 2 3 4 5 6 7 8 9 10"
```

#### List conversions

List of int to strings:

```
[ 1 .. 100 ] |> List.map string
```